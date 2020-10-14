using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    public float sensitivity;
    public Transform playerBody;
    private float xRotation = 0f;
    public float rotationSpeed = 0f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        UpdateCameraOrientation(mouseX, mouseY);
    }

    void UpdateCameraOrientation(float mouseX, float mouseY)
    {

        xRotation -= (mouseY * sensitivity * Time.deltaTime); //invert
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        //Euler angle rotates on the global scale
        playerBody.Rotate(Vector3.up * mouseX * sensitivity * Time.deltaTime);

//        transform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(xRotation, 0.0f, 0.0f), rotationSpeed * Time.deltaTime);
    }
}
  