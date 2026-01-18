using System.Collections;
using UnityEngine;

public class NpcHamdler : MonoBehaviour
{
    [SerializeField] CarHandler carHandler;

    //Check if hitting other cars
    [SerializeField] LayerMask otherCarsLayerMask;
    [SerializeField] MeshCollider meshCollider;
    private RaycastHit[] raycastHits = new RaycastHit[1];
    private bool isCarAhead = false;
    private float carAheadDistance = 0;

    [Header("SFX")]
    [SerializeField] AudioSource honkHornAS;

    //Timing
    WaitForSeconds wait = new WaitForSeconds(0.2f);

    //Lanes
    private int drivingInLane = 0;

    private void Awake()
    {
        if(CompareTag("Player"))
        {
            Destroy(this);
            return;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(UpdateSectionLessOftenCO());
    }

    // Update is called once per frame
    void Update()
    {
        float accelerationInput = 1.0f;
        float steeringInput = 0.0f;

        if (isCarAhead)
        {
            accelerationInput = -1;

            if(carAheadDistance < 10 && !honkHornAS.isPlaying)
            {
                honkHornAS.pitch = Random.Range(0.5f, 1.1f);
                honkHornAS.Play();
            }
        }

        float desiredPositionX = Utils.CarLanes[drivingInLane];
        float difference = desiredPositionX - transform.position.x;

        if (Mathf.Abs(difference) > 0.05f)
            steeringInput = 1.0f * difference;

        steeringInput = Mathf.Clamp(steeringInput, -1.0f, 1.0f);

        carHandler.SetInput(new Vector2(steeringInput, accelerationInput));
    }

    IEnumerator UpdateSectionLessOftenCO()
    {
        while (true)
        {
            isCarAhead = CheckIfOtherCarIsAhead();
            yield return wait;
        }
    }

    private bool CheckIfOtherCarIsAhead()
    {
        meshCollider.enabled = false;

        int numbOfHits = Physics.BoxCastNonAlloc(transform.position, Vector3.one * 0.25f, transform.forward, raycastHits, Quaternion.identity, 2, otherCarsLayerMask);

        meshCollider.enabled = true;

        if (numbOfHits > 0)
        {
            carAheadDistance = (transform.position - raycastHits[0].point).magnitude;
            return true;
        }
        else
            return false;
    }

    //Events
    private void OnEnable()
    {
        //Set random initial speed
        carHandler.SetMaxSpeed(Random.Range(2, 4));

        //Set random lane
        drivingInLane = Random.Range(0, Utils.CarLanes.Length);

        // **Reset Z relative to player**
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 pos = transform.position;
        pos.x = Utils.CarLanes[drivingInLane];
        pos.z = playerTransform.position.z + UnityEngine.Random.Range(20f, 50f); // spawn 20–50 units ahead
        transform.position = pos;
    }
}
