using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI distanceTraveledText;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private CanvasGroup gameOverCanvasGroup;

    private CarHandler playerCarHandler;

    private void Awake()
    {
        playerCarHandler = GameObject.FindGameObjectWithTag("Player").GetComponent<CarHandler>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOverCanvasGroup.interactable = false;
        gameOverCanvasGroup.alpha = 0;

        playerCarHandler.OnPlayerCrashed += PlayerCarHandler_OnPlayerCrashed;
    }

    // Update is called once per frame
    void Update()
    {
        distanceTraveledText.text = playerCarHandler.DistanceTravelled.ToString("000000");
    }

    IEnumerator StartGameOverAnimationCO()
    {
        yield return new WaitForSecondsRealtime(3.0f);

        gameOverCanvasGroup.interactable = true;
        gameOverCanvasGroup.blocksRaycasts = true;

        while (gameOverCanvasGroup.alpha < 0.8f)
        {
            gameOverCanvasGroup.alpha = Mathf.MoveTowards(gameOverCanvasGroup.alpha, 1.0f, Time.unscaledDeltaTime * 2);


            yield return null;
        }
    }

    //Events
    private void PlayerCarHandler_OnPlayerCrashed(CarHandler obj)
    {
        gameOverText.text = $"DISTANCE: {distanceTraveledText.text}";

        StartCoroutine(StartGameOverAnimationCO());
    }

    public void OnRestartClicked()
    {
        //Restore time scale
        Time.timeScale = 1.0f;
        gameOverCanvasGroup.blocksRaycasts = false;


        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
