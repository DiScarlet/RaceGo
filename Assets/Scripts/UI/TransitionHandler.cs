using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionHandler : MonoBehaviour
{
    [SerializeField] private Animator transition;
    public float transitionTime = 1f;

    public void LoadNextScene(string nextLevelName)
    {
        StartCoroutine(LoadLevel(nextLevelName));
    }

    private IEnumerator LoadLevel(string nextLevelName)
    {
        Debug.Log("Transitioning!");
        //Play
        transition.SetTrigger("start");

        //Wait
        yield return new WaitForSeconds(transitionTime);

        //Load next
        SceneManager.LoadScene(nextLevelName);
    }
}
