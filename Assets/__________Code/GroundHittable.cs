using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundHittable : MonoBehaviour, IHittable
{
    public Faction Faction => Faction.AlwaysHit;

    public void GetHit(Hit hit)
    {

    }
}
