using UnityEngine;

public class NPCCarMovement : MonoBehaviour
{
    [SerializeField] float minOffsetSpeed = -2f; // slower than player
    [SerializeField] float maxOffsetSpeed = 5f;  // faster than player

    private Transform player;
    private float speedOffset;

    void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        speedOffset = Random.Range(minOffsetSpeed, maxOffsetSpeed);
    }

    void Update()
    {
        if (player == null)
            return;

        float playerSpeed = player.GetComponent<Rigidbody>().linearVelocity.z;

        float npcSpeed = Mathf.Max(2f, playerSpeed + speedOffset);
        transform.Translate(Vector3.forward * npcSpeed * Time.deltaTime);
    }
}
