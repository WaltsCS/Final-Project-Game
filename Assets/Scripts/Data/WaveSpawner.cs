// WaveSpawner.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Tooltip("Define each wave (enemy prefabs, count, interval) in order")]
    [SerializeField] private List<WaveData> waves;
    [Tooltip("Miniboss wave to spawn after all regular waves")]
    [SerializeField] private WaveData minibossWave;

    public bool IsDoneSpawning { get; private set; } = false;

    private void Start()
    {
        if (waves == null || waves.Count == 0)
        {
            Debug.LogWarning("[WaveSpawner] No waves assigned!");
            return;
        }
        StartCoroutine(RunAllWaves());
    }

    private IEnumerator RunAllWaves()
    {
        foreach (var wave in waves)
        {
            yield return StartCoroutine(SpawnWave(wave));
            yield return new WaitUntil(() =>
                GameObject.FindGameObjectsWithTag("Enemy").Length == 0
            );
        }

        // Spawn miniboss after regular waves
        if (minibossWave != null)
        {
            yield return StartCoroutine(SpawnWave(minibossWave));
            yield return new WaitUntil(() =>
                GameObject.FindGameObjectsWithTag("Enemy").Length == 0
            );
        }

        Debug.Log("[WaveSpawner] All waves and miniboss complete on this spawner.");
        IsDoneSpawning = true;
    }

    private IEnumerator SpawnWave(WaveData wave)
    {
        for (int i = 0; i < wave.count; i++)
        {
            var prefab = wave.enemyPrefabs[Random.Range(0, wave.enemyPrefabs.Count)];
            Instantiate(prefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(wave.spawnInterval);
        }
    }
}