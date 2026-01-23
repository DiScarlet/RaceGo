using System.Collections;
using UnityEngine;

public class NPCCarSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] carNpcPrefabs;

    private GameObject[] carNpcPool = new GameObject[20];
    private Transform playerCarTransform;

    //Timing
    private float timeLastCarSpawned = 0;
    private WaitForSeconds wait = new WaitForSeconds(0.5f);

    //Overlapped check
    [SerializeField] LayerMask otherCarsLayerMask;
    private Collider[] overlappedCheckCollider = new Collider[1];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCarTransform = GameObject.FindGameObjectWithTag("Player").transform;

        int prefabIndex = 0;

        for (int i = 0; i < carNpcPool.Length; i++)
        {
            carNpcPool[i] = Instantiate(carNpcPrefabs[prefabIndex]);
            carNpcPool[i].SetActive(false);

            prefabIndex++;

            if (prefabIndex > carNpcPrefabs.Length - 1)
                prefabIndex = 0;
        }

        StartCoroutine(UpdateLessOftenCO());
    }

    private IEnumerator UpdateLessOftenCO()
    {
        while (true)
        {
            CleanUpCarsBeyondView();
            SpawnNewCars();

            yield return wait;
        }
    }

    private void SpawnNewCars()
    {
        if (Time.time - timeLastCarSpawned < 2)
            return;

        GameObject carToSpawn = null;

        //Find a car to spawn
        foreach (GameObject npcCar in carNpcPool)
        {
            if (npcCar.activeInHierarchy)
                continue;

            carToSpawn = npcCar;
            break;
        }

        if (carToSpawn == null)
            return;
        Vector3 spawnPosition = new Vector3(0, 0, playerCarTransform.position.z + 40);

        if (Physics.OverlapBoxNonAlloc(spawnPosition, Vector3.one * 2, overlappedCheckCollider, Quaternion.identity, otherCarsLayerMask) > 0)
            return;

        carToSpawn.transform.position = spawnPosition;
        carToSpawn.SetActive(true);

        timeLastCarSpawned = Time.time;
    }

    private void CleanUpCarsBeyondView()
    {
        foreach (GameObject npcCar in carNpcPool)
        {
            if (!npcCar.activeInHierarchy)
                continue;

            if (npcCar.transform.position.z - playerCarTransform.position.z > 200)
                npcCar.SetActive(false);

            if (npcCar.transform.position.z - playerCarTransform.position.z < -50)
                npcCar.SetActive(false);
        }
    }
}