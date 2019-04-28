using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public enum BulletType { Small, Medium, Large, RPG, Grenade}
    public BulletType type;
    public float bulletSpeed;
    public float bulletPower;
    public bool goThrough;
    public GameObject explosion;
    bool exploded;
    public PlayerInfo owner;

    void Start()
    {
        if (type == BulletType.Grenade)
        {
            StartCoroutine(DestroyBullet(4));
        }
    }


    void FixedUpdate()
    {
        if (type != BulletType.Grenade)
        {
            transform.Translate(0, 0, bulletSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.CompareTag("bonhomme") && type != BulletType.Grenade)
        {
            col.collider.GetComponent<BonhommeController>().Kill(transform.forward * bulletPower, true, true);
            PlayerInfo victim = col.collider.GetComponent<PlayerInfo>();
            if (owner == victim)
            {
                FlowManager.Instance.SendChatMessage(owner.name + " killed themselves...");
                FlowManager.Instance.RemovePlayer(victim, false);
            }
            else
            {
                FlowManager.Instance.SendChatMessage(owner.name + " blew " + col.collider.GetComponent<PlayerInfo>().name + " up");
                FlowManager.Instance.RemovePlayer(victim, true);
                owner.kills++;
                owner.totalKills++;
            }

            if (!goThrough)
            {
                StartCoroutine(DestroyBullet(0));
            }

        }
        else if (col.collider.CompareTag("vehicle") && type != BulletType.Grenade)
        {
            VehicleController v = col.gameObject.GetComponent<VehicleController>();
            v.life -= bulletPower;
            if (v.life < 0)
            {
                v.Kill(transform.forward * bulletPower, true);
            }
            StartCoroutine(DestroyBullet(0));
        }
        else if (type != BulletType.Grenade)
        {
            StartCoroutine(DestroyBullet(0));
        }
    }

    private IEnumerator DestroyBullet(float t)
    {
        yield return new WaitForSeconds(t);
        if ((type == BulletType.RPG || type == BulletType.Grenade) && !exploded)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            exploded = true;
        }
        Destroy(this.gameObject);
    }


}
