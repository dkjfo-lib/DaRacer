using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerHittable : MonoBehaviour, IHittable
{
    public float maxhp;
    public float hp = 3;
    public Faction faction = Faction.AlwaysHit;
    public Faction Faction => faction;
    [Space]
    public ClampedValue Addon_HPValueOutput;
    public NavMeshAgent addon_BotNavMesh;
    [Space]
    public Pipe_SoundsPlay Pipe_SoundsPlay;
    public ClipsCollection sounds_hit;
    public ClipsCollection sounds_bodyPartDestroed;

    private void Awake()
    {
        maxhp = hp;
        if (Addon_HPValueOutput != null) Addon_HPValueOutput.SetMaxValue(hp);
        if (Addon_HPValueOutput != null) Addon_HPValueOutput.Value = hp;
    }

    public void GetHit(Hit hit)
    {
        hp -= hit.damage;
        if (Addon_HPValueOutput != null) Addon_HPValueOutput.Value = hp;
        if (hp <= 0)
        {
            Pipe_SoundsPlay?.AddClip(new PlayClipData(sounds_bodyPartDestroed, transform.position));
            Die();
        }
        else
        {
            Pipe_SoundsPlay?.AddClip(new PlayClipData(sounds_hit, transform.position));
            if (addon_BotNavMesh != null)
            {
                addon_BotNavMesh.SetDestination(PlayerSinglton.PlayerPosition);
            }
        }
    }

    public void Heal(float heal)
    {
        hp += heal;
        if (hp > maxhp) hp = maxhp;
        if (Addon_HPValueOutput != null) Addon_HPValueOutput.Value = hp;
    }

    private void Die()
    {
        Destroy(transform.parent.parent.gameObject);
    }
}
