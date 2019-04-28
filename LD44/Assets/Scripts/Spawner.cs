using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public enum SpawnType {Weapon, Vehicle}
    public SpawnType type;
    private PlayerInfoDB db;

    // Start is called before the first frame update
    public void Spawn()
    {
        db = FlowManager.Instance.db;
        if (type == SpawnType.Weapon)
        {
            int i = Random.Range(0, db.availableWeapons.Count);
            GameObject go = Instantiate(db.availableWeapons[i], transform.position, transform.rotation) as GameObject;
            FlowManager.Instance.mapItems.Add(go);
        }

        if (type == SpawnType.Vehicle)
        {
            int i = Random.Range(0, db.availableVehicles.Count);
            GameObject go = Instantiate(db.availableVehicles[i], transform.position, transform.rotation) as GameObject;
            FlowManager.Instance.mapItems.Add(go);

        }
    }


}
