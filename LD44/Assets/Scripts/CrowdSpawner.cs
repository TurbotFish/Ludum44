using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public float raycastHeight;
    public  float minX,maxX, minY, maxY;
    public int numberOfPlayers;

    public GameObject plane, fakeShadow;
    //public FlowManager flowManager;



    public IEnumerator InstantiateCrowd()
    {
        plane.GetComponent<Animation>().Play();
        yield return new WaitForSeconds(1.9f);
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
                    if (i==0)
                    {
                        GameObject p = Instantiate(FlowManager.Instance.playerSave.gameObject, hit.point + Vector3.up * 100f, Quaternion.identity) as GameObject;
                        Destroy(FlowManager.Instance.playerSave.gameObject);
                        p.transform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
                        p.GetComponent<Rigidbody>().velocity += new Vector3(0, -100f, 0);
                        p.GetComponent<Rigidbody>().isKinematic = false;
                        //p.GetComponent<PlayerInfo>().playerName = FlowManager.Instance.names[Random.Range(0, FlowManager.Instance.names.Count - 1)];
                        p.GetComponent<PlayerInfo>().Reset();
                        FlowManager.Instance.AddPlayer(p.GetComponent<PlayerInfo>());

                        p.GetComponent<BonhommeController>().impactFX.Play();
                    }
                    else
                    {
                        GameObject p = Instantiate(playerPrefab, hit.point + Vector3.up * 100f, Quaternion.identity) as GameObject;
                        p.transform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
                        p.GetComponent<Rigidbody>().velocity += new Vector3(0, -100f, 0);

                        Material skin = FlowManager.Instance.availableSkins[Random.Range(0, FlowManager.Instance.availableSkins.Count)].GetComponentInChildren<MeshRenderer>().sharedMaterial;
                        MeshRenderer[] meshes = p.GetComponentsInChildren<MeshRenderer>();
                        foreach (MeshRenderer mesh in meshes)
                        {
                            mesh.material = skin;
                        }
                        p.GetComponent<PlayerInfo>().playerName = FlowManager.Instance.names[Random.Range(0, FlowManager.Instance.names.Count - 1)];
                        p.GetComponent<PlayerInfo>().Reset();

                        FlowManager.Instance.AddPlayer(p.GetComponent<PlayerInfo>());

                        p.GetComponent<BonhommeController>().impactFX.Play();

                    }

                    //GameObject s = Instantiate(fakeShadow, hit.point + Vector3.up * 0.1f, Quaternion.Euler(90,0,0)) as GameObject;
                    //Destroy(s, 3);
                    yield return new WaitForSeconds(0.002f);
                    i++;
                }

                StartCoroutine(ShakeImpact());
            }

            sec++;

        }
    }

    private IEnumerator ShakeImpact ()
    {
        yield return new WaitForSeconds(1f);
        CameraShake.shakeDuration = 0.01f;

    }

}
