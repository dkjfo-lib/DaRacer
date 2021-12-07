using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotStats : MonoBehaviour
{
    public float SightRange = 10;
    public float BlindCloseRange = 1.2f;
    [Range(-1, 1)]
    public float SightAngleDotProd = .25f;
    [Space]
    public float MovementSpeed = 10;
    public float RotationSpeed = 720;
    [Space]
    public float AttackRange = 5;
    [Range(-1, 1)]
    public float AttackAngleDotProd = .9f;
    [Space]
    public Weapon Weapon;
    public float BotAimTime = .75f;
    public float ShotsInValley = 1;



    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
        // draw AttackAngleDotProd

        // Sight
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, SightRange);
        Gizmos.DrawWireSphere(transform.position, BlindCloseRange);
        // draw SightAngleDotProd
    }
}
