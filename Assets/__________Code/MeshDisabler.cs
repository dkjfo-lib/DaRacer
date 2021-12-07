using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDisabler : MonoBehaviour
{
    public Renderer[] controlledMeshes;
    public float rendDistance = 40;
    public Vector3[] togglePoints;
    public float rendDotAngle = .6f;
    [Space]
    [Range(1, 5)] public int updateEveryRrame = 2;

    public bool currentState = true;

    private void Start()
    {
        StartCoroutine(CheckSight());
    }

    IEnumerator CheckSight()
    {
        while (true)
        {
            bool playerHitted = false;
            var target = PlayerSinglton.PlayerPosition;

            foreach (var togglePoint in togglePoints)
            {
                var playerInAngle = DotAngle(transform.position + togglePoint, target);
                if (playerInAngle)
                {
                    playerHitted = RayCast(transform.position + togglePoint, target);
                    if (playerHitted)
                    {
                        break;
                    }
                }
            }

            if (currentState != playerHitted)
            {
                currentState = playerHitted;
                foreach (var item in controlledMeshes)
                {
                    item.enabled = playerHitted;
                }
            }
            for (int i = 0; i < updateEveryRrame; i++)
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }

    bool RayCast(Vector3 origin, Vector3 target)
    {
        Ray ray = new Ray(origin, target - origin);
        RaycastHit raycastHit;
        var hitted = Physics.Raycast(ray, out raycastHit, rendDistance, Layers.PlayerAndGround);
        if (hitted)
        {
            if (raycastHit.transform.GetComponent<PlayerSinglton>() != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    bool DotAngle(Vector3 origin, Vector3 target)
    {
        Vector3 dir = target - origin;
        Vector3 dirNorm = dir.normalized;

        //Vector3 playerForward = -PlayerSinglton.PlayerDirection;
        Vector3 playerForward = -Camera.main.transform.forward;
        Vector3 playerForwardNorm = playerForward.normalized;

        float dotProd = Vector3.Dot(dirNorm, playerForwardNorm);
        return dotProd > rendDotAngle;
    }

    private void OnDrawGizmos()
    {
        foreach (var togglePoint in togglePoints)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, transform.position + togglePoint);
            if (PlayerSinglton.IsGood)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position + togglePoint, PlayerSinglton.PlayerPosition);
            }
        }
    }
}
