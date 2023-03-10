using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;
    Camera cam;
    float mouseX;
    float mouseY;
    float multi = 0.01f;
    float xRotation;
    float yRotation;

    public float movespd = 6f;
    public float drag = 6f;
    public float movementmultiplier = 10f;
    float verticalmov;
    float horizontalmov;
    Vector3 movedirection;

    Rigidbody rb;
    private void Start()
    {
        cam = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }


    private void Update()
    {
        input();
        dragcontrol();

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void dragcontrol()
    {
        rb.drag = drag;
    }

    public void input()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");
        yRotation += mouseX * sensX * multi;
        xRotation -= mouseY * sensY * multi;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        horizontalmov = Input.GetAxisRaw("Horizontal");
        verticalmov = Input.GetAxisRaw("Vertical");

        movedirection = transform.forward * verticalmov + transform.right * horizontalmov;

    }

    private void FixedUpdate()
    {
        moveplayer();
    }

    void moveplayer()
    {
        rb.AddForce(movedirection.normalized * movespd * movementmultiplier, ForceMode.Acceleration);
    }
}
