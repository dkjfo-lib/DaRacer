using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShot : MonoBehaviour, ICanHit
{
    public float damage = 1;
    public float speed = 10;
    public float timeToLive = .75f;
    public Faction FactionToHit = Faction.AlwaysHit;
    [Space]
    public ParticleSystem addon_trail;

    public Object CoreObject => transform;
    public bool IsSelfDamageOn => false;
    public bool IsFriendlyDamageOn => false;
    public bool IsEnemy(Faction faction)
    {
        return faction == FactionToHit;
    }

    private void Start()
    {
        Destroy(gameObject, timeToLive);
    }

    void FixedUpdate()
    {
        var movement = transform.forward * speed * Time.fixedDeltaTime;
        transform.position += movement;
    }

    private void OnTriggerEnter(Collider collision)
    {
        var hittable = collision.GetComponent<IHittable>();
        if (hittable == null) return;

        if (this.ShouldHit(hittable))
        {
            hittable.GetHit(new Hit(damage));

            if (addon_trail != null)
            {
                addon_trail.transform.parent = transform.parent;
                Destroy(addon_trail.gameObject, addon_trail.main.duration);
            }

            Destroy(gameObject);
        }
    }
}
