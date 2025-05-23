using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Tooltip("List your LevelData assets here (in build order)")]
    public List<LevelData> levels;

    private List<WaveSpawner> spawners;
    private int currentLevelIndex = 0;

    private void Awake()
    {
        ///Find all and any spawners exist/enabled  in the scene
        spawners = new List<WaveSpawner>(WaveSpawner.Instances);

        if (spawners.Count == 0)
            Debug.LogWarning("No WaveSpawner components found in scene!");
    }

    private void Start()
    {
        if (levels == null || levels.Count == 0)
        {
            Debug.LogError("No LevelData assigned to LevelManager!");
            return;
        }

        StartCoroutine(RunLevel(levels[currentLevelIndex]));
    }

    private IEnumerator RunLevel(LevelData level)
    {
        ///1) Regular waves
        foreach (var wave in level.waves)
        {
            ///Broadcast this wave to every spawner
            foreach (var sp in spawners)
                sp.StartCoroutine(sp.SpawnWaveAtPoint(wave));

            ///Wait until all enemies spawned by all spawners are gone
            yield return StartCoroutine(WaitForEnemiesCleared());
        }

        ///2) Mini-boss (every N levels)
        if ((currentLevelIndex + 1) % level.miniBossEveryXLevels == 0
            && level.miniBossWave != null)
        {
            foreach (var sp in spawners)
                sp.StartCoroutine(sp.SpawnWaveAtPoint(level.miniBossWave));
            yield return StartCoroutine(WaitForEnemiesCleared());
        }

        ///3) Final boss on the last LevelData
        if (currentLevelIndex == levels.Count - 1 && level.finalBossWave != null)
        {
            foreach (var sp in spawners)
                sp.StartCoroutine(sp.SpawnWaveAtPoint(level.finalBossWave));
            yield return StartCoroutine(WaitForEnemiesCleared());

            OnGameComplete();
            yield break;
        }

        ///4) Advance to next level
        currentLevelIndex++;
        StartCoroutine(RunLevel(levels[currentLevelIndex]));
    }

    private IEnumerator WaitForEnemiesCleared()
    {
        ///Poll until no objects tagged "Enemy" remain
        while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
            yield return null;
    }

    private void OnGameComplete()
    {
        Debug.Log("All levels cleared! Showing end‐game screen...");
        ///TODO: trigger your victory UI, credits, etc.
    }
}
