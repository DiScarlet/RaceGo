using System.Collections;
using UnityEngine;

public class CanvasTransitionHandler : MonoBehaviour
{
    [Header("Transition Overlay")]
    [SerializeField] private CanvasGroup fadeCanvasGroup; // Fullscreen black image with CanvasGroup
    [SerializeField] private float transitionTime = 1f;

    /// <summary>
    /// Switch from one canvas to another with smooth fade.
    /// </summary>
    public void SwitchCanvas(GameObject fromCanvas, GameObject toCanvas)
    {
        StartCoroutine(SwitchCoroutine(fromCanvas, toCanvas));
    }

    private IEnumerator SwitchCoroutine(GameObject from, GameObject to)
    {
        // Ensure overlay is on top and fully transparent at start
        fadeCanvasGroup.alpha = 0f;
        fadeCanvasGroup.gameObject.SetActive(true);

        // Fade to black
        float elapsed = 0f;
        while (elapsed < transitionTime / 2f)
        {
            elapsed += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / (transitionTime / 2f));
            yield return null;
        }
        fadeCanvasGroup.alpha = 1f;

        // Switch canvases while fully covered
        if (from != null) from.SetActive(false);
        if (to != null) to.SetActive(true);

        // Fade back to transparent
        elapsed = 0f;
        while (elapsed < transitionTime / 2f)
        {
            elapsed += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / (transitionTime / 2f));
            yield return null;
        }

        fadeCanvasGroup.alpha = 0f;
        fadeCanvasGroup.gameObject.SetActive(false);
    }
}
