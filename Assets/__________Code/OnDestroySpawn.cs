using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnDestroySpawn : MonoBehaviour
{
    public List<SpawnObject> spawnList;
    public static bool isQuitting = false;

    void OnApplicationQuit()
    {
        isQuitting = true;
    }

    private void OnDestroy()
    {
        if (isQuitting) return;
        foreach (var item in spawnList)
        {
            var newitem = Instantiate(item.item, transform.position, Quaternion.identity);
            if (item.deleteAfter > 0)
            {
                Destroy(newitem.gameObject, item.deleteAfter);
            }
        }
    }
}

[System.Serializable]
public struct SpawnObject
{
    public GameObject item;
    public float deleteAfter;
}