using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    [SerializeField] private Button button;

    void Start()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
            if (button == null)
            {
                return;
            }
        }
        button.onClick.AddListener(LoadStartScene);
    }

    public void LoadStartScene()
    {
        SceneManager.LoadScene(0);
    }
}