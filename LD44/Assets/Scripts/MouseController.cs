using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{

    public Animator camAnimator;
    public float scrollSpeed;
    public float zoomSpeed;
    public float maxZoom;
    public Transform cam;
    public float minX, maxX, minY, maxY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 300.0f))
            {
                if (hit.collider.CompareTag("bonhomme"))
                {
                    camAnimator.SetBool("select", true);
                }
                else
                {
                    camAnimator.SetBool("select", false);
                }
            }
        }



        //RTS CAM:

        cam.localPosition = new Vector3(cam.localPosition.x, cam.localPosition.y, Mathf.Clamp(cam.localPosition.z + Input.mouseScrollDelta.y*zoomSpeed,0, maxZoom));


        float zoomFactor = cam.localPosition.z / maxZoom;

        if (Input.mousePosition.y >= Screen.height * 0.95f && cam.localPosition.y < maxY*zoomFactor)
        {
            cam.Translate(Vector3.forward * Time.deltaTime * scrollSpeed, Space.World);
        }
        if (Input.mousePosition.y <= Screen.height * 0.05f && cam.localPosition.y > minY * zoomFactor)
        {
            cam.Translate(Vector3.back * Time.deltaTime * scrollSpeed, Space.World);
        }
        if(Input.mousePosition.x >= Screen.width * 0.95f && cam.localPosition.x < maxX * zoomFactor)
        {
            cam.Translate(Vector3.right * Time.deltaTime * scrollSpeed, Space.World);
        }
        if(Input.mousePosition.x <= Screen.width * 0.05f && cam.localPosition.x > minX * zoomFactor)
        {
            cam.Translate(Vector3.left * Time.deltaTime * scrollSpeed, Space.World);
        }
    }
}
