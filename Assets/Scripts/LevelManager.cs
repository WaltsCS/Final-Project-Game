using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Overlays;

public class LevelManager : MonoBehaviour
{
    [Tooltip("All levels in build order")]
    public List<LevelData> levels;
    private int currentLevelIndex = 0;

    private void Start()
    {
        StartCoroutine(RunLevel(levels[currentLevelIndex]));
    }

    private IEnumerator RunLevel(LevelData level)
    {
        //1. Run regular waves
        foreach (var wave in level.waves)
            yield return StartCoroutine(SpawnWave(wave));

        //2.  Mini-boss (if defined)
        if ((currentLevelIndex + 1) % level.miniBossEveryXLevels == 0
            && level.miniBossWave != null)
            yield return StartCoroutine(SpawnWave(level.miniBossWave));

        //3. Final boss on the very last LevelData
        if (currentLevelIndex == levels.Count - 1)
            yield return StartCoroutine(SpawnWave(level.finalBossWave));

        //4. Level completion: advance or end game
        currentLevelIndex++;
        if (currentLevelIndex < levels.Count)
            StartCoroutine(RunLevel(levels[currentLevelIndex]));
        else
            OnGameComplete();
    }

    private IEnumerator SpawnWave(WaveData wave)
    {
        for (int i = 0; i < wave.count; i++)
        {
            //pick a random prefab, spawn at spawn‐point logic
            var prefab = wave.enemyPrefabs[Random.Range(0, wave.enemyPrefabs.Count)];
            Instantiate(prefab, GetSpawnPosition(), Quaternion.identity);
            yield return new WaitForSeconds(wave.spawnInterval);
        }

        //wait until all enemies die
        while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
            yield return null;
    }

    private Vector3 GetSpawnPosition()
    {
        //your logic: random edge, fixed points, etc.
        return Vector3.zero;
    }

    private void OnGameComplete()
    {
        Debug.Log("All Levels Cleared! Show credits, etc.");
    }
}
