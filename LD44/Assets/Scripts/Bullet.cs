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
        transform.Translate(0,0,bulletSpeed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.CompareTag("bonhomme"))
        {
            col.collider.GetComponent<BonhommeController>().Kill(transform.forward * bulletPower, true);
            DestroyBullet();

        }
        else if (col.collider.CompareTag("vehicle"))
        {
            VehicleController v = col.gameObject.GetComponent<VehicleController>();
            v.life -= bulletPower;
            if (v.life < 0)
            {
                v.Kill(transform.forward * bulletPower, true);
            }
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
