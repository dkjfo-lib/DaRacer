using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float timeBetweenSpawns = 3;
    public int spawnItems = 1;
    public bool spawnOnSatrt = false;
    public bool isSpawning = false;
    [Space]
    public List<GameObject> spawnedObjects;

    void Start()
    {
        if (spawnOnSatrt)
        {
            StartSpawning();
        }
    }

    public void StartSpawning()
    {
        StartCoroutine(KeepSpawning());
    }

    IEnumerator KeepSpawning()
    {
        isSpawning = true;
        for (int i = 0; i < spawnItems; i++)
        {
            yield return new WaitForSeconds(timeBetweenSpawns);
            var newItem = Instantiate(objectToSpawn, transform.position, transform.rotation);
            spawnedObjects.Add(newItem);
        }
        isSpawning = false;
    }
}
