using UnityEngine;
using System.Collections.Generic;

public class GateManager : MonoBehaviour
{
    public static event System.Action<string> OnAnyGateOpened;

    [Tooltip("Tag of the objects the player must destroy (keys)")]
    [SerializeField] private string keyTag = "GateKey";

    [Tooltip("How many keys must be destroyed before opening the gate")]
    [SerializeField] private int requiredCount = 3;

    [Tooltip("These wall GameObjects will be disabled when unlocked")]
    [SerializeField] private List<GameObject> gateWalls;

    private int currentCount = 0;

    // Static registry of managers by keyTag
    private static Dictionary<string, List<GateManager>> registry = new Dictionary<string, List<GateManager>>();

    private void Awake()
    {
        // Register this manager under its keyTag
        if (!registry.TryGetValue(keyTag, out var list))
        {
            list = new List<GateManager>();
            registry[keyTag] = list;
        }
        list.Add(this);
    }

    private void OnDestroy()
    {
        // Unregister
        if (registry.TryGetValue(keyTag, out var list))
        {
            list.Remove(this);
            if (list.Count == 0)
                registry.Remove(keyTag);
        }
    }

    // Called by GateKey to notify any managers for a given tag
    public static void NotifyKeyDestroyedForTag(string tag)
    {
        if (!registry.TryGetValue(tag, out var list))
            return;

        foreach (var gm in list)
        {
            gm.currentCount++;
            Debug.Log($"[GateManager] {gm.currentCount}/{gm.requiredCount} keys destroyed for tag '{gm.keyTag}'");

            if (gm.currentCount >= gm.requiredCount)
            {
                gm.OpenGate();
            }
        }
    }

    private void OpenGate()
    {
        Debug.Log($"[GateManager] Opening gate for tag '{keyTag}'");
        foreach (var wall in gateWalls)
            wall.SetActive(false);

        OnAnyGateOpened?.Invoke(keyTag);
        // Optional: play sound or VFX here
    }
}