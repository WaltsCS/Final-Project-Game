using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Wave Configuration")]
    [Tooltip("Regular waves for this scene")]
    public List<WaveData> waves;

    [Tooltip("Mini-boss wave (set to None if unused)")]
    public WaveData miniBossWave;

    [Tooltip("Final boss wave (set to None if unused)")]
    public WaveData finalBossWave;

    [Tooltip("Spawn a mini-boss every N levels (scene indices start at 0)")]
    public int miniBossEveryXLevels = 3;

    [Header("UI Hooks")]
    [Tooltip("Button or UI panel to show when level is complete (Next Level)")]
    public GameObject levelCompleteUI;

    private List<WaveSpawner> spawners;

    private void Awake()
    {
        // Hide complete UI initially
        if (levelCompleteUI != null)
            levelCompleteUI.SetActive(false);
    }

    private void Start()
    {
        // Cache all active spawners that self-registered
        spawners = new List<WaveSpawner>(WaveSpawner.Instances);
        if (spawners.Count == 0)
            Debug.LogWarning("[LevelManager] No WaveSpawner instances found in scene.");

        if (waves == null || waves.Count == 0)
        {
            Debug.LogError("[LevelManager] No regular waves assigned!");
            return;
        }

        StartCoroutine(RunWavesRoutine());
    }

    private IEnumerator RunWavesRoutine()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        int buildCount = SceneManager.sceneCountInBuildSettings;

        ///1) Regular waves
        foreach (var wave in waves)
        {
            BroadcastWave(wave);
            yield return StartCoroutine(WaitForEnemiesCleared());
        }

        ///2) Mini-boss if this is an Nth level
        if (miniBossWave != null && (sceneIndex + 1) % miniBossEveryXLevels == 0)
        {
            BroadcastWave(miniBossWave);
            yield return StartCoroutine(WaitForEnemiesCleared());
        }

        ///3) Final boss if this is the last build scene
        if (finalBossWave != null && sceneIndex == buildCount - 1)
        {
            BroadcastWave(finalBossWave);
            yield return StartCoroutine(WaitForEnemiesCleared());
        }

        ///4) Level complete – show UI for Next Level or Restart
        OnLevelComplete();
    }

    private void BroadcastWave(WaveData wave)
    {
        foreach (var sp in spawners)
        {
            sp.StartCoroutine(sp.SpawnWaveAtPoint(wave));
        }
    }

    private IEnumerator WaitForEnemiesCleared()
    {
        ///Poll until no GameObjects tagged "Enemy" remain
        while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
            yield return null;
    }

    private void OnLevelComplete()
    {
        Debug.Log("[LevelManager] Level complete. Waiting for player to proceed.");
        if (levelCompleteUI != null)
            levelCompleteUI.SetActive(true);
    }
}