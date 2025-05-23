using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour        ///For each spawnPoint empty gameObject
{
    [Tooltip("Point this spawner at the same LevelData list your LevelManager uses.")]
    public List<WaveData> waves;
    private int waveIndex = 0;      ///purely internal variable, Logs say "unused" to keep absolute control of spawners

    public static readonly List<WaveSpawner> Instances = new List<WaveSpawner>();   ///have each spawner announce itself:
    private void OnEnable()
    {
        Instances.Add(this);
    }
    private void OnDisable()
    {
        Instances.Remove(this);
    }

    public IEnumerator SpawnWaveAtPoint(WaveData wave)
    {
        yield return new WaitForSeconds(Random.Range(0f, 1f));  /// optional delay for next wave spawn
        for (int i = 0; i < wave.count; i++)
        {
            var prefab = wave.enemyPrefabs[Random.Range(0, wave.enemyPrefabs.Count)];
            Instantiate(prefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(wave.spawnInterval);
        }
    }
}
