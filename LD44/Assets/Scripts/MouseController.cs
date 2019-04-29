using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{

    public float scrollSpeed;
    public float zoomSpeed;
    public float maxZoom;
    public float startZoom;
    public Transform cam;
    public float minX, maxX, minY, maxY;

    public void StartZoom()
    {
        cam.Translate(Vector3.forward * startZoom, Space.World);
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

                if (!FlowManager.inMenu)
                {
                    if (hit.collider.CompareTag("bonhomme"))
                    {
                        Debug.Log("bonhomme selected");
                    }
                    else
                    {
                        Debug.Log("bonhomme unselected");
                    }
                }

            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 300.0f))
            {

                if (FlowManager.inMenu && !FlowManager.Instance.inLootbox)
                {
                    if (hit.collider.CompareTag("goButton"))
                    {
                        ButtonScript button = hit.collider.GetComponentInParent<ButtonScript>();
                        if (button.clickable)
                        {
                            button.OnClick();
                            FlowManager.Instance.ClickTransition(button.transition);

                        }


                    }
                }
            }
        }


        if (!FlowManager.inMenu)
        {
            //RTS CAM:

            cam.localPosition = new Vector3(cam.localPosition.x, cam.localPosition.y, Mathf.Clamp(cam.localPosition.z + Input.mouseScrollDelta.y * zoomSpeed, 0, maxZoom));


            float zoomFactor = cam.localPosition.z / maxZoom;

            if (Input.mousePosition.y >= Screen.height * 0.95f && cam.localPosition.y < maxY * zoomFactor)
            {
                cam.Translate(Vector3.forward * Time.deltaTime * scrollSpeed, Space.World);
            }
            if (Input.mousePosition.y <= Screen.height * 0.05f && cam.localPosition.y > minY * zoomFactor)
            {
                cam.Translate(Vector3.back * Time.deltaTime * scrollSpeed, Space.World);
            }
            if (Input.mousePosition.x >= Screen.width * 0.95f && cam.localPosition.x < maxX * zoomFactor)
            {
                cam.Translate(Vector3.right * Time.deltaTime * scrollSpeed, Space.World);
            }
            if (Input.mousePosition.x <= Screen.width * 0.05f && cam.localPosition.x > minX * zoomFactor)
            {
                cam.Translate(Vector3.left * Time.deltaTime * scrollSpeed, Space.World);
            }
        }
    }
}
