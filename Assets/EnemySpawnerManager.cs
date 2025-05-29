using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Manages sequential batches of WaveSpawners, followed by the mini-boss and final boss phases.
public class WaveBatchManager : MonoBehaviour
{
    [System.Serializable]
    public class Batch
    {
        [Tooltip("WaveSpawners that fire together in this batch")]        
        public List<WaveSpawner> spawners;
    }

    [Header("Wave Spawner Batches")]
    [Tooltip("Each batch spawns after the previous one has cleared all its enemies.")]
    [SerializeField] private List<Batch> waveBatches;

    [Header("Optional MiniBoss Spawner")] 
    [Tooltip("Spawn the mini-boss after all batches complete.")]
    [SerializeField] private WaveSpawner miniBossSpawner;

    [Header("Level Manager Reference")] 
    [SerializeField] private LevelManager levelManager;

    private void Start()
    {
        StartCoroutine(RunBatches());
    }

    private IEnumerator RunBatches()
    {
        // 1) Run each wave batch in order
        foreach (var batch in waveBatches)
        {
            if (batch.spawners == null || batch.spawners.Count == 0)
                continue;

            // Trigger all spawners in this batch
            foreach (var sp in batch.spawners)
                sp?.StartSpawning();

            // Wait until no "Enemy"-tagged objects remain
            yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);
        }

        // 2) Mini-boss phase
        if (miniBossSpawner != null)
        {
            miniBossSpawner.StartSpawning();
            // Wait until the mini-boss (tagged "MiniBoss") is gone
            yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("MiniBoss").Length == 0);
        }

        // 3) Final-boss phase
        // If a FinalBoss exists in the scene, wait until it's destroyed
        if (GameObject.FindGameObjectsWithTag("FinalBoss").Length > 0)
        {
            yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("FinalBoss").Length == 0);
        }

        // 4) All phases complete → show Level Complete UI
        if (levelManager != null)
            levelManager.DisplayLevelComplete();
        else
            Debug.LogWarning("WaveBatchManager: LevelManager reference missing.");
    }
}
