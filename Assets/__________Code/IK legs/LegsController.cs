using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LegsController : MonoBehaviour
{
    public Transform controlledObj;
    public Leg[] pair1;
    public Leg[] pair2;

    Vector3 offsetPosition;
    public float offsetHeight = 0;
    [Range(0, 1)] public float roughness = .5f;

    Leg[] f;
    Leg[] s;
    private void Start()
    {
        offsetPosition = GetMediumPosition() - controlledObj.position;
        f = pair1;
        s = pair2;
    }

    void FixedUpdate()
    {
        if (NeedsStep(pair1) && NeedsStep(pair2) && AllowedToStep(s))
        {
            Step(f);
            f = f == pair1 ? pair2 : pair1;
            s = s == pair1 ? pair2 : pair1;
        }
        else if (ShouldStep(pair1, pair2))
        {
            Step(pair1);
        }
        else if (ShouldStep(pair2, pair1))
        {
            Step(pair2);
        }

        controlledObj.position = Vector3.Lerp(controlledObj.position, GetMediumPosition() + offsetPosition + Vector3.up * offsetHeight, roughness);
    }

    bool LegsAreMoving(IEnumerable<Leg> legs) => !legs.All(s => !s.IsWalking);

    bool ShouldStep(IEnumerable<Leg> thisLegs, IEnumerable<Leg> otherLegs) => NeedsStep(thisLegs) && AllowedToStep(otherLegs);
    bool NeedsStep(IEnumerable<Leg> thisLegs) => !thisLegs.All(s => !s.NeedsWalking && !s.IsWalking);
    bool AllowedToStep(IEnumerable<Leg> otherLegs) => !LegsAreMoving(otherLegs);

    void Step(IEnumerable<Leg> legs)
    {
        foreach (var leg in legs.Where(s => !s.IsWalking))
            leg.Step();
    }

    Vector3 GetMediumPosition()
    {
        Vector3 pos = Vector3.zero;
        float posY = 0;
        foreach (var item in pair1)
        {
            pos += item.targetCaster.targetPosition;
            pos += item.feetIK.Target.position;
            posY += item.feetIK.Target.position.y;
        }
        foreach (var item in pair2)
        {
            pos += item.targetCaster.targetPosition;
            pos += item.feetIK.Target.position;
            posY += item.feetIK.Target.position.y;
        }
        pos /= (pair1.Length + pair2.Length) * 2;
        posY /= (pair1.Length + pair2.Length);
        pos.y = posY;
        return pos;
    }
}
