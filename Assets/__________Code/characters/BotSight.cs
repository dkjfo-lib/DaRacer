using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSight : MonoBehaviour, IBotSight
{
    public BotStats BotStats;
    float sightRange => BotStats.SightRange;
    float blindCloseRange => BotStats.BlindCloseRange;
    float sightDotProduct => BotStats.SightAngleDotProd;
    public float offsetY = 1;
    public float additionalRaycastLength = 1;

    PlayerSinglton thePlayer;
    bool canSee = false;

    public bool CanSee => canSee;
    public bool EnemyIsNear => thePlayer != null;

    // "Vector3.up * 1" due to error in character's y coordinate
    public Vector3 vectorToPlayer => thePlayer.transform.position + Vector3.up * offsetY - transform.position;
    public Vector3 directionToPlayer => vectorToPlayer.normalized;
    public float distanceToPlayer => vectorToPlayer.magnitude;
    public float dotProductToPlayer => thePlayer != null ?
        Vector3.Dot(
            new Vector3(transform.forward.normalized.x, 0, transform.forward.normalized.z),
            new Vector3(directionToPlayer.x, 0, directionToPlayer.z)) :
        -2;

    void Start()
    {
        thePlayer = null;
        GetComponent<SphereCollider>().radius = sightRange;

        StartCoroutine(LookAtPlayer());
    }


    IEnumerator LookAtPlayer()
    {
        bool IsPlayerInFront() => dotProductToPlayer >= sightDotProduct;
        bool IsPlayerTooClose() => distanceToPlayer <= blindCloseRange;
        while (true)
        {
            yield return new WaitUntil(() => thePlayer != null && (IsPlayerInFront() || IsPlayerTooClose()));

            if (IsPlayerTooClose())
            {
                canSee = true;
            }
            else
            {
                Vector3 origin = transform.position + directionToPlayer * blindCloseRange;
                Vector3 direction = directionToPlayer;
                float castRadius = distanceToPlayer - blindCloseRange + additionalRaycastLength;

                if (Physics.Raycast(origin, direction, out RaycastHit raycastHit, castRadius, Layers.PlayerAndGround))
                {
                    canSee = true;
                    canSee = raycastHit.transform.GetComponent<PlayerSinglton>() != null;
                }
                else
                {
                    canSee = false;
                }
            }

            yield return new WaitForSeconds(.5f);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!PlayerSinglton.IsGood) return;

        var player = collision.transform.
            GetComponent<PlayerSinglton>();
        if (player == null) return;

        thePlayer = player;
    }

    private void OnTriggerExit(Collider collision)
    {
        if (!PlayerSinglton.IsGood) return;

        var player = collision.transform.parent.parent.
            GetComponent<PlayerSinglton>();
        if (player == null) return;

        thePlayer = null;
        canSee = false;
    }

    private void OnDrawGizmos()
    {
        if (thePlayer != null)
        {
            Gizmos.color = Color.red;

            Vector3 origin = transform.position + directionToPlayer * blindCloseRange;
            float castDistance = distanceToPlayer - blindCloseRange + .75f;
            Vector3 end = transform.position + directionToPlayer * castDistance;
            Gizmos.DrawLine(origin, end);
        }
    }
}

public interface IBotSight
{
    bool CanSee { get; }
    bool EnemyIsNear { get; }
}