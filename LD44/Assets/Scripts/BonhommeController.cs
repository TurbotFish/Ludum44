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
    [HideInInspector]
    public GameObject weaponGO;
    [HideInInspector]
    public Weapon weapon;
    public bool inZone;
    public float zoneTimer;
    public Transform head;
    public bool inVehicle;
    public ParticleSystem impactFX;

    public RuntimeAnimatorController defaultAnimController;

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
            Destroy(this.gameObject, 0.1f);
            this.gameObject.tag = "Untagged";

            PlayerInfo pi = GetComponent<PlayerInfo>();
            FlowManager.Instance.RemovePlayer(pi, kill);
        }

    }

    public void PickWeapon(GameObject w)
    {
        if (!noPick)
        {
            Debug.Log("pick");

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
        Debug.Log("drop");
        if (weaponGO != null)
        {
            weaponGO.transform.SetParent(null);
            weapon.pickable = true;
            weaponGO.transform.position = new Vector3(weaponGO.transform.position.x, 2, weaponGO.transform.position.z);
            weaponGO = null;
            weapon = null;
        }

    }

    public void EnterVehicle(VehicleController vhc)
    {
        Debug.Log("enter");
        if (!noPick)
        {
            StartCoroutine(NoPick(1));
            DropWeapon();
            GetComponent<CapsuleCollider>().enabled = false;
            anim.runtimeAnimatorController = vhc.animController;
            rb.isKinematic = true;
            inVehicle = true;
        }

    }

    public void ExitVehicle()
    {
        Debug.Log("exit");
        StartCoroutine(GodFrames(2f));
        transform.SetParent(null);
        inVehicle = false;
        GetComponent<CapsuleCollider>().enabled = true;
        rb.isKinematic = false;
        anim.runtimeAnimatorController = defaultAnimController;

    }

    public IEnumerator GodFrames(float t)
    {
        invincible = true;
        GetComponent<CapsuleCollider>().enabled = false;
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        yield return new WaitForSeconds(t);
        GetComponent<CapsuleCollider>().enabled = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        invincible = false;
    }

    public IEnumerator NoPick(float t)
    {
        noPick = true;
        yield return new WaitForSeconds(t);
        noPick = false;
    }

    public IEnumerator InZone(float t)
    {
        yield return new WaitForSeconds(t);
        if (inZone)
        {
            invincible = false;
            if (this != null)
                Kill(Vector3.zero, false, false);
        }
    }
}
