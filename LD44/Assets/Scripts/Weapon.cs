﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool pickable = true;
    public enum WeaponType { Handgun, Sniper, Gatling, Grenade, RPG, Sword}
    public WeaponType type;
    public GameObject bullet;
    public float rateOfAction;
    public bool canFire = true;
    public bool continuedShot;
    float rotationSpeed = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pickable)
        {
            transform.Rotate(new Vector3(0, rotationSpeed, 0));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bonhomme") && pickable)
        {
            pickable = false;
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
        if (canFire)
        {
            canFire = false;
            StartCoroutine(FireRate());

            if (type == WeaponType.Handgun)
            {
                GameObject b = Instantiate(bullet, origin.position, origin.rotation) as GameObject;
            }
        }

    }

    private IEnumerator FireRate()
    {
        yield return new WaitForSeconds(rateOfAction);
        canFire = true;
    }
}
