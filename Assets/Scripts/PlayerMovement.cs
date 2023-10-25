using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    private bool jumping;
    private float jumpTime;
    private float buttonTime = 0.3f;
    private bool freeze;
    private bool inConsole;
    public bool activeGrapple;
    public float groundDrag;
    public float distToGround = 1f;

    public LayerMask whatisGround;

    [Header("Slopehandle")]
    public float maxSlopeAngle;

    private Vector3 moveDirection;
    private Transform orientation;
    private RaycastHit slopeHit;
    private bool grounded;
    private bool doubleJump;

    [SerializeField] private TetrisConsole[] allTetrisConsole;
    [SerializeField] private CameraHandler cams;
    private TetrisConsole activeConsole;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        orientation = cam.transform;
        moveDirection = orientation.forward * Input.GetAxis("Vertical") + orientation.right * Input.GetAxis("Horizontal");
        if (activeGrapple) return;
        if (freeze){ rb.velocity = Vector3.zero; return; }
        if (grounded)
        {
            Vector3 newVelocity = cam.transform.forward * Input.GetAxis("Vertical") * speed;
            newVelocity += cam.transform.right * Input.GetAxis("Horizontal") * speed;
            newVelocity.y = rb.velocity.y;
            rb.velocity = newVelocity;
        }
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, distToGround + 0.1f, whatisGround);
        if (grounded)
        {
            doubleJump = true;
        }

        OnSlope();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (grounded)
            {
                jumping = true;
                jumpTime = 0;
            } else if (doubleJump)
            {
                doubleJump = false;
                jumping = true;
                jumpTime = 0;
            }
        }
        if (jumping)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            jumpTime += Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.Space) | jumpTime > buttonTime)
        {
            jumping = false;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (inConsole)
            {
                ExitConsole();
                return;
            }
            
            foreach (TetrisConsole tconsole in allTetrisConsole)
            {
                float dist = Vector3.Distance(transform.position, tconsole.transform.position);
                if (dist < 1.4f)
                {
                    if (!inConsole)
                    {
                        EnterConsole(tconsole);
                        activeConsole = tconsole;
                    }
                }
            }

        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit hit;
            Ray downRay = new Ray(transform.position, Vector3.down);
            Physics.Raycast(downRay, out hit);
            Debug.Log(hit.distance);
            Debug.Log("Is grounded: " + grounded);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            grounded = true;
        }
    }


    public void jumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        activeGrapple = true;
        velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
        Invoke(nameof(SetVelocity), 0.1f);
    }
    private Vector3 velocityToSet;
    private void SetVelocity()
    {
        rb.velocity = velocityToSet;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, cam.transform.position.y * distToGround + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }
    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity)
            + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }

    public void EnterConsole(TetrisConsole tconsole)
    {
        freeze = true;
        inConsole = true;
        cams.SwitchToCam(tconsole.cam);
        tconsole.activate();
    }

    public void ExitConsole()
    {
        if (activeConsole.deactivate())
        {
            freeze = false;
            inConsole = false;
            cams.SwitchToPlayerCam();
        }
    }
}
