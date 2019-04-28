using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public enum SpawnType {Weapon, Vehicle}
    public SpawnType type;
    public PlayerInfoDB db;

    // Start is called before the first frame update
    void Start()
    {
        if (type == SpawnType.Weapon)
        {
            int i = Random.Range(0, db.availableWeapons.Count - 1);
            Instantiate(db.availableWeapons[i], transform.position, transform.rotation);
        }

        if (type == SpawnType.Vehicle)
        {
            int i = Random.Range(0, db.availableVehicles.Count - 1);
            Instantiate(db.availableVehicles[i], transform.position, transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
