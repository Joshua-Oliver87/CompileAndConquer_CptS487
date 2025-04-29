using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;

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
    public static int GetTypeBHealth() => GetEntityInt("TypeB", "health");
    public static int GetMiniBossHealth() => GetEntityInt("MiniBoss", "health");
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
}
