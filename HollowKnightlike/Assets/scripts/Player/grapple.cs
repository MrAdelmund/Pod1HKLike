using System.Collections;
using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    [Header("References")]
    public Camera mainCamera;
    public LineRenderer lineRenderer;
    public SpringJoint2D springJoint;
    public Rigidbody2D playerRb;
    public Transform hookTip;
    [Tooltip("Drag your Player Movement script here so it pauses while grappling.")]
    public MonoBehaviour movementScript;

    [Header("Grapple Settings")]
    public KeyCode grappleKey = KeyCode.Z;
    public LayerMask grappleLayer;
    public float maxDistance = 10f;
    public float hookSpeed = 35f;

    [Header("Reel-In Physics")]
    public float reelSpeed = 20f;
    public float springFrequency = 4f;
    [Range(0f, 1f)]
    public float dampingRatio = 0.8f;
    public float autoReleaseDistance = 1.2f;
    public float arrivalUpwardBoost = 5f;

    public bool IsGrappling => isLatched;

    private Vector2 grapplePoint;
    private Vector2 currentHookPos;
    private Transform hitTarget;
    private Vector2 localHitOffset;

    private bool isGrappling = false;
    private bool isLatched = false;
    private float originalGravity;
    private Coroutine grappleCoroutine;

    void Start()
    {
        springJoint.enabled = false;
        lineRenderer.enabled = false;

        if (playerRb != null)
        {
            originalGravity = playerRb.gravityScale;
        }

        if (hookTip != null)
        {
            hookTip.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(grappleKey))
        {
            StartGrapple();
        }
        else if (Input.GetKeyUp(grappleKey))
        {
            StopGrapple(false);
        }

        if (isGrappling)
        {
            if (hitTarget != null)
            {
                grapplePoint = hitTarget.TransformPoint(localHitOffset);
            }

            if (isLatched)
            {
                currentHookPos = grapplePoint;
                springJoint.connectedAnchor = grapplePoint;

                // Shrink spring distance toward 0 to reel player in
                springJoint.distance = Mathf.MoveTowards(springJoint.distance, 0f, reelSpeed * Time.deltaTime);

                if (autoReleaseDistance > 0f && Vector2.Distance(transform.position, grapplePoint) <= autoReleaseDistance)
                {
                    StopGrapple(true);
                    return;
                }
            }

            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, currentHookPos);

            if (hookTip != null)
            {
                hookTip.position = currentHookPos;
            }
        }
    }

    void StartGrapple()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(mouseScreenPos);

        Vector2 direction = (mousePos - (Vector2)transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDistance, grappleLayer);

        if (hit.collider != null)
        {
            hitTarget = hit.transform;
            localHitOffset = hitTarget.InverseTransformPoint(hit.point);
            grapplePoint = hit.point;

            if (grappleCoroutine != null) StopCoroutine(grappleCoroutine);
            grappleCoroutine = StartCoroutine(ShootHookRoutine(direction));
        }
    }

    IEnumerator ShootHookRoutine(Vector2 shootDirection)
    {
        isGrappling = true;
        isLatched = false;
        currentHookPos = transform.position;

        lineRenderer.enabled = true;
        if (hookTip != null)
        {
            hookTip.gameObject.SetActive(true);
            hookTip.position = currentHookPos;
            hookTip.right = shootDirection;
        }

        while (Vector2.Distance(currentHookPos, grapplePoint) > 0.2f)
        {
            currentHookPos = Vector2.MoveTowards(currentHookPos, grapplePoint, hookSpeed * Time.deltaTime);
            yield return null;
        }

        // --- LATCHED ---
        isLatched = true;
        currentHookPos = grapplePoint;

        // Pause your movement script so it doesn't fight the spring pull
        if (movementScript != null)
        {
            movementScript.enabled = false;
        }

        if (playerRb != null)
        {
            playerRb.gravityScale = 0f;
            playerRb.linearVelocity = Vector2.zero; // Change to .velocity if on Unity 2022 or older
        }

        springJoint.connectedBody = null;
        springJoint.autoConfigureDistance = false;
        springJoint.connectedAnchor = grapplePoint;
        springJoint.distance = Vector2.Distance(transform.position, grapplePoint);
        springJoint.frequency = springFrequency;
        springJoint.dampingRatio = dampingRatio;
        springJoint.enabled = true;
    }

    void StopGrapple(bool arrivedAtTarget)
    {
        if (grappleCoroutine != null)
        {
            StopCoroutine(grappleCoroutine);
        }

        isGrappling = false;
        isLatched = false;
        hitTarget = null;
        springJoint.enabled = false;
        lineRenderer.enabled = false;

        // Re-enable your movement script
        if (movementScript != null)
        {
            movementScript.enabled = true;
        }

        if (playerRb != null)
        {
            playerRb.gravityScale = originalGravity;

            if (arrivedAtTarget && arrivalUpwardBoost > 0f)
            {
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x * 0.5f, arrivalUpwardBoost);
            }
        }

        if (hookTip != null)
        {
            hookTip.gameObject.SetActive(false);
        }
    }
}