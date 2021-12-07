using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour
{
    public PlayerSinglton playerPrefab;

    void Start()
    {
        OnDestroySpawn.isQuitting = false;
        StartCoroutine(RespawnPlayer());
    }

    IEnumerator RespawnPlayer()
    {
        yield return new WaitUntil(() => !PlayerSinglton.IsGood);
        Time.timeScale = .5f;
        yield return new WaitForSeconds(1);
        Time.timeScale = 1;

        OnDestroySpawn.isQuitting = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
