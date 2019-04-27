using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    public float moveSpeed, rotationSpeed;
    Rigidbody rb;
    public bool alive;
    public bool playerIn;
    public Transform playerPlace;
    public BonhommeController player;
    bool canKlaxon = true;
    public float life = 1;

    void Start()
    {
        alive = true;
        rb = this.GetComponent<Rigidbody>();

    }

    private void Update()
    {
        if (CrowdController.firing && canKlaxon)
        {
            canKlaxon = false;
            StartCoroutine(Klaxon(Random.Range(0f, 0.5f)));
        }
        else if (!CrowdController.firing)
        {
            canKlaxon = true;
        }

    }

    void FixedUpdate()
    {
        if (alive && playerIn)
        {
            Vector3 movement = this.transform.forward * CrowdController.moveInput * moveSpeed;
            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
            this.transform.Rotate(new Vector3(0, CrowdController.rotationInput * rotationSpeed, 0));

        }
    }

    public void Kill(Vector3 deathForce, bool shake)
    {
        player.ExitVehicle();
        alive = false;
        rb.constraints = RigidbodyConstraints.None;
        rb.AddForce(deathForce);
        if (shake)
        {
            CameraShake.shakeDuration = 0.5f;
        }
        Destroy(this.gameObject, 5);
    }

    private IEnumerator Klaxon(float t)
    {
        yield return new WaitForSeconds(t);
        Debug.Log("klaxon");
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.CompareTag("bullet"))
        {
            Bullet b = col.gameObject.GetComponent<Bullet>();
            life -= b.bulletPower;
            if (life<0)
            {
                Kill(b.transform.forward * b.bulletPower, true);
            }
        }
        else if (col.collider.CompareTag("bonhomme") && playerIn)
        {
            col.gameObject.GetComponent<BonhommeController>().Kill(rb.velocity, true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bonhomme") && !playerIn)
        {
            playerIn = true;
            player = other.GetComponent<BonhommeController>();
            player.EnterVehicle();
            other.transform.SetParent(playerPlace);
            other.transform.localPosition = Vector3.zero;
            other.transform.localEulerAngles = Vector3.zero;
        }
    }
}
