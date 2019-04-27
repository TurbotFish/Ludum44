using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float explosionForce;
    public float explosionDamage;
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
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].CompareTag("bonhomme"))
            {
                BonhommeController bc = hitColliders[i].GetComponent<BonhommeController>();
                bc.Kill((bc.transform.position - (transform.position + col.center)) * explosionForce, true);
            }
            else if (hitColliders[i].CompareTag("vehicle"))
            {
                VehicleController vc = hitColliders[i].GetComponent<VehicleController>();
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
