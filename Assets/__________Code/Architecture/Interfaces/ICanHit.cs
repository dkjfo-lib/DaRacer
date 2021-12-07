using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICanHit
{
    Object CoreObject { get; }
    bool IsSelfDamageOn { get; }
    bool IsFriendlyDamageOn { get; }

    bool IsEnemy(Faction faction);
}

public static class Ext_ICanHit
{
    public static bool ShouldHit(this ICanHit hitting, IHittable hitted)
    {
        if (hitted == null) return false;

        bool isOneself = hitting.CoreObject == (Object)hitted;
        bool selfDamageIsOn = hitting.IsSelfDamageOn;
        bool shoulHitSelf = isOneself && selfDamageIsOn;
        if (shoulHitSelf) return true;

        bool friendlyFireIsOn = hitting.IsFriendlyDamageOn;
        bool isEnemy = hitting.IsEnemy(hitted.Faction);
        bool shoulHitOther = !isOneself && (friendlyFireIsOn || isEnemy);
        if (shoulHitOther) return true;

        bool alwaysHitted = hitted.Faction == Faction.AlwaysHit;
        if (alwaysHitted) return alwaysHitted;
        return false;
    }
}