using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFactionAlignment", menuName = "My/FactionAlignment")]
public class FactionAlignment : ScriptableObject
{
    public Faction faction;
    public Faction[] enemyFactions;

    public bool IsEnemy(Faction otherFaction)
    {
        return enemyFactions.Contains(otherFaction);
    }
}
