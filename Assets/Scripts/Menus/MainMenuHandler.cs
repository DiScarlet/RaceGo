using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.SceneManagement.SceneManager;

public class MainMenuHandler : MonoBehaviour
{
    private const string fullStartScene = "StoryScene"; 
    private const string quickStartScene = "Stage";
    private const string settingsScene = "Settings";

    public void OnFullStartClick()
    {
        LoadScene(fullStartScene);
    }

    public void OnQuickStartClick()
    {
        LoadScene(quickStartScene);
    }

    public void OnSettingsClick()
    {
        LoadScene(settingsScene);
    }

    public void OnQuitClick()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
