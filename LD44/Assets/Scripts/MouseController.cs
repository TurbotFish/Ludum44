using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 300.0f))
            {
                //suppose i have two objects here named obj1 and obj2.. how do i select obj1 to be transformed 
                if (hit.collider.CompareTag("bonhomme"))
                {
                    Debug.Log("HELLO");
                }
            }
        }
    }
}
