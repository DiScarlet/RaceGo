using UnityEngine;
using TMPro;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI distanceTraveledText;

    private CarHandler playerCarHandler;

    private void Awake()
    {
        playerCarHandler = GameObject.FindGameObjectWithTag("Player").GetComponent<CarHandler>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distanceTraveledText.text = playerCarHandler.DistanceTravelled.ToString("000000");
    }
}
