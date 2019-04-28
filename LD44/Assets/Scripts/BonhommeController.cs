using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonhommeController : MonoBehaviour
{
    Rigidbody rb;
    public bool alive;
    public bool invincible;
    public bool noPick;
    public Animator anim;
    public PlayerInfo playerInfo;
    public Transform weaponHold;
    public Transform weaponFire;
    GameObject weaponGO;
    Weapon weapon;

    bool inVehicle;

   // public FlowManager flowManager;

    void Start()
    {
        alive = true;
        rb = this.GetComponent<Rigidbody>();

    }

    private void Update()
    {
        if (CrowdController.firing)
        {
            anim.SetTrigger("Shoot");

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

            if (CrowdController.moveInput >= 0.1f || CrowdController.moveInput <= -0.1f)
            {
                anim.SetBool("Running", true);
            }
            else
            {
                anim.SetBool("Running", false);
            }
        }
    }

    public void Kill(Vector3 deathForce, bool shake, bool kill)
    {
        if (!invincible)
        {
            gameObject.layer = LayerMask.NameToLayer("NoCollision");
            alive = false;
            DropWeapon();
            rb.constraints = RigidbodyConstraints.None;
            rb.AddForce(deathForce*100);
            if (shake)
            {
                CameraShake.shakeDuration = 0.25f;
            }
            Destroy(this.gameObject, 3);
            this.gameObject.tag = "Untagged";

            PlayerInfo pi = GetComponent<PlayerInfo>();
            FlowManager.Instance.RemovePlayer(pi, kill);
        }

    }

    public void PickWeapon(GameObject w)
    {
        if (!noPick)
        {
            DropWeapon();
            w.transform.SetParent(weaponHold);
            w.transform.localPosition = Vector3.zero;
            w.transform.localEulerAngles = Vector3.zero;
            weaponGO = w;
            weapon = w.GetComponent<Weapon>();
            anim.runtimeAnimatorController = weapon.playerAnim;
            weapon.pickable = false;
            weapon.owner = playerInfo;
            StartCoroutine(NoPick(1));
        }

    }

    public void DropWeapon()
    {
        if (weaponGO != null)
        {
            weaponGO.transform.SetParent(null);
            weapon.pickable = true;
            weaponGO.transform.position = new Vector3(weaponGO.transform.position.x, 2, weaponGO.transform.position.z);
            weaponGO = null;
            weapon = null;
        }

    }

    public void EnterVehicle()
    {
        if (!noPick)
        {
            DropWeapon();
            GetComponent<CapsuleCollider>().enabled = false;
            rb.isKinematic = true;
            inVehicle = true;
        }

    }

    public void ExitVehicle()
    {
        StartCoroutine(GodFrames(2f));
        inVehicle = false;
        GetComponent<CapsuleCollider>().enabled = true;
        rb.isKinematic = false;

    }

    public IEnumerator GodFrames(float t)
    {
        invincible = true;
        yield return new WaitForSeconds(t);
        invincible = false;
    }

    public IEnumerator NoPick(float t)
    {
        noPick = true;
        yield return new WaitForSeconds(t);
        noPick = false;
    }
}
