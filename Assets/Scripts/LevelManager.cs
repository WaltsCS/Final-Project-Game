using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Text Display")]
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI levelCompleteText;

    [Header("Buttons")]
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject nextLevelButton;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip victorySoundFX;

    private AudioSource audioSource;

    private bool isLevelActive = true;

    public bool IsLevelActive
    {
        get { return isLevelActive; }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(CheckForLevelClear());
    }

    private IEnumerator CheckForLevelClear()
    {
        // Allocate a short delay to load the scene
        // Fix for race condition where enemies spawn before the level is active
        yield return new WaitForSeconds(0.5f);

        // Wait until there are no more enemies in the scene
        while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0
        || GameObject.FindGameObjectsWithTag("FinalBoss").Length > 0
        || GameObject.FindGameObjectsWithTag("MiniBoss").Length > 0)
            yield return null;

        if (isLevelActive)
        {
            DisplayLevelComplete();
        }
    }

    public void DisplayLevelComplete()
    {
        isLevelActive = false;
        
        // Only activate levelCompleteText if it’s been assigned in the Inspector
        if (levelCompleteText != null)
            levelCompleteText.gameObject.SetActive(true);
        // Only activate nextLevelButton if it’s been assigned
        if (nextLevelButton != null)
            nextLevelButton.SetActive(true);

        PlayVictorySound();
    }

    private void PlayVictorySound()
    {
        audioSource.PlayOneShot(victorySoundFX);
    }

    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }

    }

    public void DisplayGameOver()
    {
        isLevelActive = false;
        if (gameOverText != null)
            gameOverText.gameObject.SetActive(true);

        if (restartButton != null)
            restartButton.SetActive(true);
    }

    public void RestartGame()
    {
        // Restart to the first scene
        SceneManager.LoadScene(0);
    }
}
