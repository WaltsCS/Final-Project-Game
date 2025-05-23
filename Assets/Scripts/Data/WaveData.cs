using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Game/Level Wave")]
public class WaveData : ScriptableObject
{
    [Tooltip("All the enemy prefabs to spawn in this wave")]
    public List<GameObject> enemyPrefabs;

    [Tooltip("Total enemies to spawn this wave")]
    public int count = 10;

    [Tooltip("Time between spawns in seconds")]
    public float spawnInterval = 1f;
}

