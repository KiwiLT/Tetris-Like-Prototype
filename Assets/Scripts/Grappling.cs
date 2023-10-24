using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    [Header("References")]
    private PlayerMovement pm;
    [SerializeField] public Transform cam;
    [SerializeField] public Transform gunTip;
    [SerializeField] public LayerMask whatIsGrappleable;
    [SerializeField] public LineRenderer lr;

    [Header("Grappling")]
    [SerializeField] public float maxGrappleDistance;
    [SerializeField] public float grappleDelayTime;
    [SerializeField] public float overshootYAxis;

    private Vector3 grapplePoint;

    [Header("Cooldown")]
    [SerializeField] public float grapplingCd;
    [SerializeField] public float grapplingCdTimer;

    [Header("Input")]
    [SerializeField] public KeyCode grappleKey = KeyCode.Mouse1;

    private bool grappling;

    // Start is called before the first frame update
    void Start()
    {
        pm = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(grappleKey)) startGrapple();

        if (grapplingCdTimer > 0)
        {
            grapplingCdTimer -= Time.deltaTime;
        }
        if (Input.GetKeyUp(grappleKey)) stopGrapple();
    }

    private void LateUpdate()
    {
        if (grappling)
        {
            lr.SetPosition(0, gunTip.position);
        }
    }

    private void startGrapple()
    {
        if (grapplingCdTimer > 0) return;

        grappling = true;
        pm.activeGrapple = grappling;

        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;

            Invoke(nameof(executeGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;

            Invoke(nameof(stopGrapple), grappleDelayTime);
        }
        lr.enabled = true;
        lr.SetPosition(1, grapplePoint);
    }

    private void executeGrapple()
    {

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;

        pm.jumpToPosition(grapplePoint, highestPointOnArc);

        //Invoke(nameof(stopGrapple), 1f);
    }

    private void stopGrapple()
    {
        grappling = false;
        pm.activeGrapple = grappling;

        grapplingCdTimer = grapplingCd;

        lr.enabled = false;
    }
}
