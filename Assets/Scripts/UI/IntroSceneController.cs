using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class IntroSceneController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI storyText;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Timing")]
    [SerializeField] private float fadeInTime = 1f;
    [SerializeField] private float visibleTime = 1.5f;
    [SerializeField] private float fadeOutTime = 1f;

    [Header("Next Scene")]
    [SerializeField] private string nextSceneName = "Stage";

    private readonly string[] storyLines =
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGroup.alpha = 0f;
        StartCoroutine(PlayIntro());
    }

    private IEnumerator PlayIntro()
    {
        foreach (var line in storyLines)
        {
            storyText.text = line;

            yield return Fade(0f, 1f, fadeInTime);
            yield return new WaitForSeconds(visibleTime);
            yield return Fade(1f, 0f, fadeOutTime);
        }
        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        float elapsed = 0f;

        canvasGroup.alpha = from;

        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = to;
    }
}
