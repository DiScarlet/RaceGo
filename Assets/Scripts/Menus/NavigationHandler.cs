using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.SceneManagement.SceneManager;

public class NavigationHandler : MonoBehaviour
{
    [SerializeField] private TransitionHandler transitionHandler;

    private const string fullStartScene = "IntroScene"; 
    private const string quickStartScene = "Stage";
    private const string settingsScene = "Settings";
    private const string mainMenuScene = "Main Menu";

    private void Awake()
    {
        UIHandler.MainMenuClicked += OnMainMenuClick;
        UIHandler.RestartClicked += OnQuickStartClick;
    }

    public void OnFullStartClick()
    {
        transitionHandler.LoadNextScene(fullStartScene);
    }

    public void OnQuickStartClick()
    {
        transitionHandler.LoadNextScene(quickStartScene);
    }

    public void OnSettingsClick()
    {
        transitionHandler.LoadNextScene(settingsScene);
    }
    public void OnMainMenuClick()
    {
        transitionHandler.LoadNextScene(mainMenuScene);
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
