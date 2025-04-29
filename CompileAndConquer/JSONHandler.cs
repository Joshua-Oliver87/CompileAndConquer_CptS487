using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public static class JSONHandler
{
    private static JsonDocument jsonDoc;
    private static JsonElement Root => jsonDoc.RootElement;

    static JSONHandler()
    {
        string jsonPath = "Gameplay.json";
        if (!File.Exists(jsonPath))
            throw new FileNotFoundException("JSON file not found.");

        string jsonContent = File.ReadAllText(jsonPath);
        jsonDoc = JsonDocument.Parse(jsonContent);
    }

    private static string GetEntityValue(string entity, string key)
    {
        if (!Root.TryGetProperty("Entities", out JsonElement entities)) return null;
        if (!entities.TryGetProperty(entity, out JsonElement array)) return null;

        foreach (JsonElement item in array.EnumerateArray())
        {
            foreach (JsonProperty prop in item.EnumerateObject())
                if (prop.Name == key)
                    return prop.Value.ToString();
        }

        return null;
    }

    // Entity Information

    public static int GetPlayerHealth() => int.Parse(GetEntityValue("player", "health") ?? "0");
    public static int GetPlayerMaxHealth() => int.Parse(GetEntityValue("player", "max-health") ?? "0");
    public static int GetPlayerAttack() => int.Parse(GetEntityValue("player", "attack") ?? "0");
    public static int GetPlayerLives() => int.Parse(GetEntityValue("player", "lives") ?? "0");
    public static int GetPlayerBombs() => int.Parse(GetEntityValue("player", "bombs") ?? "0");
    public static string GetPlayerTexture() => GetEntityValue("player", "texture");

    public static int GetTypeAHealth() => int.Parse(GetEntityValue("TypeA", "health") ?? "0");
    public static string GetTypeABehavior() => GetEntityValue("TypeA", "Behavior");

    public static int GetTypeBHealth() => int.Parse(GetEntityValue("TypeB", "health") ?? "0");
    public static string GetTypeBBehavior() => GetEntityValue("TypeB", "Behavior");

    public static int GetMiniBossHealth() => int.Parse(GetEntityValue("MiniBoss", "health") ?? "0");
    public static string GetMiniBossBehavior() => GetEntityValue("MiniBoss", "Behavior");

    public static int GetFinalBossHealth() => int.Parse(GetEntityValue("FinalBoss", "health") ?? "0");
    public static string GetFinalBossBehavior() => GetEntityValue("FinalBoss", "Behavior");

    public static int GetPlayerBulletDamage() => int.Parse(GetEntityValue("PlayerBullet", "damage") ?? "0");
    public static int GetPlayerBulletSpeed() => int.Parse(GetEntityValue("PlayerBullet", "speed") ?? "0");

    public static int GetBasicBulletDamage() => int.Parse(GetEntityValue("BasicBullet", "damage") ?? "0");
    public static int GetBasicBulletSpeed() => int.Parse(GetEntityValue("BasicBullet", "speed") ?? "0");

    public static string GetPowerUpTexture() => GetEntityValue("PowerUp", "texture");

    // Movement Patterns
    

    public static List<string> GetEntryPatterns()
    {
        var entries = Root.GetProperty("MovementPatterns")[1].GetProperty("Entry");
        var list = new List<string>();
        foreach (var item in entries.EnumerateArray())
            list.Add(item.GetString());
        return list;
    }

    public static List<string> GetExitPatterns()
    {
        var exits = Root.GetProperty("MovementPatterns")[2].GetProperty("Exit");
        var list = new List<string>();
        foreach (var item in exits.EnumerateArray())
            list.Add(item.GetString());
        return list;
    }
}