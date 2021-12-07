using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DitzelGames.FastIK;

public class Leg : MonoBehaviour
{
    public FastIKFabric feetIK;
    public LegTargetCaster targetCaster;
    public LookAtYZ look;
    [Space]
    public float stepDist = 2;
    public float stepHeight = 2;
    public float legSpeed = 10;
    [Space]
    public bool IsWalking = false;
    Vector3 diff() => targetCaster.targetPosition - target.position;
    float distSqr() => diff().sqrMagnitude;
    public bool NeedsWalking => distSqr() > stepDist * stepDist;

    Transform target;
    Vector3 targetEulerOffset;

    void Start()
    {
        target = feetIK.Target;
        targetEulerOffset = target.eulerAngles - transform.eulerAngles;
        Step();
        //StartCoroutine(UpdateLeg());
    }

    //IEnumerator UpdateLeg()
    //{
    //    if (targetCaster.HasTarget)
    //    {
    //        yield return Step(diff().normalized * stepDist);
    //    }
    //    while (true)
    //    {
    //        yield return new WaitUntil(() => targetCaster.HasTarget && NeedsWalking);
    //        yield return Step(diff().normalized * stepDist);
    //    }
    //}

    //private void Update()
    //{
    //    if (!IsWalking && NeedsWalking)
    //    {
    //        Step();
    //    }
    //}

    public void Step()
    {
        Vector3 diff = targetCaster.targetPosition - target.position;
        StartCoroutine(Step(diff));
    }

    IEnumerator Step(Vector3 direction)
    {
        IsWalking = true;

        var lerp = 0f;
        //var lerpV = 0f;
        var startPos = target.transform.position;
        var endPos = startPos + direction;

        //var startRot = target.eulerAngles;
        //var endRot = transform.eulerAngles + targetEulerOffset;
        //while (lerpV < .5f)
        //{
        //    Vector3 newPos = startPos;
        //    newPos -= look.transform.up.normalized * Mathf.Sin(lerpV * Mathf.PI) * stepHeight;
        //    target.transform.position = newPos;

        //    lerpV += Time.deltaTime * legSpeed;
        //    yield return new WaitForEndOfFrame();
        //}
        while (lerp < 1)
        {
            Vector3 newPos = Vector3.Lerp(startPos, endPos, lerp);
            newPos -= look.transform.up.normalized * Mathf.Sin(lerp * Mathf.PI) * stepHeight;
            target.transform.position = newPos;
            //target.eulerAngles = Vector3.Lerp(startRot, endRot, lerp);

            lerp += Time.deltaTime * legSpeed;
            yield return new WaitForEndOfFrame();
        }
        //while (lerpV < 1f)
        //{
        //    Vector3 newPos = endPos;
        //    newPos -= look.transform.up.normalized * Mathf.Sin(lerpV * Mathf.PI) * stepHeight;
        //    target.transform.position = newPos;

        //    lerpV += Time.deltaTime * legSpeed;
        //    yield return new WaitForEndOfFrame();
        //}
        target.transform.position = endPos;
        IsWalking = false;
    }

    void OnDestroy()
    {
        Destroy(target.gameObject);
    }
}
