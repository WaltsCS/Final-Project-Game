using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance { get; private set; }

    [System.Serializable]
    public class Pool {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        // Pre-warm each pool
        foreach (var pool in pools) {
            var queue = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++) {
                var obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                queue.Enqueue(obj);
            }
            poolDictionary[pool.tag] = queue;
        }
    }

    /// <summary>
    /// Rent an object by tag, setting its position/rotation.
    /// </summary>
    public GameObject GetFromPool(string tag, Vector3 position, Quaternion rotation) {
        if (!poolDictionary.ContainsKey(tag)) {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist!");
            return null;
        }

        var objectToSpawn = poolDictionary[tag].Count > 0
            ? poolDictionary[tag].Dequeue()
            : Instantiate(pools.Find(p => p.tag == tag).prefab);

        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);

        // If your pooled object has an IPooledObject interface, call OnObjectSpawn:
        var pooled = objectToSpawn.GetComponent<IPooledObject>();
        pooled?.OnObjectSpawn();

        return objectToSpawn;
    }

    /// <summary>
    /// Return a GameObject to its pool.
    /// </summary>
    public void ReturnToPool(string tag, GameObject obj) {
        obj.SetActive(false);
        if (poolDictionary.ContainsKey(tag)) {
            poolDictionary[tag].Enqueue(obj);
        } else {
            Destroy(obj);
        }
    }
}

/// <summary>
/// Optional interface for pooled objects to reset themselves when spawned.
/// </summary>
public interface IPooledObject {
    void OnObjectSpawn();
}
