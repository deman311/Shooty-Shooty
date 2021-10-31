using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    int WEAPON_COUNT = 6;

    public GameObject gunSpawn, granadeSpawn;
    Transform[] spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoints = GetComponentsInChildren<Transform>(); // the first one is the container so i need to ignore it

        // spawn 5 pistols and 5 granades
        for(int i=0; i < 2*WEAPON_COUNT; i++)
        {
            int randIndex;
            do
            {
                randIndex = Random.Range(1, spawnPoints.Length);
            } while (!spawnPoints[randIndex].gameObject.activeInHierarchy);
            spawnPoints[randIndex].gameObject.SetActive(false);
            if (i < WEAPON_COUNT)
            {
                GameObject gun = Instantiate(gunSpawn, null);
                gun.transform.position = spawnPoints[randIndex].position;
            }
            else
            {
                GameObject granade = Instantiate(granadeSpawn, null);
                granade.transform.position = spawnPoints[randIndex].position;
            }
        }

        // disable all the otherspawn
        spawnPoints[0].gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
