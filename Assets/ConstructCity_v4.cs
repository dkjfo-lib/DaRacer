using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using cg = CityGenerator;

public class ConstructCity_v4 : MonoBehaviour
{
    public GenerateCity_v4 gc;
    [Space]
    public GameObject normalPath;
    public GameObject normalStreet;
    public GameObject normalWall;
    [Space]
    public float pathWidth = 5;
    public float streetWidth = 10;
    public float wallHeight = 5;
    public float crossSectionRadius = 5;

    private void Start()
    {
        StartCoroutine(GenerateOnTimer());
    }

    IEnumerator GenerateOnTimer()
    {
        while (true)
        {
            yield return new WaitUntil(() => gc.hasUpdate);
            gc.hasUpdate = false;
            DestroyChildren();
            Generate();
        }
    }

    private void DestroyChildren()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    void Generate()
    {
        foreach (var cp in gc.crossPoints)
        {
            cg.GenerateIntersection(cp.point * gc.cellSize, cp.connectedRoads.Select(s => s * gc.cellSize).ToArray(), normalStreet, normalWall, transform, wallHeight, streetWidth, crossSectionRadius);
        }
        foreach (var street in gc.streets)
        {
            cg.GenerateStreet(street.Start * gc.cellSize, street.End * gc.cellSize, normalStreet, normalWall, transform, wallHeight, streetWidth, crossSectionRadius);
        }
    }
}
