using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonhommeController : MonoBehaviour
{
    Rigidbody rb;
    public bool alive;

    public Transform weaponHold;
    public Transform weaponFire;
    GameObject weaponGO;
    Weapon weapon;

    void Start()
    {
        alive = true;
        rb = this.GetComponent<Rigidbody>();

    }

    private void Update()
    {
        if (CrowdController.firing)
        {
            if (weapon != null)
            {
                weapon.FireWeapon(weaponFire);
            }
        }
    }

    void FixedUpdate()
    {
        if (alive)
        {
            Vector3 movement = this.transform.forward * CrowdController.moveInput * CrowdController.moveSpeed;
            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
            this.transform.Rotate(new Vector3(0, CrowdController.rotationInput * CrowdController.rotationSpeed, 0));

        }
    }

    public void Kill(Vector3 deathForce)
    {
        alive = false;
        rb.constraints = RigidbodyConstraints.None;
        rb.AddForce(deathForce);
        
    }

    public void PickWeapon(GameObject w)
    {
        w.transform.SetParent(weaponHold);
        w.transform.localPosition = Vector3.zero;
        w.transform.localEulerAngles = Vector3.zero;
        weaponGO = w;
        weapon = w.GetComponent<Weapon>();
    }
}
