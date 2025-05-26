using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Tooltip("Define each wave (enemy prefabs, count, interval) in order")]
    [SerializeField] private List<WaveData> waves;

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
            ///1) Spawn this wave
            yield return StartCoroutine(SpawnWave(wave));

            ///2) Wait until the player has destroyed every spawned enemy
            yield return new WaitUntil(() =>
                GameObject.FindGameObjectsWithTag("Enemy").Length == 0
            );
        }

        Debug.Log("[WaveSpawner] All waves complete on this spawner.");
    }

    private IEnumerator SpawnWave(WaveData wave)
    {
        for (int i = 0; i < wave.count; i++)
        {
            ///pick a random prefab and spawn
            var prefab = wave.enemyPrefabs[Random.Range(0, wave.enemyPrefabs.Count)];
            Instantiate(prefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(wave.spawnInterval);
        }
    }
}
