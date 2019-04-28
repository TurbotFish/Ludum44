using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public enum SpawnType {Weapon, Vehicle}
    public SpawnType type;

    // Start is called before the first frame update
    public void Spawn()
    {
        if (type == SpawnType.Weapon)
        {
            int i = Random.Range(0, FlowManager.Instance.availableWeapons.Count);
            GameObject go = Instantiate(FlowManager.Instance.availableWeapons[i], transform.position, transform.rotation) as GameObject;
            FlowManager.Instance.mapItems.Add(go);
        }

        if (type == SpawnType.Vehicle)
        {
            int i = Random.Range(0, FlowManager.Instance.availableVehicles.Count);
            GameObject go = Instantiate(FlowManager.Instance.availableVehicles[i], transform.position, transform.rotation) as GameObject;
            FlowManager.Instance.mapItems.Add(go);

        }
    }


}
