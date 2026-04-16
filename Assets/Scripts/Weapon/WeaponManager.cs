using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WeaponManager : MonoBehaviour
{
    [Header("Fire Rate")]
    [SerializeField] float fireRate;
    [SerializeField] bool semiAuto;
    float fireRateTimer;

    [Header("Bullet")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform barrelPos;
    [SerializeField] float bulletVelocity;
    [SerializeField] int bulletsPerShot;
    public float damage =20;
    AimStateManager aim;

    [SerializeField] AudioClip gunShot;
    AudioSource audioSource;

    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] WFX_LightFlicker muzzleLight;

    WeaponBloom bloom;

    public float enemyKickbackForce = 100;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        aim = GetComponentInParent<AimStateManager>();
        fireRateTimer = fireRate;
        bloom = GetComponent<WeaponBloom>();
        muzzleFlash.Stop(true);
        muzzleLight.enabled = false;
        muzzleLight.GetComponent<Light>().enabled = false;
    }

    void Update()
    {
        if (ShouldFire()) Fire();
    }

    bool ShouldFire()
    {
        fireRateTimer += Time.unscaledDeltaTime;
        if (fireRateTimer < fireRate) return false;
        if (semiAuto && Input.GetKeyDown(KeyCode.Mouse0)) return true;
        if (!semiAuto && Input.GetKey(KeyCode.Mouse0)) return true;
        return false;
    }

    void Fire()
    {
        fireRateTimer = 0;
        barrelPos.LookAt(aim.aimPos);
        barrelPos.localEulerAngles = bloom.BloomAnngle(barrelPos);
        muzzleFlash.Play();
        muzzleLight.enabled = true;
        StartCoroutine(DisableMuzzleLight());
        audioSource.PlayOneShot(gunShot);
        for (int i = 0; i < bulletsPerShot; i++)
        {
            GameObject currentBullet = Instantiate(bullet, barrelPos.position, barrelPos.rotation);

            Bullet bulletScript = currentBullet.GetComponent<Bullet>();
            bulletScript.weapon = this;
            bulletScript.dir = barrelPos.transform.forward;

            Rigidbody rb = currentBullet.GetComponent<Rigidbody>();
            rb.AddForce(barrelPos.forward * bulletVelocity, ForceMode.Impulse);
        }
    }

    IEnumerator DisableMuzzleLight()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        muzzleLight.enabled = false;
        muzzleLight.GetComponent<Light>().enabled = false;
    }
}

