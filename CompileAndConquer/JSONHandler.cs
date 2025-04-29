using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text.Json;
using System.Text.Json.Nodes;
using CompileAndConquer.Content;
using CompileAndConquer.Content.AttackPatterns;
using CompileAndConquer.Content.EnemyWaveManager;
using CompileAndConquer.Content.EnemyWaveManager.entryPatterns;
using CompileAndConquer.Content.EnemyWaveManager.exitPatterns;
using CompileAndConquer.Content.Interfaces;
using CompileAndConquer.Content.MovementTypes.BulletMovementTypes;
using CompileAndConquer.SpawnPatterns;

using CompileAndConquer.Entities.BulletSpawners;

public static class JSONHandler
{
    private static JsonNode gameplayEntities;
    private static JsonNode waves;
    private static JsonNode movementBehavior;

    static JSONHandler()
    {
        // Adjust paths as needed for your project structure
        gameplayEntities = LoadJson("GameConfigs\\GameplayEntities.json");
        waves = LoadJson("GameConfigs\\Waves.json");
        movementBehavior = LoadJson("GameConfigs\\MovementBehavior.json");
    }

    private static JsonNode LoadJson(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"JSON file not found: {path}");
        return JsonNode.Parse(File.ReadAllText(path));
    }

    // Gameplay
    public static int GetPlayerHealth()
    {
        return GetEntityInt("player", "lives");
    }

    public static int GetPlayerAttackDamage()
    {
        return GetEntityInt("player", "attack-damage");
    }

    public static float GetPlayerAttackSpeed()
    {
        return GetEntityFloat("player", "attack-speed");
    }

    public static float GetPlayerSpeed()
    {
        return GetEntityFloat("player", "speed");
    }

    public static float GetPlayerScale()
    {
        return GetEntityFloat("player", "scale");
    }

    public static float GetPlayerInvincibleDuration()
    {
        return GetEntityFloat("player", "invincible-duration");
    }

    public static int GetTypeAHealth() => GetEntityInt("TypeA", "health");
    public static int GetTypeAAttackSpeed() => GetEntityInt("TypeA", "attack-speed");
    public static int GetTypeASpeed() => GetEntityInt("TypeA", "speed");
    public static int GetTypeAScale() => GetEntityInt("TypeA", "scale");

    public static int GetTypeBHealth() => GetEntityInt("TypeB", "health");
    public static int GetTypeBAttackSpeed() => GetEntityInt("TypeB", "attack-speed");
    public static int GetTypeBSpeed() => GetEntityInt("TypeB", "speed");
    public static int GetTypeBScale() => GetEntityInt("TypeB", "scale");
    public static int GetMiniBossHealth() => GetEntityInt("MiniBoss", "health");
    public static int GetMiniBossAttackDamage() => GetEntityInt("MiniBoss", "attack-damage");
    public static int GetMiniBossAttackSpeed() => GetEntityInt("MiniBoss", "attack-speed");
    public static int GetFinalBossHealth() => GetEntityInt("FinalBoss", "health");

    private static int GetEntityInt(string entity, string key)
    {
        var arr = gameplayEntities?["Entities"]?[entity]?.AsArray();
        if (arr == null) throw new Exception($"Entity {entity} not found.");
        foreach (var obj in arr)
        {
            if (obj is JsonObject o && o.ContainsKey(key))
                return int.Parse(o[key]?.ToString() ?? "0");
        }
        throw new Exception($"Key {key} not found for entity {entity}.");
    }

    private static float GetEntityFloat(string entity, string key)
    {
        var arr = gameplayEntities?["Entities"]?[entity]?.AsArray();
        if (arr == null) throw new Exception($"Entity {entity} not found.");
        foreach (var obj in arr)
        {
            if (obj is JsonObject o && o.ContainsKey(key))
                return float.Parse(o[key]?.ToString() ?? "0");
        }
        throw new Exception($"Key {key} not found for entity {entity}.");
    }

    // Waves.json
    public static JsonArray GetWaves()
    {
        return waves?["Waves"]?.AsArray();
    }

    public static JsonArray GetWaveEvents(int waveIndex)
    {
        var wavesArr = GetWaves();
        if (wavesArr == null || waveIndex < 0 || waveIndex >= wavesArr.Count)
            throw new Exception("Invalid wave index.");
        return wavesArr[waveIndex]?["Events"]?.AsArray();
    }

    // Example: Get enemy type for a specific event in a wave
    public static string GetEnemyType(int waveIndex, int eventIndex, int enemyIndex)
    {
        var events = GetWaveEvents(waveIndex);
        if (events == null || eventIndex < 0 || eventIndex >= events.Count)
            throw new Exception("Invalid event index.");
        var enemyArr = events[eventIndex]?.AsArray();
        if (enemyArr == null || enemyIndex < 0 || enemyIndex >= enemyArr.Count)
            throw new Exception("Invalid enemy index.");
        return enemyArr[enemyIndex]?["EnemyType"]?.ToString();
    }

    // MovementBehavior
    public static JsonArray GetOnScreenMovementPatterns()
    {
        return movementBehavior?["MovementPatterns"]?["OnScreen"]?.AsArray();
    }

    public static JsonArray GetEntryTypes()
    {
        return movementBehavior?["MovementPatterns"]?["EntryTypes"]?.AsArray();
    }

    public static JsonArray GetExitTypes()
    {
        return movementBehavior?["MovementPatterns"]?["ExitTypes"]?.AsArray();
    }

    public static JsonObject GetAttackPattern(string patternName)
    {
        return movementBehavior?["AttackPatterns"]?[patternName]?.AsObject();
    }

    // Scoring
    public static int GetScoreForHitEnemy()
    {
        return gameplayEntities?["Scoring"]?["HitEnemy"]?.GetValue<int>() ?? 0;
    }
    public static IMovementStrategy ParseMovement(JsonNode node)
    { 
        if (node == null) return new StationaryMovement();
        string type = node["Type"]?.ToString();
        switch (type)
        { 
            case "Linear":
                bool startRight = node["StartRight"]?.GetValue<bool>() ?? false;
                return new LinearMovement(startRight);
            case "Curved":
                float hAmp = node["HorizontalAmplitude"]?.GetValue<float>() ?? 0f;
                float vAmp = node["VerticalAmplitude"]?.GetValue<float>() ?? 0f; 
                float hFreq = node["HorizontalFrequency"]?.GetValue<float>() ?? 1f; 
                float vFreq = node["VerticalFrequency"]?.GetValue<float>() ?? 1f; 
                return new CurvedMovement(hAmp, vAmp, hFreq, vFreq);
            case "Stationary":
                return new StationaryMovement();
            case "MiniBossMovement":
                return new miniBossMovement();
            default: 
                return new StationaryMovement();
        }
    }

    public static IAttackPattern ParseAttack(JsonNode node)
    {
        if (node == null) return new noAttack();
        string type = node["Type"]?.ToString();
        switch (type)
        {
            case "Triangle":
                int bulletCount = node["BulletCount"]?.GetValue<int>() ?? 8;
                float interval = node["Interval"]?.GetValue<float>() ?? 10f;
                float speed = node["Speed"]?.GetValue<float>() ?? 350f;
                return new TriangleAttack(bulletCount, interval, 2.0, dir => new StraightBullet(new Vector2(0, 1), speed));
    
            case "TimedStraight":
                float tInterval = node["Interval"]?.GetValue<float>() ?? 0.5f;
                float tSpeed = node["Speed"]?.GetValue<float>() ?? 250f;
                return new TimedStraightAttack(dir => new StraightBullet(dir, tSpeed), tInterval);
    
            case "Circle":
                int bulletCountRad = node["BulletCount"]?.GetValue<int>() ?? 24;
                float radius = node["Radius"]?.GetValue<float>() ?? 30f;
                float cSpeed = node["Speed"]?.GetValue<float>() ?? 200f;
                return new CircleAttack(bulletCountRad, radius, dir => new StraightBullet(dir, cSpeed));
    
            case "AlternatingAttack":
                float pauseBetweenAttacks = node["PauseBetweenAttacks"]?.GetValue<float>() ?? 5f;
                var subAttacksNode = node["Attacks"]?.AsArray();
                if (subAttacksNode == null || subAttacksNode.Count == 0)
                    return new noAttack();
    
                var subPatterns = new List<IAttackPattern>();
                foreach (var subNode in subAttacksNode)
                {
                    var pattern = ParseAttack(subNode);
                    if (pattern != null)
                        subPatterns.Add(pattern);
                }
                return new AlternatingAttackPattern(pauseBetweenAttacks, subPatterns.ToArray());
    
            case "Arc":
                float aSpeed = node["Speed"]?.GetValue<float>() ?? 200f;
                float aCooldown = node["Cooldown"]?.GetValue<float>() ?? 0.5f;
                return new ArcAttackPattern(dir => new StraightBullet(dir, aSpeed), 1f);
    
            case "ConcentricCircle":
                return new ConcentricCircleAttack();
    
            case "SpecialFeature":
                return new SpecialFeature();
    
            case "ThreeWaySpiralAttack":
                return new ThreeWaySpiralAttack();
    
            case "RepeatingSpiralAttack":
                return new RepeatingSpiralAttack();
    
            case "NoAttack":
            default:
                return new noAttack();
        }
    }


    public static IEntry ParseEntry(JsonNode node, Vector2 finalPos)
    {
        if (node == null) return new DirectionalEntry(CardinalDirection.Top, finalPos, 120f);
        string dirStr = node["Direction"]?.ToString();
        float speed = node["Speed"]?.GetValue<float>() ?? 120f;
        CardinalDirection dir = dirStr switch
        {
            "Top" => CardinalDirection.Top,
            "Bottom" => CardinalDirection.Bottom,
            "Left" => CardinalDirection.Left,
            "Right" => CardinalDirection.Right,
            _ => CardinalDirection.Top
        };
        return new DirectionalEntry(dir, finalPos, speed);
    }

    public static IExit ParseExit(JsonNode node, Vector2 finalPos)
    {
        if (node == null) return new DirectionalExit(CardinalDirection.Top, 120f);
        string dirStr = node["Direction"]?.ToString();
        float speed = node["Speed"]?.GetValue<float>() ?? 120f;
        CardinalDirection dir = dirStr switch
        { 
            "Top" => CardinalDirection.Top, 
            "Bottom" => CardinalDirection.Bottom,
            "Left" => CardinalDirection.Left,
            "Right" => CardinalDirection.Right,
            _ => CardinalDirection.Top
        };
        return new DirectionalExit(dir, speed);
    }

    public static ISpawnPattern ParseSpawnPattern(JsonNode node)
    {
        if (node == null) return new LineSpawnPattern(y: 200f);
        string type = node["Type"]?.ToString();
        switch (type)
        { 
            case "Line": 
                float y = node["Y"]?.GetValue<float>() ?? 200f; 
                return new LineSpawnPattern(y);
            default:
                return new LineSpawnPattern(y: 200f);
        }
    }
    

    
    public static List<BossPhase> ParseBossPhases()
    {
        var wavesArr = waves?["Waves"]?.AsArray();
        if (wavesArr == null) return new List<BossPhase>();

        foreach (var waveNode in wavesArr)
        {
            var eventsArr = waveNode?["Events"]?.AsArray();
            if (eventsArr == null) continue;

            foreach (var stepArr in eventsArr)
            {
                foreach (var eventNode in stepArr.AsArray())
                {
                    string enemyType = eventNode?["EnemyType"]?.ToString();
                    if (enemyType == "FinalBoss")
                    {
                        var bossPhasesNode = eventNode["BossPhases"]?.AsArray();
                        if (bossPhasesNode == null) continue;

                        var phases = new List<BossPhase>();
                        foreach (var phaseNode in bossPhasesNode)
                        {
                            var movement = ParseMovement(phaseNode["Movement"]);
                            var attack = ParseAttack(phaseNode["Attack"]);
                            int hpThreshold = phaseNode["HpThreshold"]?.GetValue<int>() ?? 1000;

                            phases.Add(new BossPhase
                            {
                                Movement = movement,
                                Attack = attack,
                                HpThreshold = hpThreshold
                            });
                        }
                        return phases;
                    }
                }
            }
        }
        return new List<BossPhase>();
    }
}
