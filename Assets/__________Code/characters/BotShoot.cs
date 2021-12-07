using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotShoot : MonoBehaviour
{
    public Weapon weapon => botStats.Weapon;
    public float botAimTime => botStats.BotAimTime;
    public float attackAngleDotProd => botStats.AttackAngleDotProd;
    [Space]
    public Pipe_SoundsPlay Pipe_SoundsPlay;
    public Transform gunpoint;
    [Space]
    public bool canShoot = true;
    [Space]
    public BotSight BotSight;
    public BotMovement BotMovement;
    public BotStats botStats;

    void Update()
    {
        if (!PlayerSinglton.IsGood) return;
        if (canShoot && BotSight.CanSee && BotMovement.InDistanceForAttack && IsEnemyInAngleForAttack())
        {
            StartCoroutine(ShootWeapon(weapon.primaryFire));
        }
    }

    bool IsEnemyInAngleForAttack()
    {
        return BotSight.dotProductToPlayer > attackAngleDotProd;
    }

    IEnumerator ShootWeapon(ShotInfo shotInfo)
    {
        canShoot = false;
        BotMovement.inAttack = true;
        Vector3 shootDirection = (PlayerSinglton.PlayerPosition - transform.position ).normalized;

        yield return new WaitForSeconds(botAimTime);

        
        for (int i = 0; i < botStats.ShotsInValley; i++)
        {
            Pipe_SoundsPlay.AddClip(new PlayClipData(shotInfo.fireSound, transform.position));

            foreach (var burst in shotInfo.bursts)
            {
                for (int shotCount = 0; shotCount < burst.count; shotCount++)
                {
                    var newProjectile = Instantiate(burst.projectile, gunpoint.transform.position, transform.rotation);
                    newProjectile.FactionToHit = Faction.PlayerTeam;
                    newProjectile.transform.forward +=
                        newProjectile.transform.right * shotInfo.GetRandomDeviation() +
                        newProjectile.transform.up * shotInfo.GetRandomDeviation();
                }

                if (shotInfo.delayBetweenProjectiles > 0)
                    yield return new WaitForSeconds(shotInfo.delayBetweenProjectiles);
            }
            yield return new WaitForSeconds(1 / shotInfo.shotsPerSecond);
        }

        canShoot = true;
        BotMovement.inAttack = false;
    }
}
