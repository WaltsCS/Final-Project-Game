// Add this new manager to control batches
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WaveBatchManager : MonoBehaviour
{
    [System.Serializable]
    public class Batch
    {
        public List<WaveSpawner> spawners;
    }

    [Header("Wave Spawner Batches")]
    [Tooltip("Each batch spawns after the previous one is cleared")]
    [SerializeField] private List<Batch> waveBatches;

    [Header("Optional MiniBoss Spawner")] 
    [SerializeField] private WaveSpawner miniBossSpawner;

    [Header("Level Manager Reference")] 
    [SerializeField] private LevelManager levelManager;

    private void Start()
    {
        StartCoroutine(RunBatches());
    }

    private IEnumerator RunBatches()
    {
        foreach (var batch in waveBatches)
        {
            if (batch.spawners == null || batch.spawners.Count == 0)
                continue;

            foreach (var spawner in batch.spawners)
            {
                if (spawner != null)
                    spawner.StartSpawning();
            }

            yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);
        }

        if (miniBossSpawner != null)
        {
            miniBossSpawner.StartSpawning();
            yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("MiniBoss").Length == 0);
        }

        if (levelManager != null)
        {
            levelManager.SendMessage("DisplayLevelComplete");
        }
    }
}