using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionHandler : MonoBehaviour
{
    [SerializeField] private Animator transition;
    public float transitionTime = 1f;


    //SCENE TRANSITION
    public void LoadNextScene(string nextLevelName)
    {
        StartCoroutine(SceneTransition(nextLevelName));
    }

    private IEnumerator SceneTransition(string sceneName)
    {
        yield return PlayTransition();
        SceneManager.LoadScene(sceneName);
    }

    //TRANSITION ANIMATION
    private IEnumerator PlayTransition()
    {
        //Play
        transition.SetTrigger("start");

        //Wait
        yield return new WaitForSeconds(transitionTime);
    }
}
