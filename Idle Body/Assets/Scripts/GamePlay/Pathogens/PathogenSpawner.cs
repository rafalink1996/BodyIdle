using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathogenSpawner : MonoBehaviour
{
    public OrganManager myOrganManager;
    [SerializeField] GameObject[] pathogenPrefabs;
    [SerializeField] List<GameObject> currentPathogens;
    bool infecting;
    float timeToInfect = 5f;
    // Start is called before the first frame update
    void Start()
    {
        myOrganManager = GameManager.gameManager.organManager;
    }


    // Update is called once per frame
    void Update()
    {
        if (!infecting)
        {
            infecting = true;
            StartCoroutine(InfectOrgans());
        }
    }
    IEnumerator InfectOrgans()
    {
        yield return new WaitForSeconds(timeToInfect);
        int infectionId = Random.Range(0, 3);
        for (int o = 0; o < myOrganManager.organTypes.Length; o++)
        {
            for (int i = 0; i < myOrganManager.organTypes[o].organs.Count; i++)
            {
                float chance = Random.Range(0, 101);
                if (chance <= myOrganManager.organTypes[o].organs[i].infectionChance)
                {
                    AddPathogen(infectionId, o, i);
                    print("infected");
                }
            }
        }
        infecting = false;
    }
    public void AddPathogen(int pathogenId, int organType, int organ)
    {
        OrganManager.OrganType.OrganInfo.Pathogens newPathogen = new OrganManager.OrganType.OrganInfo.Pathogens();
        newPathogen.id = pathogenId;
        newPathogen.health = 5;
        myOrganManager.organTypes[organType].organs[organ].pathogensList.Add(newPathogen);
    }
    public void SpawnPathogens(bool random = true, Transform myTransform = null)
    {
        if (myOrganManager.organTypes[myOrganManager.activeOrganType].organs[myOrganManager.activeOrganID].pathogensList == null)
        {
            Debug.LogWarning("Organ manager is null!");
        }
        for (int p = currentPathogens.Count; p < myOrganManager.organTypes[myOrganManager.activeOrganType].organs[myOrganManager.activeOrganID].pathogensList.Count; p++)
        {
            Vector3 spawnPosition;
            Vector2 randomPosition = new Vector2(Random.Range(0.2f, 0.8f), Random.Range(0.2f, 0.8f));
            if (random)
            {
                spawnPosition = Camera.main.ViewportToWorldPoint(randomPosition);
            }
            else
            {
                Vector3 offset = new Vector2(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f));
                spawnPosition = myTransform.position + offset;
            }
            GameObject pathogen = Instantiate(pathogenPrefabs[myOrganManager.organTypes[myOrganManager.activeOrganType].organs[myOrganManager.activeOrganID].pathogensList[p].id], spawnPosition, Quaternion.identity);
            currentPathogens.Add(pathogen);
        }
    }
    public void DestroyPathogens()
    {
        for (int i = 0; i < currentPathogens.Count; i++)
        {
            Destroy(currentPathogens[i]);
        }
        currentPathogens.Clear();
    }
}
