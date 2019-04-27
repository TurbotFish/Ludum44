using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public float raycastHeight;
    public  float minX,maxX, minY, maxY;
    public int numberOfPlayersToInstantiate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            InstantiateCrowd(numberOfPlayersToInstantiate);
        }
    }

    void InstantiateCrowd(int numberOfPlayers)
    {
        int i = 0;
        int sec = 0;
        while (i<numberOfPlayers && sec < 500)
        {
            float x = Random.Range(minX, maxX);
            float y = Random.Range(minY, maxY);


            RaycastHit hit;
            if (Physics.Raycast(new Vector3(x,raycastHeight,y), new Vector3(0,-1,0), out hit, raycastHeight*2))
            {
                if (hit.collider.CompareTag("ground"))
                {
                    GameObject p = Instantiate(playerPrefab, hit.point + Vector3.up * 0.5f, Quaternion.identity) as GameObject;
                    p.transform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
                    i++;
                }
            }

            sec++;

        }
    }

}
