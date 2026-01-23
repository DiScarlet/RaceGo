using UnityEngine;
using System.Collections;
using TMPro;

public class IntroSceneController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI storyText;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Timing")]
    [SerializeField] private float fadeInTime = 1f;
    [SerializeField] private float visibleTime = 1.5f;
    [SerializeField] private float fadeOutTime = 1f;

    [SerializeField] private IntroFlowController introFlowController;


    private readonly string[] storyLines1 =
    {
        "After a Grand Prix, the race was over — but the journey wasn’t.",
        "Formula 1 drivers Charles Leclerc and Carlos Sainz were heading home together.",
        "Bad weather shut down private flights.",
        "No jets. No shortcuts.",
        "So they did the unexpected.",
        "They rented a van and took the road instead.",
        "Later, they learned another driver’s jet landed safely at the same time.",
        "Charles Leclerc shared the moment on Instagram.",
        "That story became the spark for this game."
    };

    private readonly string[] storyLines2 =
    {
        "You are a professional racing driver.",
        "The Grand Prix is over, but the journey home has just begun.",
        "With no usual transport available, there is only one option left —",
        "the racing car itself.",
        "Public roads replace the circuit.",
        "Traffic, buildings, and everyday obstacles fill the streets.",
        "The road never ends.",
        "One mistake ends the journey.",
        "How far can you drive before the streets claim the car?"
    };
    
    private int currentTextBlock = 0;

    private void Awake()
    {
        canvasGroup.alpha = 0f;

        introFlowController.OnTextBlockRequested += PlayTextBlock;
    }

    public void PlayTextBlock(int blockIndex)
    {
        StopAllCoroutines(); // safety

        canvasGroup.alpha = 1f;
        canvasGroup.gameObject.SetActive(true);

        if (blockIndex == 0)
            StartCoroutine(PlayIntro(storyLines1));
        else if (blockIndex == 1)
            StartCoroutine(PlayIntro(storyLines2));
    }


    private IEnumerator PlayIntro(string[] lines)
    {
        foreach (var line in lines)
        {
            storyText.text = line;

            yield return Fade(0f, 1f, fadeInTime);
            yield return new WaitForSeconds(visibleTime);
            yield return Fade(1f, 0f, fadeOutTime);
        }

        // Notify IntroFlowController that this text block is finished
        introFlowController.OnTextFinished();
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        float elapsed = 0f;

        canvasGroup.alpha = from;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = to;
    }

    void OnDestroy()
    {
        if (introFlowController != null)
            introFlowController.OnTextBlockRequested -= PlayTextBlock;
    }
}
