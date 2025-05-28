// LevelManager.cs
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

    private bool isLevelActive = true;
    public bool IsLevelActive => isLevelActive;

    private WaveSpawner waveSpawner;

    void Start()
    {
        waveSpawner = FindObjectOfType<WaveSpawner>();
        StartCoroutine(CheckForLevelClear());
    }

    private IEnumerator CheckForLevelClear()
    {
        yield return new WaitForSeconds(0.5f);

        while (true)
        {
            // Wait until all enemies are gone AND the wave spawner is done
            if (waveSpawner != null && waveSpawner.IsDoneSpawning)
            {
                if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
                    break;
            }
            yield return null;
        }

        if (isLevelActive)
        {
            DisplayLevelComplete();
        }
    }

    private void DisplayLevelComplete()
    {
        isLevelActive = false;
        levelCompleteText.gameObject.SetActive(true);
        nextLevelButton.gameObject.SetActive(true);
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
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
