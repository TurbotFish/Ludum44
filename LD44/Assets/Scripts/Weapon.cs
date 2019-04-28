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

    void Start()
    {
        
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
        }
        else if (other.CompareTag("vehicle") && pickable)
        {
            pickable = false;
            VehicleController v = other.GetComponent<VehicleController>();
            v.player.PickWeapon(this.gameObject);
            v.player.ExitVehicle();
            v.ResetVehicle();
        }
    }

    public void FireWeapon(Transform origin)
    {
        if (canFire && !fired)
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
                Debug.Log((Vector3.up + b.transform.forward));
            }
        }

    }

    private IEnumerator FireRate()
    {
        yield return new WaitForSeconds(rateOfAction);
        canFire = true;
    }
}
