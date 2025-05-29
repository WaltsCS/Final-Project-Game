using UnityEngine;

public class QuitGame : MonoBehaviour
{
    ///Call this from UI Button's OnClick to quit the application.
    public void Quit()
    {
        ///If running in the editor, stop play mode
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}