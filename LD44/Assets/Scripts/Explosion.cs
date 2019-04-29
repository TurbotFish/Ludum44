using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float explosionForce;
    public float explosionDamage;
    bool exploded;
    public PlayerInfo owner;

    // Start is called before the first frame update
    void Start()
    {
        ExplosionDamage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ExplosionDamage()
    {
        SphereCollider col = GetComponent<SphereCollider>();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position+col.center, col.radius);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("bonhomme"))
            {
                BonhommeController bc = hitCollider.GetComponent<BonhommeController>();
                bc.Kill((bc.transform.position - (transform.position + col.center)).normalized * explosionForce, true, true);
                PlayerInfo victim = hitCollider.GetComponent<PlayerInfo>();
                if (owner == victim)
                {
                    FlowManager.Instance.SendChatMessage(owner.playerName + " killed themselves...");
                    FlowManager.Instance.RemovePlayer(victim, false);
                }
                else if (owner != null)
                {
                    FlowManager.Instance.SendChatMessage(owner.playerName + " blew " + bc.playerInfo.playerName + " up");
                    FlowManager.Instance.RemovePlayer(victim, true);
                    owner.kills++;
                    owner.totalKills++;
                    FlowManager.Instance.CheckKillStreak(owner);
                }
                else
                {
                    FlowManager.Instance.RemovePlayer(victim, false);
                }

            }
            else if (hitCollider.CompareTag("vehicle"))
            {
                VehicleController vc = hitCollider.GetComponent<VehicleController>();
                vc.life -= explosionDamage;
                if (vc.life < 0)
                {
                    vc.Kill((vc.transform.position - (transform.position + col.center)) * explosionForce, true);
                }
            }
        }
       
        Destroy(this.gameObject, 5);
    }
}
