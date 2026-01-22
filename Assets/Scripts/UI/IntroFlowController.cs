using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections;


public class IntroFlowController : MonoBehaviour
{
    [Header("Canvases")]
    [SerializeField] GameObject textCanvas;
    [SerializeField] GameObject videoCanvas;
    [SerializeField] GameObject creditsCanvas;

    [Header("Video")]
    [SerializeField] VideoPlayer videoPlayer;

    [Header("Timing")]
    [SerializeField] float creditsDuration = 10f;

    [Header("Navigation")]
    [SerializeField] private TransitionHandler transitionHandler;
    [SerializeField] private CanvasTransitionHandler canvasTransitionHandler;
    public string nextScene = "Stage";


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartIntro();
    }

    private void StartIntro()
    {
        DisableAll();
        textCanvas.SetActive(true);
    }

    public void OnTextFinished()
    {
        canvasTransitionHandler.SwitchCanvas(textCanvas, videoCanvas);

        videoPlayer.Play();
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        videoPlayer.loopPointReached -= OnVideoFinished;
        StartCoroutine(ShowCredits());
    }

    private IEnumerator ShowCredits()
    {
        canvasTransitionHandler.SwitchCanvas(videoCanvas, creditsCanvas);

        yield return new WaitForSeconds(creditsDuration);

        transitionHandler.LoadNextScene(nextScene);
    }

    private void DisableAll()
    {
        textCanvas.SetActive(false);
        videoCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
    }
}
