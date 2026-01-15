using System.Collections;
using UnityEngine;

public class EndlessLevelHandler : MonoBehaviour
{
    [SerializeField] GameObject[] sectionsPrefab;

    GameObject[] sectionsPool = new GameObject[20];
    GameObject[] sections = new GameObject[10];

    Transform playerCarTransform;

    WaitForSeconds waitFor100ms = new WaitForSeconds(0.1f);

    const float sectionLength = 26;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCarTransform = GameObject.FindGameObjectWithTag("Player").transform;

        int prefabIndex = 0;

        //Pool for endless level
        for(int i = 0; i < sectionsPool.Length; i++)
        {
            sectionsPool[i] = Instantiate(sectionsPrefab[prefabIndex]);
            sectionsPool[i].SetActive(false);

            prefabIndex++;

            if (prefabIndex > sectionsPrefab.Length - 1)
                prefabIndex = 0;
        }

        //Add first sections to the road
        for(int i = 0; i < sections.Length; i++)
        {
            GameObject randomSection = GetRandomSectionFromPool();

            //Move section to position and activate
            //randomSection.transform.position = new Vector3(sectionsPool[i].transform.position.x, -10, i * sectionLength);
            randomSection.transform.position = new Vector3(0f, -10f, i * sectionLength);
            randomSection.SetActive(true);

            sections[i] = randomSection;
        }

        StartCoroutine(UpdateSectionLessOftenCO());
    }

    IEnumerator UpdateSectionLessOftenCO()
    {
        while(true)
        {
            UpdateSectionPositions();
            yield return waitFor100ms;
        }
    }

    private void UpdateSectionPositions()
    {
        for(int i = 0; i < sections.Length; i++)
        {
            //Check if section was passed
            if (sections[i].transform.position.z  - playerCarTransform.position.z < -sectionLength)
            {
                Vector3 lastSectionPosition = sections[i].transform.position;
                sections[i].SetActive(false);

                sections[i] = GetRandomSectionFromPool();

                sections[i].transform.position = new Vector3(lastSectionPosition.x, -10, lastSectionPosition.z + sectionLength * sections.Length);
                sections[i].SetActive(true);
            }
        }
    }

    private GameObject GetRandomSectionFromPool()
    {
        int randomIndex = Random.Range(0, sectionsPool.Length);

        bool isNewSectionFound = false;

        while (!isNewSectionFound)
        {
            if (!sectionsPool[randomIndex].activeInHierarchy)
                isNewSectionFound = true;
            else
            {
                randomIndex++;

                if (randomIndex > sectionsPool.Length - 1)
                    randomIndex = 0;
            }

        }
        return sectionsPool[randomIndex];
    }
}
