using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegTargetCaster : MonoBehaviour
{
    public bool HasTarget = false;
    public Vector3 targetPosition = Vector3.zero;
    [SerializeField] float updateTime = .1f;
    [SerializeField] float raycastDepth = 10f;

    void Awake()
    {
        CalculateTargetPosition();
    }

    void Start()
    {
        StartCoroutine(UpdateTargetPosition());
    }

    IEnumerator UpdateTargetPosition()
    {
        while (true)
        {
            yield return new WaitForSeconds(updateTime);
            CalculateTargetPosition();
        }
    }

    private void CalculateTargetPosition()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        if (Physics.Raycast(ray, out RaycastHit hit, raycastDepth, Layers.Ground))
        {
            HasTarget = true;
            targetPosition = hit.point + Vector3.up * 0.35f;
        }
        else
        {
            HasTarget = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, transform.position - transform.up * raycastDepth);
    }
}
