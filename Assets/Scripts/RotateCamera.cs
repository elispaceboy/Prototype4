using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public float rotateSpeed;
    private float rotateSpeedMultiplier = 4.0f;
    private float startRotateSpeed;
    

    // Start is called before the first frame update
    void Start()
    {
        startRotateSpeed = rotateSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //Increase camera speed when player shifts
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            rotateSpeed = startRotateSpeed * rotateSpeedMultiplier;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            rotateSpeed = startRotateSpeed;
        }
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, horizontalInput * rotateSpeed * Time.deltaTime);
    }
}
