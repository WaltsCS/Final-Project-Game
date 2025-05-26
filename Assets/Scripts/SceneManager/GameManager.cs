using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Text Display")]
    [SerializeField] private TextMeshProUGUI gameOverText;

    [Header("Buttons")]
    [SerializeField] private GameObject restartButton;

    [Header("Managers")]
    // todo: Insert managers here (time, audio, etc.)

    private bool isGameActive = false;

    public bool IsGameActive
    {
        get { return isGameActive; }
    }

    public void DisplayGameOver()
    {
        isGameActive = false;
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        // Restart to the first scene
        SceneManager.LoadScene(0);
    }
}
