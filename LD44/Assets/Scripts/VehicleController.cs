using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    public float acceleration, rotationSpeed, maxSpeed, speedKillThreshold;
    Rigidbody rb;
    public bool alive;
    public bool playerIn;
    public Transform playerPlace;
    public BonhommeController player;
    bool canKlaxon = true;
    public float life = 1;
    public Vector3 horizontalVelocity;

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
        horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if (alive && playerIn)
        {
            Vector3 movement = this.transform.forward * CrowdController.moveInput * acceleration;
            rb.AddForce(movement);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
            this.transform.Rotate(new Vector3(0, CrowdController.rotationInput * rotationSpeed * Mathf.Clamp01(horizontalVelocity.magnitude/ speedKillThreshold), 0));

        }
    }

    public void Kill(Vector3 deathForce, bool shake)
    {
        if (player != null)
        {
            player.ExitVehicle();
        }
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



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bonhomme") && !playerIn && horizontalVelocity.magnitude<speedKillThreshold)
        {
            playerIn = true;
            player = other.GetComponent<BonhommeController>();
            player.EnterVehicle();
            other.transform.SetParent(playerPlace);
            other.transform.localPosition = Vector3.zero;
            other.transform.localEulerAngles = Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision col)
    {

        if (col.collider.CompareTag("bonhomme") && horizontalVelocity.magnitude > speedKillThreshold)
        {
            col.gameObject.GetComponent<BonhommeController>().Kill(rb.velocity, true);
            StartCoroutine(player.NoPick(1));

        }
    }

    public void ResetVehicle()
    {
        playerIn = false;
        player = null;
    }
}
