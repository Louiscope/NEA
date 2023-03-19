using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// THIS IS TUTORIAL CODE -- THIS IS NOT MY CODE APART FROM DOUBLEJUMP
public class Movement : MonoBehaviour
{
    // Used for finding direction of player
    [SerializeField] Transform orientation;

    // Contains speed, multiplier and directional values
    [Header("Movement")]
    public float movespd = 24f;
    public float movementmultiplier = 10f;
    float verticalmov;
    float horizontalmov;
    Vector3 movedirection;

    // Contains dash information (Cooldowns, force, counters)
    [Header("Dashing")]
    [SerializeField] float dashspd = 160f;
    [SerializeField] float airdashspd = 90f;
    [SerializeField] float cd = 1f;
    [SerializeField] float cooldown = 0.15f;
    [SerializeField] float lastDash;
    [SerializeField] float dashCounter;

    // Contains drag values for the rigidbody physics to interact with
    [Header("Drag")]
    public float grnddrag = 6f;
    public float airdrag = 2f;
    public float dashdrag = 6f;
    [SerializeField] float airmultiplier = 0.4f;

    // Contains jump information (GroundChecker, jump height, double jump)
    [Header("Jumping")]
    [SerializeField] LayerMask groundMask;
    [SerializeField] Transform groundCheck;
    bool grounded;
    float playerheight = 2f;
    float groundDist = 0.4f;
    public float jumpForce = 20f;
    public float JumpForwardForce;
    bool dj = true;

    [Header("Binds")]
    [SerializeField] KeyCode dashkey = KeyCode.LeftShift;

    // Contains wallrunning info (Location of wall relative to player etc.)
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

    // Determines slope function
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

    bool canrun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minjump);
    }

    // Checks location of wall and returns bool if it is wallDist away from player
    void wallCheck()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftwallhit, wallDist);
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightwallhit, wallDist);
    }

    // Chenges player state for wallrunning
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

    // Collects rigidbody from player object -- Prevents player object from spinning
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Checkers for walls and ground -- Controls double jump, dash and wallrun operations actively
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
   
    // Controls jumping
    void jumping()
    {
        if (grounded | dj)
        {
            if (Time.time - lastDash < 1)
            {
                JumpForwardForce = 20f;
            }
            else
            {
                JumpForwardForce = 10f;
            }
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            rb.AddForce(movedirection * JumpForwardForce, ForceMode.Impulse);
        }
        
    }
    // Controls dashing
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
            lastDash = Time.time;
        }
    }

    // Fixed update used so that player moves in time with physics engine
    private void FixedUpdate()
    {
        moveplayer();
    }

    // Moves player dependent on input
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

    // Controls slowing effect of player while on ground or midair
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

    // Collects inputs
    public void input()
    {
        horizontalmov = Input.GetAxisRaw("Horizontal");
        verticalmov = Input.GetAxisRaw("Vertical");

        movedirection = orientation.forward * verticalmov + orientation.right * horizontalmov;
    }

}
