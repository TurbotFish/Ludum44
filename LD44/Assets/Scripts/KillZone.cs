using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    public GameObject waterFX;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bonhomme"))
        {
            BonhommeController b = other.GetComponent<BonhommeController>();
            b.invincible = false;
            b.Kill(Vector3.zero, false, false, true);
            FlowManager.Instance.SendChatMessage("<b>" + b.playerInfo.playerName + " </b> drowned...");
            GameObject fx = Instantiate(waterFX, other.transform.position, Quaternion.identity) as GameObject;
            Destroy(fx, 4);
        }

        if (other.CompareTag("vehicle"))
        {
            VehicleController v = other.GetComponent<VehicleController>();
            v.player.ExitVehicle(false);
            v.Kill(Vector3.zero, false,true);
            GameObject fx = Instantiate(waterFX, other.transform.position, Quaternion.identity) as GameObject;
            fx.transform.localScale *= 1.5f;
            Destroy(fx, 4);
        }
    }
}
