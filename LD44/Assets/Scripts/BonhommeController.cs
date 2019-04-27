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

    bool inVehicle;

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
        if (alive && !inVehicle)
        {
            Vector3 movement = this.transform.forward * CrowdController.moveInput * CrowdController.moveSpeed;
            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
            this.transform.Rotate(new Vector3(0, CrowdController.rotationInput * CrowdController.rotationSpeed, 0));

        }
    }

    public void Kill(Vector3 deathForce, bool shake)
    {
        alive = false;
        if (weaponGO!=null)
        {
            DropWeapon();
        }
        rb.constraints = RigidbodyConstraints.None;
        rb.AddForce(deathForce);
        if (shake)
        {
            CameraShake.shakeDuration = 0.25f;
        }
        Destroy(this.gameObject, 5);
    }

    public void PickWeapon(GameObject w)
    {
        w.transform.SetParent(weaponHold);
        w.transform.localPosition = Vector3.zero;
        w.transform.localEulerAngles = Vector3.zero;
        weaponGO = w;
        weapon = w.GetComponent<Weapon>();
    }

    public void DropWeapon()
    {
        weaponGO.transform.SetParent(null);
        weapon.pickable = true;
        weaponGO.transform.position = new Vector3(weaponGO.transform.position.x, 1, weaponGO.transform.position.z);
        weaponGO = null;
        weapon = null;
    }

    public void EnterVehicle()
    {
        DropWeapon();
        inVehicle = true;
    }

    public void ExitVehicle()
    {
        inVehicle = true;
    }
}
