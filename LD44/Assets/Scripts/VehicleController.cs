﻿using System.Collections;
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
    public GameObject explosion;
    public AudioSource klaxon;
    public RuntimeAnimatorController animController;


    void Start()
    {
        alive = true;
        rb = this.GetComponent<Rigidbody>();

    }

    private void Update()
    {
        if (CrowdController.firing && canKlaxon && playerIn)
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
            Debug.Log("exitKill");
            player.ExitVehicle();
        }
        alive = false;
        rb.constraints = RigidbodyConstraints.None;
        rb.AddForce(deathForce);
        if (shake)
        {
            CameraShake.shakeDuration = 0.5f;
        }

        GameObject explo = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
        Destroy(explo, 4);

        Destroy(this.gameObject, 5);
        this.gameObject.tag = "Untagged";

    }

    private IEnumerator Klaxon(float t)
    {
        yield return new WaitForSeconds(t);
        klaxon.Play();
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bonhomme") && !playerIn && horizontalVelocity.magnitude<speedKillThreshold)
        {
            BonhommeController b = other.GetComponent<BonhommeController>();
            if (!b.noPick)
            {
                playerIn = true;
                player = b;
                player.EnterVehicle(this);
                other.transform.SetParent(playerPlace);
                other.transform.localPosition = Vector3.zero;
                other.transform.localEulerAngles = Vector3.zero;
            }

        }
    }

    private void OnCollisionEnter(Collision col)
    {

        if (col.collider.CompareTag("bonhomme") && horizontalVelocity.magnitude > speedKillThreshold)
        {
            BonhommeController b = col.gameObject.GetComponent<BonhommeController>();
            if (!b.invincible)
            {
                b.Kill(rb.velocity, true, true);
                if (player != null)
                {
                    StartCoroutine(player.NoPick(1));
                }
                PlayerInfo victim = b.playerInfo;
                if (player != null)
                {

                    FlowManager.Instance.SendChatMessage(player.playerInfo.playerName + " ran " + b.playerInfo.playerName + " over");
                    FlowManager.Instance.RemovePlayer(victim, true);
                    player.playerInfo.kills++;
                    player.playerInfo.totalKills++;
                    FlowManager.Instance.CheckKillStreak(player.playerInfo);

                }
                else
                {
                    FlowManager.Instance.SendChatMessage(victim.name + " killed themselves...");
                    FlowManager.Instance.RemovePlayer(victim, false);
                }

            }

        }
    }

    public void ResetVehicle()
    {
        playerIn = false;
        player = null;
    }
}
