using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSinglton : MonoBehaviour
{
    static PlayerSinglton thePlayer;

    internal static bool IsGood => thePlayer != null;

    internal static Transform PlayerTransform => thePlayer.transform;
    internal static Vector3 PlayerPosition => thePlayer.transform.position;
    internal static Vector3 PlayerDirection => thePlayer.transform.forward;

    internal static PlayerHittable PlayerHealth => thePlayer.playerHealth;
    internal PlayerHittable playerHealth;

    internal static Quaternion CamRotation => thePlayer.camPosition.rotation;
    internal static Vector3 CamPosition => thePlayer.camPosition.position;
    internal static Transform CamAnchor => thePlayer.camPosition;
    public Transform camPosition;

    public static Vector3 Velocity => thePlayer.playerRigidbody.velocity;
    public static Rigidbody PlayerRigidbody => thePlayer.playerRigidbody;
    public Rigidbody playerRigidbody;

    public static Collider[] PlayerColliders => thePlayer.playerColliders;
    public Collider[] playerColliders;

    internal static bool PlayerCanMove = true;

    void Awake()
    {
        thePlayer = this;
    }
}
