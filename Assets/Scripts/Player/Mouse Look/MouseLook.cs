using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    private const CursorLockMode locked = CursorLockMode.Locked;

    [SerializeField] private float mouseSensitivity = 100.0f;
    [SerializeField] private Transform headTransform;
    [SerializeField] private float lerpSpeed;
    
    public Quaternion localRotation;
    private Quaternion smoothRot;
    public float clampAngle = 80.0f;

    public float rotY = 0.0f; // rotation around the up/y axis
    public float rotX = 0.0f; // rotation around the right/x axis

    void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = MathHelpers.ExpDecay(rotY, rot.y, 50, Time.deltaTime);
        rotX = MathHelpers.ExpDecay(rotX, rot.x, 50, Time.deltaTime);
        Cursor.visible = false;
        Cursor.lockState = locked;
    }

    private void Update()   
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        rotY += mouseX * mouseSensitivity;
        rotX += mouseY * mouseSensitivity;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        localRotation = Quaternion.Euler(rotX, rotY, 0);
        smoothRot = MathHelpers.ExpDecay(smoothRot, localRotation, lerpSpeed, Time.deltaTime);
        
    }
    
    private void LateUpdate(){
        
        transform.position = headTransform.position;
        transform.localRotation = smoothRot;
    }
}
