using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("UI Hooks")]
    [Tooltip("Button or UI panel to show when level is complete (Next Level)")]
    public GameObject levelCompleteUI;

    void Awake()
    {
        if (levelCompleteUI != null)
            levelCompleteUI.SetActive(false);
    }

    void Start()
    {
        StartCoroutine(CheckForLevelClear());
    }

    private IEnumerator CheckForLevelClear()
    {
        // Wait until there are no more enemies in the scene
        while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
            yield return null;

        // Once clear, show your next-level UI
        if (levelCompleteUI != null)
            levelCompleteUI.SetActive(true);
    }
}
