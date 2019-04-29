using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool pickable = true;
    public enum WeaponType { Handgun, Sniper, AssaultRifle, Grenade, RPG, Sword}
    public WeaponType type;
    public GameObject bullet;
    public GameObject actionZone;
    public float rateOfAction;
    public bool canFire = true;
    public bool fired;
    public bool continuedShot;
    float rotationSpeed = 3;
    public RuntimeAnimatorController playerAnim;
    [HideInInspector]
    public PlayerInfo owner;
    public List<AudioClip> pickUpSounds;
    AudioSource audioSource;
    public AudioClip firingSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (pickable)
        {
            transform.Rotate(new Vector3(0, rotationSpeed, 0));
            canFire = true;
            fired = false;
        }
        if (!CrowdController.firing)
        {
            fired = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bonhomme") && pickable)
        {
            other.GetComponent<BonhommeController>().PickWeapon(this.gameObject);

            audioSource.clip = pickUpSounds[Random.Range(0, pickUpSounds.Count)];
            audioSource.Play();
        }
        else if (other.CompareTag("vehicle") && pickable)
        {
            VehicleController v = other.GetComponent<VehicleController>();
            if (!v.player.noPick)
            {
                pickable = false;
                v.player.PickWeapon(this.gameObject);
                v.player.ExitVehicle(true);
                v.ResetVehicle();

                audioSource.clip = pickUpSounds[Random.Range(0, pickUpSounds.Count)];
                audioSource.Play();
            }

        }
    }

    public void FireWeapon(Transform origin)
    {
        /*if (canFire && !fired)
        {
            canFire = false;
            StartCoroutine(FireRate());
            if (!continuedShot)
            {
                fired = true;
            }

            if (type == WeaponType.Handgun || type == WeaponType.AssaultRifle || type == WeaponType.Sniper || type == WeaponType.RPG)
            {
                GameObject b = Instantiate(bullet, origin.position, origin.rotation) as GameObject;
                b.GetComponent<Bullet>().owner = owner;
            }
            else if (type == WeaponType.Sword)
            {
                GameObject zone = Instantiate(actionZone, transform.position + transform.forward*(actionZone.GetComponent<SphereCollider>().radius+0.5f), Quaternion.identity) as GameObject;
                zone.GetComponent<Explosion>().owner = owner;
            }
            else if (type == WeaponType.Grenade)
            {
                GameObject b = Instantiate(bullet, origin.position, origin.rotation) as GameObject;
                b.GetComponent<Rigidbody>().AddForce((Vector3.up*0.5f + b.transform.forward.normalized) * 100);
            }
        }*/

        StartCoroutine(FireWithDelay(origin, 0.05f));

    }

    private IEnumerator FireWithDelay(Transform origin, float delay)
    {
        if (canFire && !fired)
        {
            owner.GetComponent<BonhommeController>().anim.SetTrigger("Shoot");
            canFire = false;
            yield return new WaitForSeconds(delay);

            StartCoroutine(FireRate());
            if (!continuedShot)
            {
                fired = true;
            }

            if (type == WeaponType.Handgun || type == WeaponType.AssaultRifle || type == WeaponType.Sniper || type == WeaponType.RPG)
            {
                GameObject b = Instantiate(bullet, origin.position, origin.rotation) as GameObject;
                b.GetComponent<Bullet>().owner = owner;
            }
            else if (type == WeaponType.Sword)
            {
                GameObject zone = Instantiate(actionZone, owner.transform.position + owner.transform.forward * (actionZone.GetComponent<SphereCollider>().radius + 1f), Quaternion.identity) as GameObject;
                zone.GetComponent<Explosion>().owner = owner;
            }
            else if (type == WeaponType.Grenade)
            {
                GameObject b = Instantiate(bullet, origin.position, origin.rotation) as GameObject;
                b.GetComponent<Rigidbody>().AddForce((Vector3.up * 0.5f + b.transform.forward.normalized) * 100);
            }
        }
    }

    private IEnumerator FireRate()
    {
        yield return new WaitForSeconds(rateOfAction);
        canFire = true;
        yield return new WaitForSeconds(Random.Range(0,0.3f));
        audioSource.clip = firingSound;
        audioSource.pitch = 1 + Random.Range(-0.2f, 0.2f);
        audioSource.Play();

    }
}
