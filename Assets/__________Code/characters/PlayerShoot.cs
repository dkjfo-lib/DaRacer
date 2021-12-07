using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public Weapon[] allWeapons;
    //public GameObjectArr[] allWeaponsModels;
    [Space]
    public int currentWeaponId = 0;
    public int buttonId = 0;
    [Space]
    public Pipe_SoundsPlay Pipe_SoundsPlay;
    public Transform gunpoint;
    [Space]
    public ClampedValue Addon_AmmoCountOutput;
    public bool canShoot = true;
    public float accuracityReplanishment = 1;
    public float currentAccuracity = 1;

    bool HasAimPoint => false;
    Vector3 AimPoint => Vector3.zero;
    Vector3 AimDirection => transform.forward;

    Weapon currentWeapon => allWeapons[currentWeaponId];
    bool isInMenu => Time.timeScale == 0;

    public Coroutine currentAction;

    void Start()
    {
        if (Addon_AmmoCountOutput != null)
        {
            Addon_AmmoCountOutput.Value = currentWeapon.primaryFire.ammoInClip;
            Addon_AmmoCountOutput.SetMaxValue(currentWeapon.primaryFire.clipSize);
        }
    }

    private void FixedUpdate()
    {
        // aim 
        currentAccuracity += accuracityReplanishment * Time.fixedDeltaTime;
    }

    void Update()
    {
        if (isInMenu) return;

        if (HasAimPoint)
        {
            transform.forward = AimPoint - transform.position;
        }
        else
        {
            transform.forward = AimDirection;
        }

        // change weapon
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            //allWeaponsModels[currentWeaponId].SetActive(false);
            currentWeaponId++;
            if (currentWeaponId > allWeapons.Length - 1)
                currentWeaponId = 0;
            if (!canShoot)
            {
                StopCoroutine(currentAction);
                canShoot = true;
            }
            //allWeaponsModels[currentWeaponId].SetActive(true);

            if (Addon_AmmoCountOutput != null)
            {
                Addon_AmmoCountOutput.Value = currentWeapon.primaryFire.ammoInClip;
                Addon_AmmoCountOutput.SetMaxValue(currentWeapon.primaryFire.clipSize);
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            //allWeaponsModels[currentWeaponId].SetActive(false);
            currentWeaponId--;
            if (currentWeaponId < 0)
                currentWeaponId = allWeapons.Length - 1;
            if (!canShoot)
            {
                StopCoroutine(currentAction);
                canShoot = true;
            }
            //allWeaponsModels[currentWeaponId].SetActive(true);

            if (Addon_AmmoCountOutput != null)
            {
                Addon_AmmoCountOutput.Value = currentWeapon.primaryFire.ammoInClip;
                Addon_AmmoCountOutput.SetMaxValue(currentWeapon.primaryFire.clipSize);
            }
        }

        // reload
        if (canShoot && currentWeapon.HasPrimary && currentWeapon.primaryFire.ammoInClip != currentWeapon.primaryFire.clipSize &&
            (Input.GetKeyDown(KeyCode.R) || currentWeapon.primaryFire.NeedsReloading))
        {
            currentAction = StartCoroutine(Reload(currentWeapon.primaryFire));
        }
        // shoot
        if (canShoot && currentWeapon.HasPrimary && Input.GetMouseButton(buttonId))
        {
            currentAction = ShotOrReaload(currentWeapon.primaryFire);
        }
    }

    Coroutine ShotOrReaload(ShotInfo shotInfo)
    {
        if (shotInfo.NeedsReloading)
        {
            return StartCoroutine(Reload(shotInfo));
        }
        else
        {
            return StartCoroutine(ShootWeapon(shotInfo));
        }
    }

    IEnumerator ShootWeapon(ShotInfo shotInfo)
    {
        canShoot = false;
        shotInfo.ammoInClip -= shotInfo.shotCost;
        Pipe_SoundsPlay.AddClip(new PlayClipData(shotInfo.fireSound, Camera.main.transform.position, Camera.main.transform));

        foreach (var burst in shotInfo.bursts)
        {
            for (int i = 0; i < burst.count; i++)
            {
                var newProjectile = Instantiate(burst.projectile, gunpoint.transform.position, transform.rotation);
                newProjectile.FactionToHit = Faction.OpponentTeam;
                currentAccuracity -= Random.Range(0, 1 - shotInfo.accuracity);
                currentAccuracity = Mathf.Clamp(currentAccuracity, shotInfo.accuracity, 1);
                newProjectile.transform.forward +=
                    newProjectile.transform.right * shotInfo.GetRandomDeviation(currentAccuracity) +
                    newProjectile.transform.up * shotInfo.GetRandomDeviation(currentAccuracity);

                if (Addon_AmmoCountOutput != null)
                {
                    Addon_AmmoCountOutput.Value = currentWeapon.primaryFire.ammoInClip;
                    Addon_AmmoCountOutput.SetMaxValue(currentWeapon.primaryFire.clipSize);
                }
            }

            if (shotInfo.delayBetweenProjectiles > 0)
                yield return new WaitForSeconds(shotInfo.delayBetweenProjectiles);
        }

        yield return new WaitForSeconds(1 / shotInfo.shotsPerSecond);
        canShoot = true;
    }

    IEnumerator Reload(ShotInfo shotInfo)
    {
        canShoot = false;
        yield return new WaitForSeconds(shotInfo.reloadTime);

        shotInfo.ammoInClip = shotInfo.clipSize;
        canShoot = true;

        if (Addon_AmmoCountOutput != null)
        {
            Addon_AmmoCountOutput.Value = currentWeapon.primaryFire.ammoInClip;
            Addon_AmmoCountOutput.SetMaxValue(currentWeapon.primaryFire.clipSize);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (HasAimPoint)
        {
            Gizmos.DrawLine(transform.position, AimPoint);
        }
        else
        {
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 100);
        }
    }
}

[System.Serializable]
public class ShotInfo
{
    public Burst[] bursts;
    public float delayBetweenProjectiles;
    [Space]
    [Tooltip("value of 1 generates deviation of up to 45 degrees from aim")]
    [Range(0, 1)] public float accuracity;
    public float shotsPerSecond;
    [Space]
    public int shotCost;
    public int clipSize;
    public int ammoInClip;
    public float reloadTime;
    [Space]
    public ClipsCollection fireSound;

    public bool NeedsReloading => ammoInClip < shotCost;

    public float GetDeviationValue() => 1 - accuracity;
    public float GetDeviationValue(float customAccuracity) => 1 - customAccuracity;
    public float GetRandomDeviation() => Random.Range(-GetDeviationValue(), GetDeviationValue());
    public float GetRandomDeviation(float customAccuracity) => Random.Range(-GetDeviationValue(customAccuracity), GetDeviationValue(customAccuracity));
}

[System.Serializable]
public struct Burst
{
    public GunShot projectile;
    public int count;
}

[System.Serializable]
public struct GameObjectArr
{
    public GameObject[] gameObjects;

    public void SetActive(bool state)
    {
        foreach (var go in gameObjects)
        {
            go.SetActive(state);
        }
    }
}