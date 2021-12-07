using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public ClampedValue CV_Forward;
    public ClampedValue CV_Sideways;
    public ClampedValue CV_Up;
    [Space]
    public Number N_Stage;
    public Number N_Velocity;
    [Space]
    public Stage[] Stages;
    public int curStageId = 0;
    public Stage curStage => Stages[curStageId];
    public float speed => curStage.maxSpeed;
    public float torque => curStage.torque;
    public float rotationProgression => curStage.rotationProgression;
    public float speedRotationInfluence => curStage.speedRotationInfluence;
    [Space]
    [Range(0, 1)] public float speedMult = .7f;
    [Range(0, 1)] public float SidewaysDump = .7f;
    [Range(0, 1)] public float speedMultGround = .7f;
    [Space]
    public float torqueSpeedDump = .5f;
    [Space]
    public float jumpForce = 200;
    [Space]
    [Range(0, 1)] public float rotationPower = .6f;
    [Space]
    public Transform car;
    public Transform CamHolder;

    Rigidbody Rigidbody;
    GroundDetector GroundDetector;
    bool wasInAir = false;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        GroundDetector = GetComponentInChildren<GroundDetector>();
        N_Velocity.Value = Rigidbody.velocity.magnitude;
        N_Stage.Value = curStageId;
    }

    void Update()
    {
        if (!PlayerSinglton.PlayerCanMove) return;

        N_Velocity.Value = Rigidbody.velocity.magnitude;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            curStageId = curStageId > 0 ? curStageId - 1 : 0;
            N_Stage.Value = curStageId;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            curStageId = curStageId < Stages.Length - 1 ? curStageId + 1 : Stages.Length - 1;
            N_Stage.Value = curStageId;
        }

        if (wasInAir && GroundDetector.onGround)
        {
            Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, 0, Rigidbody.velocity.z);
            CamHolder.localRotation = Quaternion.Euler(0, 0, 0);
        }
        wasInAir = !GroundDetector.onGround;
    }

    void FixedUpdate()
    {
        Vector4 input;
        input.x = 0;
        input.y = CV_Up.Value;
        input.z = CV_Forward.Value;
        input.w = CV_Sideways.Value;


        //rotate
        if (input.w != 0)
        {
            var speedRotation = Rigidbody.velocity.sqrMagnitude * torqueSpeedDump / (speed * speed);

            var _torque = torque / (1 + speedRotation);
            Rigidbody.AddTorque(new Vector3(0, input.w * _torque * Time.fixedDeltaTime, 0), ForceMode.Acceleration);
        }
        // move xz
        if (input.x != 0 || input.z != 0)
        {
            if (input.z == 0) input.z = input.x;
            var inputXZ = new Vector3(input.x, 0, input.z).normalized;
            var localInputXZ = inputXZ.x * transform.right + inputXZ.z * transform.forward;
            var addMovementXZ = localInputXZ * speed * Time.fixedDeltaTime;
            var newMovementXZ = Rigidbody.velocity + addMovementXZ;
            Rigidbody.velocity = newMovementXZ;
        }
        // move y
        if (input.y != 0)
        {
            var movementY = input.y * jumpForce * Time.fixedDeltaTime;
            Rigidbody.velocity += Vector3.up * movementY;
        }

        if (GroundDetector.onGround)
        {
            CamHolder.localRotation = Quaternion.Euler(0, 0, 0);
            Rigidbody.velocity = new Vector3(Rigidbody.velocity.x * speedMultGround, Rigidbody.velocity.y * speedMultGround, Rigidbody.velocity.z * speedMultGround);
        }
        else
        {
            var sidewaysMovement = Vector3.Project(Rigidbody.velocity, transform.forward);
            Rigidbody.velocity = new Vector3(Rigidbody.velocity.x * speedMult, Rigidbody.velocity.y * speedMult, Rigidbody.velocity.z * speedMult) - sidewaysMovement * SidewaysDump;

            var speedRotation = Rigidbody.velocity.sqrMagnitude / (speed * speed * speedRotationInfluence * speedRotationInfluence);
            CamHolder.localRotation = Quaternion.Euler(0, 0, -Rigidbody.angularVelocity.y * (rotationProgression + speedRotation));
        }
    }
}

[System.Serializable]
public class Stage
{
    public float maxSpeed = 100;
    public float torque = 400;
    [Range(0, 10)] public float speedRotationInfluence = .5f;
    [Range(0.01f, 1)] public float rotationProgression = 6f;
}