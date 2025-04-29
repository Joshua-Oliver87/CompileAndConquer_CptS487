using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Xna.Framework;
using CompileAndConquer.Content.EnemyWaveManager.entryPatterns;
using CompileAndConquer.Content.EnemyWaveManager.exitPatterns;
using CompileAndConquer.SpawnPatterns;
using CompileAndConquer.Entities.Enemy_Factories;
using CompileAndConquer.Content.MovementTypes;
using CompileAndConquer.Content.AttackPatterns;
using CompileAndConquer.Content.MovementTypes.BulletMovementTypes;

namespace CompileAndConquer.Content.EnemyWaveManager
{
    public class WaveBuilder
    {
        public WaveManager.Wave Build(JsonElement waveJson)
        {
            var wave = new WaveManager.Wave();
            
            if (waveJson.TryGetProperty("Steps", out JsonElement stepsJson))
            {
                foreach (JsonElement stepJson in stepsJson.EnumerateArray())
                {
                    var waveEvents = new List<WaveManager.WaveEvent>();
                    
                    foreach (JsonElement eventJson in stepJson.EnumerateArray())
                    {
                        var waveEvent = BuildWaveEvent(eventJson);
                        if (waveEvent != null)
                        {
                            waveEvents.Add(waveEvent);
                        }
                    }
                    
                    wave.Steps.Add(waveEvents);
                }
            }
            
            return wave;
        }
        
        private WaveManager.WaveEvent BuildWaveEvent(JsonElement eventJson)
        {
            try
            {
                string enemyType = GetStringProperty(eventJson, "EnemyType");
                int spawnCount = GetIntProperty(eventJson, "SpawnCount");
                int duration = GetIntProperty(eventJson, "Duration");
                
                var enemyFactory = CreateEnemyFactory(eventJson);
                var entryFactory = CreateEntryFactory(eventJson);
                var exitFactory = CreateExitFactory(eventJson);

                if (enemyFactory == null || entryFactory == null || exitFactory == null)
                {
                    return null;
                }

                var waveEvent = new WaveManager.WaveEvent(
                    enemyFactory,
                    spawnCount,
                    duration,
                    entryFactory,
                    exitFactory
                );
                
                SetSpawnPattern(waveEvent, eventJson);
                
                return waveEvent;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error building wave event: {ex.Message}");
                return null;
            }
        }
        
        private IEnemyFactory CreateEnemyFactory(JsonElement eventJson)
        {
            string enemyType = GetStringProperty(eventJson, "EnemyType");
            
            switch (enemyType)
            {
                case "TypeA":
                    return new TypeAFactory(
                        TextureHandler.TypeAEnemy,
                        new Vector2(0, 0),
                        () => CreateMovement(eventJson),
                        () => CreateAttack(eventJson)
                    );
                case "TypeB":
                    return new TypeBFactory(
                        TextureHandler.TypeBEnemy,
                        new Vector2(0, 0),
                        () => CreateMovement(eventJson),
                        () => CreateAttack(eventJson)
                    );
                case "MiniBoss":
                    return new MiniBossFactory(
                        TextureHandler.MiniBoss,
                        new Vector2(0, 0),
                        () => CreateMovement(eventJson),
                        () => CreateAttack(eventJson)
                    );
                case "FinalBoss":
                    // Special handling for FinalBoss
                    var bossPhases = new List<BossPhase>
                    {
                        new BossPhase
                        {
                            Movement = new FinalBossMovement(),
                            Attack = CreateAttack(eventJson),
                            HpThreshold = GetIntProperty(eventJson, "HpThreshold", 2000)
                        }
                    };
                    
                    return new FinalBossFactory(
                        TextureHandler.FinalBoss,
                        new Vector2(0, 0),
                        bossPhases,
                        bossPhases[0].Attack
                    );
                default:
                    return null;
            }
        }
        
        private IMovementStrategy CreateMovement(JsonElement eventJson)
        {
            if (eventJson.TryGetProperty("Movement", out JsonElement movementJson))
            {
                string type = GetStringProperty(movementJson, "Type");
                
                switch (type)
                {
                    case "Linear":
                        bool startRight = GetBoolProperty(movementJson, "StartRight", true);
                        return new LinearMovement(startRight);
                        
                    case "Stationary":
                        return new StationaryMovement();
                        
                    case "Curved":
                        float horizontalAmplitude = GetFloatProperty(movementJson, "HorizontalAmplitude", 200f);
                        float verticalAmplitude = GetFloatProperty(movementJson, "VerticalAmplitude", 50f);
                        float horizontalFrequency = GetFloatProperty(movementJson, "HorizontalFrequency", 0.7f);
                        float verticalFrequency = GetFloatProperty(movementJson, "VerticalFrequency", 1.3f);
                        return new CurvedMovement(horizontalAmplitude, verticalAmplitude, horizontalFrequency, verticalFrequency);
                        
                    case "MiniBoss":
                        return new miniBossMovement();
                        
                    case "FinalBoss":
                        return new FinalBossMovement();
                        
                    default:
                        return new LinearMovement(true);
                }
            }
            
            return new LinearMovement(true);
        }
        
        private IAttackPattern CreateAttack(JsonElement eventJson)
        {
            if (eventJson.TryGetProperty("Attack", out JsonElement attackJson))
            {
                string type = GetStringProperty(attackJson, "Type");
                
                switch (type)
                {
                    case "None":
                        return new noAttack();
                        
                    case "TimedStraight":
                        float interval = GetFloatProperty(attackJson, "Interval", 0.5f);
                        float speed = GetFloatProperty(attackJson, "Speed", 250f);
                        return (IAttackPattern)new TimedStraightAttack(dir => new StraightBullet(dir, speed), interval);
                        
                    case "Triangle":
                        int bulletCount = GetIntProperty(attackJson, "BulletCount", 8);
                        float triangleInterval = GetFloatProperty(attackJson, "Interval", 10f);
                        float triangleSpeed = GetFloatProperty(attackJson, "Speed", 350f);
                        float delay = GetFloatProperty(attackJson, "Delay", 2.0f);
                        return new TriangleAttack(bulletCount, triangleInterval, delay, dir => new StraightBullet(new Vector2(0, 1), triangleSpeed));
                        
                    case "Circle":
                        int circleBulletCount = GetIntProperty(attackJson, "BulletCount", 24);
                        float circleInterval = GetFloatProperty(attackJson, "Interval", 30f);
                        float circleSpeed = GetFloatProperty(attackJson, "Speed", 200f);
                        return new CircleAttack(circleBulletCount, circleInterval, dir => new StraightBullet(dir, circleSpeed));
                        
                    default:
                        return new noAttack();
                }
            }
            
            return new noAttack();
        }
        
        private Func<Vector2, IEntry> CreateEntryFactory(JsonElement eventJson)
        {
            if (eventJson.TryGetProperty("Entry", out JsonElement entryJson))
            {
                string direction = GetStringProperty(entryJson, "Direction");
                float speed = GetFloatProperty(entryJson, "Speed", 120f);
                
                CardinalDirection dir = CardinalDirection.Top;
                switch (direction)
                {
                    case "Top":
                        dir = CardinalDirection.Top;
                        break;
                    case "Right":
                        dir = CardinalDirection.Right;
                        break;
                    case "Bottom":
                        dir = CardinalDirection.Bottom;
                        break;
                    case "Left":
                        dir = CardinalDirection.Left;
                        break;
                }
                
                return finalPos => new DirectionalEntry(dir, finalPos, speed);
            }
            
            return finalPos => new DirectionalEntry(CardinalDirection.Top, finalPos, 120f);
        }
        
        private Func<Vector2, IExit> CreateExitFactory(JsonElement eventJson)
        {
            if (eventJson.TryGetProperty("Exit", out JsonElement exitJson))
            {
                string direction = GetStringProperty(exitJson, "Direction");
                float speed = GetFloatProperty(exitJson, "Speed", 120f);
                
                CardinalDirection dir = CardinalDirection.Top;
                switch (direction)
                {
                    case "Top":
                        dir = CardinalDirection.Top;
                        break;
                    case "Right":
                        dir = CardinalDirection.Right;
                        break;
                    case "Bottom":
                        dir = CardinalDirection.Bottom;
                        break;
                    case "Left":
                        dir = CardinalDirection.Left;
                        break;
                }
                
                return finalPos => new DirectionalExit(dir, speed);
            }
            
            return finalPos => new DirectionalExit(CardinalDirection.Top, 120f);
        }
        
        private void SetSpawnPattern(WaveManager.WaveEvent waveEvent, JsonElement eventJson)
        {
            if (eventJson.TryGetProperty("SpawnPattern", out JsonElement spawnPatternJson))
            {
                string type = GetStringProperty(spawnPatternJson, "Type");
                
                switch (type)
                {
                    case "Line":
                        float y = GetFloatProperty(spawnPatternJson, "Y", 200f);
                        waveEvent.SpawnPattern = new LineSpawnPattern(y);
                        break;
                        
                    case "Pyramid":
                        int rows = GetIntProperty(spawnPatternJson, "Rows", 3);
                        float spacing = GetFloatProperty(spawnPatternJson, "Spacing", 40f);
                        float pyramidY = GetFloatProperty(spawnPatternJson, "Y", 200f);
                        waveEvent.SpawnPattern = new PyramidSpawnPattern(rows, spacing, pyramidY);
                        break;
                }
            }
            else
            {
                waveEvent.SpawnPattern = new LineSpawnPattern(200f);
            }
        }
        
        private string GetStringProperty(JsonElement element, string propertyName, string defaultValue = "")
        {
            if (element.TryGetProperty(propertyName, out JsonElement property))
            {
                return property.GetString() ?? defaultValue;
            }
            return defaultValue;
        }
        
        private int GetIntProperty(JsonElement element, string propertyName, int defaultValue = 0)
        {
            if (element.TryGetProperty(propertyName, out JsonElement property))
            {
                return property.GetInt32();
            }
            return defaultValue;
        }
        
        private float GetFloatProperty(JsonElement element, string propertyName, float defaultValue = 0f)
        {
            if (element.TryGetProperty(propertyName, out JsonElement property))
            {
                if (property.ValueKind == JsonValueKind.Number)
                {
                    return property.GetSingle();
                }
            }
            return defaultValue;
        }
        
        private bool GetBoolProperty(JsonElement element, string propertyName, bool defaultValue = false)
        {
            if (element.TryGetProperty(propertyName, out JsonElement property))
            {
                return property.GetBoolean();
            }
            return defaultValue;
        }
    }
}