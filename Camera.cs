using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private float sensX = 100f;
    [SerializeField] private float sensY = 100f;
    [SerializeField] Transform cam;
    [SerializeField] Transform orientation;
    float mouseX;
    float mouseY;
    float multi = 0.01f;
    float xRotation;
    float yRotation;
  
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        input();
        cam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
    void input()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        yRotation += mouseX * sensX * multi;
        xRotation -= mouseY * sensY * multi;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }
}
