# Compile And Conquer - Game Configuration System

This directory contains JSON configuration files that control various aspects of the game. The most significant is the wave system that allows defining enemy waves, patterns, and behaviors without changing code.

## Wave System

The wave system uses JSON to define sequences of enemies, their behaviors, and spawn patterns. This approach allows for easy customization of game difficulty and level design.

### File Structure

- `GameConfigs/Waves.json` - Defines all waves, events, and enemy spawn patterns
- `GameConfigs/GameplayEntities.json` - Defines entity properties like health, damage, etc.
- `GameConfigs/MovementBehavior.json` - Defines movement patterns and behaviors

## Wave JSON Format

The wave system JSON has this general structure (grunt event):

```
{
  "Waves": [
    {
      "Events": [
      {
        "EnemyType": "TypeA",
        "SpawnCount": #,
        "Duration": #,
        "SpawnPattern": { 
        "Type": "",
        "Y": "###"
        ...
        },
        "Entry": {
        "Direction": ""
        "Speed": "###"
        ... 
        },
        "Exit": { 
        "Direction": "",
        "Speed": "###",
        ... 
        },
        "Movement": { 
        "Type": ""
        ... 
        },
        "Attack": { 
        "Type": ""
        ... 
        },
    }
  ]
}
```
And for bosses (example):
```json
{
  "EnemyType": "FinalBoss",
  "SpawnCount": 1,
  "Duration": 90,
  "SpawnPattern": { "Type": "Line", "Y": 100 },
  "Entry": { "Direction": "Top", "Speed": 120 },
  "Exit": { "Direction": "Top", "Speed": 120 },
  "Movement": { "Type": "MiniBossMovement" },
  "Attack": {
    "Type": "AlternatingAttack",
    "PauseBetweenAttacks": 3,
    "Attacks": [
      { "Type": "Arc", "Cooldown": 8, "Speed": 200 },
      { "Type": "ConcentricCircle" }
    ]
  },
  "BossPhases": [
    {
      "Movement": { "Type": "FinalBossMovement" },
      "Attack": {
        "Type": "AlternatingAttack",
        "PauseBetweenAttacks": 5,
        "Attacks": [
          { "Type": "Arc", "Speed": 200, "Cooldown": 1 },
          { "Type": "ConcentricCircle" }
        ]
      },
      "HpThreshold": 2000
    },
    {
      "Movement": { "Type": "MiniBossMovement" },
      "Attack": {
        "Type": "AlternatingAttack",
        "PauseBetweenAttacks": 3,
        "Attacks": [
          { "Type": "RepeatingSpiralAttack" },
          { "Type": "Arc", "Speed": 200, "Cooldown": 1 }
        ]
      },
      "HpThreshold": 800
    }
  ]
}
```

### Valid Enemy Types
- "TypeA" - Basic enemy
- "TypeB" - Secondary enemy type
- "MiniBoss" - Mid-level boss
- "FinalBoss" - End-game boss

### Valid TypeA/TypeB Movement Types
- "Linear" - Straight line movement
- "Stationary" - Fixed position
- "Curved" - Curved/wave motion (Requires float properties: HorizontalAmplitude, VerticalAmplitude, HorizontalFrequency, VerticalFrequency)
### Valid mini/final Boss Movement Types
- "MiniBossMovement" - Special miniboss movement
- "FinalBoss" - Special final boss movement

### Valid Entry/Exit Directions
- "Top"
- "Bottom"
- "Left"
- "Right"

### Valid Attack Types
- "NoAttack" - No attacks (Also default)
- "TimedStraight" - Basic timed straight shots (Requires float properties: Interval, Speed)
- "Triangle" - Triangle-pattern bullets (Requires float properties: BulletCount, Interval, Speed)
- "Circle" - Circular bullet pattern (Requires float properties: BulletCount, Radius, Speed)
### Valid Boss Attacks
- "AlternatingAttack" - (Requires float PauseBetweenAttacks and "Attacks" array of attack types to alternate)
- "RepeatingSpiralAttack"
- "Arc" - Arc attack (Requires float properties: Speed, Cooldown)
- "ConcentricCircle" - Concentric circle attack

### Valid SpawnPatterns
- "Line" - Horizontal line formation
- "Pyramid" - Pyramid formation

### Key Components

1. **Waves**: An array of wave definitions
   - **Events**: A 2D array representing the individual events within the wave

2. **Event Properties**:
   - **EnemyType**: Type of enemy to spawn (TypeA, TypeB, MiniBoss, FinalBoss)
   - **SpawnCount**: Number of enemies to spawn in this event
   - **Duration**: How long the event lasts before enemies exit (seconds)

3. **SpawnPattern**: Controls how enemies are positioned
   - **Type**: "Line" or "Pyramid"
   - **Y**: Base Y-coordinate
   - For Pyramid patterns:
     - **Rows**: Number of rows in the pyramid
     - **Spacing**: Spacing between enemies

4. **Entry/Exit**: Controls how enemies enter and leave the screen
   - **Direction**: "Top", "Right", "Bottom", "Left"
   - **Speed**: Movement speed

5. **Movement**: Controls enemy movement behavior
   - **Type**: "Linear", "Stationary", "Curved", "MiniBoss", "FinalBoss"
   - Type-specific parameters like "StartRight" for Linear movement

6. **Attack**: Defines enemy attack patterns
   - **Type**: "None", "TimedStraight", "Triangle", "Circle"
   - Type-specific parameters like "BulletCount", "Interval", "Speed"
