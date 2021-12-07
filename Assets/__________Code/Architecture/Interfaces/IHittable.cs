using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHittable
{
    Faction Faction { get; }

    void GetHit(Hit hit);
}

public struct Hit
{
    public float damage;

    public Hit(float damage)
    {
        this.damage = damage;
    }
}

public enum Faction
{
    AlwaysHit,
    NeverHit,
    PlayerTeam,
    OpponentTeam,
}