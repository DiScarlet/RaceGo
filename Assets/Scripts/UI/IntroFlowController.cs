using UnityEngine;
using UnityEngine.Video;
using System.Collections;
using System;

public class IntroFlowController : MonoBehaviour
{
    public static IntroFlowController Instance;

    [Header("Canvases")]
    [SerializeField] GameObject introVideoCanvas;
    [SerializeField] GameObject textCanvas;
    [SerializeField] GameObject refVideoCanvas;
    [SerializeField] GameObject creditsCanvas;

    [Header("Videos")]
    [SerializeField] VideoPlayer introVideoPlayer;
    [SerializeField] VideoPlayer refVideoPlayer;

    [Header("Timing")]
    [SerializeField] float creditsDuration = 10f;

    [Header("Navigation")]
    [SerializeField] TransitionHandler transitionHandler;
    [SerializeField] CanvasTransitionHandler canvasTransitionHandler;
    [SerializeField] string nextScene = "Stage";

    // Event to notify when to play a text block
    public event Action<int> OnTextBlockRequested;

    private int textStage = 0;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        DisableAll();
        PlayIntroVideo();
    }

    // ================= FLOW =================

    void PlayIntroVideo()
    {
        Debug.Log("Vid1 started");
        introVideoCanvas.SetActive(true);

        introVideoPlayer.loopPointReached += OnIntroVideoFinished;
        introVideoPlayer.Play();
    }

    void OnIntroVideoFinished(VideoPlayer vp)
    {
        introVideoPlayer.loopPointReached -= OnIntroVideoFinished;
        canvasTransitionHandler.SwitchCanvas(introVideoCanvas, textCanvas);
        Debug.Log("Text1 started");
        OnTextBlockRequested?.Invoke(0);

        textStage = 1; // first text
        Debug.Log("Vid1 finished");
       
    }

    // CALLED FROM TEXT SCRIPT
    public void OnTextFinished()
    {
        Debug.Log("OnTextFinished logged!" + textStage);
        if (textStage == 1)
        {
            ShowRefVideo();
            Debug.Log("Text1 finished");
        }
        else if (textStage == 2)
        {
            ShowCredits();
            Debug.Log("Text2 finished");
        }
    }

    void ShowRefVideo()
    {
        Debug.Log("Vid2 started");
        canvasTransitionHandler.SwitchCanvas(textCanvas, refVideoCanvas);

        refVideoPlayer.loopPointReached += OnRefVideoFinished;
        refVideoPlayer.Play();
    }

    void OnRefVideoFinished(VideoPlayer vp)
    {
        refVideoPlayer.loopPointReached -= OnRefVideoFinished;

        Debug.Log("Vid2 finished");
        canvasTransitionHandler.SwitchCanvas(refVideoCanvas, textCanvas);
        OnTextBlockRequested?.Invoke(1);
        Debug.Log("Text2 started");
        textStage = 2; // second text
    }

    void ShowCredits()
    {
        Debug.Log("Credits started");
        StartCoroutine(CreditsCO());
    }

    IEnumerator CreditsCO()
    {
        canvasTransitionHandler.SwitchCanvas(textCanvas, creditsCanvas);

        yield return new WaitForSeconds(creditsDuration);
        Debug.Log("Credits finished");
        transitionHandler.LoadNextScene(nextScene);
    }

    // ================= UTILS =================

    void DisableAll()
    {
        introVideoCanvas.SetActive(false);
        textCanvas.SetActive(false);
        refVideoCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
    }
}
