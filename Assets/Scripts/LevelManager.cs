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

    // todo: set to false in the future
    // level starts when player clicks play in game manager
    private bool isLevelActive = true;

    public bool IsLevelActive
    {
        get { return isLevelActive; }
    }


    void Start()
    {
        StartCoroutine(CheckForLevelClear());
    }

    private IEnumerator CheckForLevelClear()
    {
        // Allocate a short delay to load the scene
        // Fix for race condition where enemies spawn before the level is active
        yield return new WaitForSeconds(0.5f);

        // Wait until there are no more enemies in the scene
        while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
            yield return null;

        DisplayLevelComplete();
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
        // Restart to the first scene
        SceneManager.LoadScene(0);
    }
}
