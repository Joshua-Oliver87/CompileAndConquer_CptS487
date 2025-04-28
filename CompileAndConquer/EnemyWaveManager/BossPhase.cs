using System;
using System.Data;
using CompileAndConquer.Content;
using Microsoft.Xna.Framework;
using CompileAndConquer.Content.Interfaces;

public class BossPhase
{
    public IMovementStrategy Movement { get; set; }
    public IAttackPattern Attack { get; set; }
    public int? HpThreshold { get; set; } // HP to switch to next phase (optional)
}