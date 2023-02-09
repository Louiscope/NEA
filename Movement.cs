using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    #region vars
    [SerializeField] Transform orientation;

    [Header("Movement")]
    public float movespd = 24f;
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

    [Header("WallRunning")]
    [SerializeField] private float wallrungrav;
    [SerializeField] private float wallrunjumpforce;

    [SerializeField] float wallDist = .5f;
    [SerializeField] float minjump = 1.5f;

    bool wallLeft = false;
    bool wallRight = false;

    RaycastHit leftwallhit;
    RaycastHit rightwallhit;

    Rigidbody rb;
    RaycastHit slope;

    #endregion
    #region funcs

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

    #region wallrun
    bool canrun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minjump);
    }

    void wallCheck()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftwallhit, wallDist);
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightwallhit, wallDist);
    }

    void wallrun() 
    {
        rb.useGravity = false;
        rb.AddForce(Vector3.down * wallrungrav, ForceMode.Force);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (wallLeft)
            {
                Vector3 wallrunjumpdirect = transform.up + leftwallhit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallrunjumpdirect * wallrunjumpforce * 100, ForceMode.Force);
            }
            else if (wallRight)
            {
                Vector3 wallrunjumpdirect = transform.up + rightwallhit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallrunjumpdirect * wallrunjumpforce * 100, ForceMode.Force);
            }
        }
    }

    void stopwallrun()
    {
        rb.useGravity = true;
    }

    #endregion

    #region start/update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);
        wallCheck();
        input();
        dragcontrol();
        dash();

        if (canrun())
        {
            if (wallLeft | wallRight)
            {
                wallrun();
            }
            else
            {
                stopwallrun();
            }
        }
        else
        {
            stopwallrun();
        }
        if ((grounded) | (wallLeft | wallRight))
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
    #endregion

    #region movement
    void jumping()
    {
        if (grounded | dj)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            rb.AddForce(movedirection * jumpForce, ForceMode.Impulse);
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
    #endregion

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

    
    #endregion
}
