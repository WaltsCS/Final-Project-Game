using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Game/Level Definition")]
public class LevelData : ScriptableObject
{
    [Tooltip("Regular waves before mini-boss")]
    public List<WaveData> waves;

    [Tooltip("Optional mini-boss wave (set to null if none)")]
    public WaveData miniBossWave;

    [Tooltip("Final boss wave")]
    public WaveData finalBossWave;

    [Tooltip("Spawn a mini-boss every X levels")]
    public int miniBossEveryXLevels = 3;
}

