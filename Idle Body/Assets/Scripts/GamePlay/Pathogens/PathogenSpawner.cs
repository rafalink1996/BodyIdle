using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathogenSpawner : MonoBehaviour
{
    public OrganManager myOrganManager;
    [SerializeField] GameObject[] pathogenPrefabs;
    [SerializeField] List<GameObject> currentPathogens;
    bool infecting;
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
        yield return new WaitForSeconds(5f);
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
    public void SpawnPathogen(int pathogenId, bool random = true, Transform myTransform = null)
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
        GameObject pathogen = Instantiate(pathogenPrefabs[pathogenId], spawnPosition, Quaternion.identity);
        currentPathogens.Add(pathogen);
    }
}
