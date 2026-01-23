using System.Collections;
using UnityEngine;

public class NPCCarSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] carNpcPrefabs;
    [SerializeField] LayerMask otherCarsLayerMask;

    [Header("Pool")]
    [SerializeField] int poolSize = 20;

    [Header("Spawning")]
    [SerializeField] float minSpawnDistance = 30f;
    [SerializeField] float maxSpawnDistance = 80f;
    [SerializeField] float minDistanceBetweenCars = 25f;
    [SerializeField] int minCarsAhead = 3;

    private GameObject[] carNpcPool;
    private Transform playerCarTransform;
    private Rigidbody playerRb;

    private WaitForSeconds wait = new WaitForSeconds(0.5f);
    private Collider[] overlapResults = new Collider[1];

    void Start()
    {
        playerCarTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerRb = playerCarTransform.GetComponent<Rigidbody>();

        carNpcPool = new GameObject[poolSize];

        int prefabIndex = 0;
        for (int i = 0; i < poolSize; i++)
        {
            carNpcPool[i] = Instantiate(carNpcPrefabs[prefabIndex]);
            carNpcPool[i].SetActive(false);

            prefabIndex++;
            if (prefabIndex >= carNpcPrefabs.Length)
                prefabIndex = 0;
        }

        StartCoroutine(UpdateTrafficCO());
    }

    IEnumerator UpdateTrafficCO()
    {
        while (true)
        {
            CleanUpCars();
            MaintainTrafficAhead();
            yield return wait;
        }
    }

    void MaintainTrafficAhead()
    {
        float playerZ = playerCarTransform.position.z;
        float playerSpeed = Mathf.Max(0f, playerRb.linearVelocity.z);

        float dynamicSpawnDistance = Mathf.Lerp(
            minSpawnDistance,
            maxSpawnDistance,
            playerSpeed / 50f
        );

        int carsAhead = 0;
        float furthestCarZ = playerZ;

        foreach (GameObject car in carNpcPool)
        {
            if (!car.activeInHierarchy)
                continue;

            float z = car.transform.position.z;

            if (z > playerZ)
                carsAhead++;

            if (z > furthestCarZ)
                furthestCarZ = z;
        }

        if (carsAhead < minCarsAhead || furthestCarZ - playerZ < dynamicSpawnDistance)
        {
            SpawnCarAt(furthestCarZ + minDistanceBetweenCars);
        }
    }

    void SpawnCarAt(float zPos)
    {
        GameObject carToSpawn = null;

        foreach (GameObject car in carNpcPool)
        {
            if (!car.activeInHierarchy)
            {
                carToSpawn = car;
                break;
            }
        }

        if (carToSpawn == null)
            return;

        Vector3 spawnPos = new Vector3(0f, 0f, zPos);

        if (Physics.OverlapBoxNonAlloc(
            spawnPos,
            Vector3.one * 2f,
            overlapResults,
            Quaternion.identity,
            otherCarsLayerMask) > 0)
            return;

        carToSpawn.transform.position = spawnPos;
        carToSpawn.SetActive(true);
    }

    void CleanUpCars()
    {
        float playerZ = playerCarTransform.position.z;

        foreach (GameObject car in carNpcPool)
        {
            if (!car.activeInHierarchy)
                continue;

            float dz = car.transform.position.z - playerZ;

            if (dz > 220f || dz < -60f)
                car.SetActive(false);
        }
    }
}
