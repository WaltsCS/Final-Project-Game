using UnityEngine;
using UnityEngine.SceneManagement;
public class BackToTitleScreen : MonoBehaviour
{
    public void ReturnToTitleScreen()
    {
        SceneManager.LoadScene(0);
    }
}
