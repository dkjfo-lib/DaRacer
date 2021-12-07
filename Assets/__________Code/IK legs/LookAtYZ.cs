using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtYZ : MonoBehaviour
{
    public Transform target;

    Vector3 Direction => target.position - transform.position;
    Vector3 PlaneDirection => Vector3.ProjectOnPlane(Direction, transform.forward).normalized;

    float offset;

    void Start()
    {
        offset = Vector3.SignedAngle(PlaneDirection, transform.up, transform.forward);
    }

    float val(float da) => -da - offset;

    void Update()
    {
        var angleZ = Vector3.SignedAngle(PlaneDirection, transform.up, transform.forward);

        if (val(angleZ) < 1 && val(angleZ) > -1) return;

        transform.rotation = Quaternion.Euler(new Vector3(
            transform.rotation.eulerAngles.x,
            transform.rotation.eulerAngles.y,
            transform.rotation.eulerAngles.z + val(angleZ)));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * 10);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + PlaneDirection * 10);
    }
}
