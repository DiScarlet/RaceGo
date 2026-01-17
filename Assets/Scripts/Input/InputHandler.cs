using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Car
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] CarHandler carHandler;

        private void Awake()
        {
            if (!CompareTag("Player"))
            {
                Destroy(this);
                return;
            }
        }
        private void Update()
        {
            Vector2 input = Vector2.zero;

            input.x = Input.GetAxis("Horizontal");
            input.y = Input.GetAxis("Vertical");

            carHandler.SetInput(input);

            if (Input.GetKeyDown(KeyCode.R))
            {
                //Restore timescale
                Time.timeScale = 1.0f;

                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}