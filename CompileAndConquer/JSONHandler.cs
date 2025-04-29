using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using CompileAndConquer.Content.EnemyWaveManager;

namespace CompileAndConquer.Content
{

    public static class JSONHandler
    {
        private static JsonDocument gameplayDoc;
        private static JsonDocument wavesDoc;
        private static JsonElement GameplayRoot => gameplayDoc.RootElement;
        private static JsonElement WavesRoot => wavesDoc.RootElement;

        static JSONHandler()
        {
            LoadGameplayJson();
            LoadWavesJson();
        }

        private static void LoadGameplayJson()
        {
            string jsonPath = "Gameplay.json";
            if (!File.Exists(jsonPath))
                throw new FileNotFoundException("Gameplay JSON file not found.");

            string jsonContent = File.ReadAllText(jsonPath);
            gameplayDoc = JsonDocument.Parse(jsonContent);
        }

        private static void LoadWavesJson()
        {
            string jsonPath = "GameConfigs/Waves.json";
            Console.WriteLine($"Attempting to load Waves.json from: {Path.GetFullPath(jsonPath)}");
            
            if (!File.Exists(jsonPath))
            {
                Console.WriteLine($"File not found at {jsonPath}. Checking for alternative paths...");
                
                // Try alternate paths
                string[] alternativePaths = {
                    "./GameConfigs/Waves.json",
                    "../GameConfigs/Waves.json",
                    "Waves.json",
                    "/mnt/c/OneDrive/School/WSU Spring 2025/CPTS 487/clone-04-28/CompileAndConquer_CptS487/CompileAndConquer/GameConfigs/Waves.json"
                };
                
                foreach (string altPath in alternativePaths)
                {
                    Console.WriteLine($"Checking {Path.GetFullPath(altPath)}");
                    if (File.Exists(altPath))
                    {
                        jsonPath = altPath;
                        Console.WriteLine($"Found file at {jsonPath}");
                        break;
                    }
                }
                
                if (!File.Exists(jsonPath))
                    throw new FileNotFoundException("Waves JSON file not found.");
            }

            string jsonContent = File.ReadAllText(jsonPath);
            Console.WriteLine($"Loaded JSON content with {jsonContent.Length} characters");
            wavesDoc = JsonDocument.Parse(jsonContent);
            Console.WriteLine("Successfully parsed Waves.json");
        }

        private static string GetEntityValue(string entity, string key)
        {
            if (!GameplayRoot.TryGetProperty("Entities", out JsonElement entities)) return null;
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
            var entries = GameplayRoot.GetProperty("MovementPatterns")[1].GetProperty("Entry");
            var list = new List<string>();
            foreach (var item in entries.EnumerateArray())
                list.Add(item.GetString());
            return list;
        }

        public static List<string> GetExitPatterns()
        {
            var exits = GameplayRoot.GetProperty("MovementPatterns")[2].GetProperty("Exit");
            var list = new List<string>();
            foreach (var item in exits.EnumerateArray())
                list.Add(item.GetString());
            return list;
        }

        // Wave Loading Methods

        public static Queue<WaveManager.Wave> LoadWavesFromJson()
        {
            Queue<WaveManager.Wave> waveQueue = new Queue<WaveManager.Wave>();
            WaveBuilder waveBuilder = new WaveBuilder();

            Console.WriteLine("Loading waves from JSON...");
            try 
            {
                if (WavesRoot.TryGetProperty("Waves", out JsonElement wavesArray))
                {
                    Console.WriteLine($"Found Waves array with {wavesArray.GetArrayLength()} elements");
                    foreach (JsonElement waveJson in wavesArray.EnumerateArray())
                    {
                        if (waveJson.TryGetProperty("WaveID", out JsonElement idElement))
                        {
                            Console.WriteLine($"Processing wave: {idElement.GetString()}");
                        }
                        
                        if (waveJson.TryGetProperty("Events", out JsonElement eventsArray))
                        {
                            Console.WriteLine($"  Found Events array with {eventsArray.GetArrayLength()} step elements");
                        }
                        else
                        {
                            Console.WriteLine("  WARNING: No Events property found in wave!");
                        }
                        
                        var wave = waveBuilder.Build(waveJson);
                        if (wave != null && wave.Steps.Count > 0)
                        {
                            Console.WriteLine($"  Built wave with {wave.Steps.Count} steps");
                            waveQueue.Enqueue(wave);
                        }
                        else
                        {
                            Console.WriteLine("  WARNING: Wave was null or had 0 steps!");
                        }
                    }
                }
                else 
                {
                    Console.WriteLine("WARNING: No 'Waves' array found in JSON!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR in LoadWavesFromJson: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }

            return waveQueue;
        }

    }
}