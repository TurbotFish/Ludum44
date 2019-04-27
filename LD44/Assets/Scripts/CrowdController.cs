using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdController : MonoBehaviour
{
    public static float moveSpeed = 8f;
    public static float rotationSpeed = 4f;
    //public float moveAccelerationSpeed;

    public static float rotationInput;
    public static float moveInput;

    public static bool firing;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Action"))
        {
            firing = true;
        }
        if (Input.GetButtonUp("Action"))
        {
            firing = false;
        }

        rotationInput = Input.GetAxis("Horizontal");
        moveInput = Input.GetAxis("Vertical");
    }
}
