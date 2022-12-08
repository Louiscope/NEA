using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] Transform orientation;

    [Header("Movement")]
    public float movespd = 6f;
    public float movementmultiplier = 10f;
    float verticalmov;
    float horizontalmov;
    Vector3 movedirection;

    [Header("Dashing")]
    [SerializeField] float dashspd = 80f;
    [SerializeField] float airdashspd = 45f;
    [SerializeField] float cd = 1f;
    [SerializeField] float cooldown = 0.15f;
    [SerializeField] float dashCounter;

    [Header("Drag")]
    public float grnddrag = 6f;
    public float airdrag = 2f;
    public float dashdrag = 6f;
    [SerializeField] float airmultiplier = 0.4f;

    [Header("Jumping")]
    [SerializeField] LayerMask groundMask;
    [SerializeField] Transform groundCheck;
    bool grounded;
    float playerheight = 2f;
    float groundDist = 0.4f;
    public float jumpForce = 20f;
    bool dj = true;

    [Header("Binds")]
    [SerializeField] KeyCode dashkey = KeyCode.LeftShift;

    Rigidbody rb;
    RaycastHit slope;

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slope, playerheight / 2 + 0.5f))
        {
            if (slope.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);
        input();
        dragcontrol();
        dash();
        if (grounded)
        {
            dj = true;
            dashCounter = 3;
        }

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            jumping();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && dj)
        {
            jumping();
            dj = false;
        }
    }

    void jumping()
    {
        if (grounded | dj)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
        
    }
    void dash()
    {
        if (Input.GetKey(dashkey) && (dashCounter != 0) && Time.time > cooldown)
        {
            if (grounded)
            {
                rb.AddForce(movedirection * dashspd, ForceMode.Impulse);
            }
            else
            {
                rb.AddForce(movedirection * airdashspd, ForceMode.Impulse);
            }
            dashCounter -= 1;
            cooldown = Time.time + cd;
        }
    }

    void dragcontrol()
    {
        if (grounded)
        {
            rb.drag = grnddrag;
        }
        else
        {
            rb.drag = airdrag;
        }
    }

    public void input()
    {
        horizontalmov = Input.GetAxisRaw("Horizontal");
        verticalmov = Input.GetAxisRaw("Vertical");

        movedirection = orientation.forward * verticalmov + orientation.right * horizontalmov;
    }

    private void FixedUpdate()
    {
        moveplayer();
    }

    void moveplayer()
    {
        if (grounded)
        {
            rb.AddForce(movedirection.normalized * movespd * movementmultiplier, ForceMode.Acceleration);
        }
        else if (!grounded)
        {
            rb.AddForce(movedirection.normalized * movespd * movementmultiplier * airmultiplier, ForceMode.Acceleration);
        }
    }
}
