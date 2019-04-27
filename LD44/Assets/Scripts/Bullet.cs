using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public enum BulletType { Small, Medium, Large, RPG, Grenade}
    public BulletType type;
    public float bulletSpeed;
    public float bulletPower;


    void Start()
    {
        
    }


    void FixedUpdate()
    {
        this.transform.Translate(this.transform.forward * bulletSpeed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.CompareTag("bonhomme"))
        {
            col.collider.GetComponent<BonhommeController>().Kill(this.transform.forward * bulletPower);
        }
        else
        {
            DestroyBullet();
        }
    }

    void DestroyBullet()
    {
        Destroy(this.gameObject);
    }
}
