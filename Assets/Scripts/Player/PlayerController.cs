using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Properties")]
    [SerializeField] private float movementSpeed = 10f;

    [Header("Mouse Look")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float rotationLerpSpeed = 15f;

    [Header("Shoot Properties")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;

    private Rigidbody rb;
    private PlayerStates playerStates;
    private LevelManager levelManager;
    private float nextFireTime;

    void Awake()
    {
        playerStates = GetComponent<PlayerStates>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        rb = GetComponent<Rigidbody>();

        if (playerCamera == null)
            Debug.LogError("PlayerController: please assign the playerCamera field.");
    }

    private void OnEnable()
    {
        playerStates.Shoot += ShootBullet;
    }

    private void OnDisable()
    {
        playerStates.Shoot -= ShootBullet;
    }

    void Update()
    {
        // Handle shooting
        if (levelManager.IsLevelActive && Input.GetButton("Fire1") && Time.time > nextFireTime)
        {
            playerStates.Shoot.Invoke();
            nextFireTime = Time.time + playerStates.FireRate;
        }

        // Handle mouse‐based rotation
        if (levelManager.IsLevelActive)
            RotateTowardsMouse();
    }

    void FixedUpdate()
    {
        if (!levelManager.IsLevelActive) return;

        // Movement input
        float verticalInput = Input.GetAxis("Vertical Movement");
        float horizontalInput = Input.GetAxis("Horizontal Movement");

        // Compute desired move direction and speed
        Vector3 moveDir = (transform.forward * verticalInput + transform.right * horizontalInput).normalized;
        Vector3 desiredVelocity = moveDir * movementSpeed;

        if (moveDir.sqrMagnitude > 0.01f)
        {
            // Check for obstacles ahead to prevent clipping through seams
            float checkDistance = 0.5f;  // adjust to match player radius
            RaycastHit hitInfo;
            if (Physics.SphereCast(transform.position, 0.4f, moveDir, out hitInfo, checkDistance, LayerMask.GetMask("Default", "Wall")))
            {
                // Slow down when near obstacles
                float slowFactor = 0.3f;  // reduce to 30% speed when near
                desiredVelocity *= slowFactor;
            }
        }

        // Apply horizontal velocity; preserve vertical velocity (gravity etc.)
        rb.linearVelocity = new Vector3(desiredVelocity.x, rb.  linearVelocity.y, desiredVelocity.z);
    }

    private void RotateTowardsMouse()
    {
        // Ray from camera through the mouse position
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);

            // Compute target rotation
            Vector3 direction = hitPoint - transform.position;
            direction.y = 0f;
            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(direction, Vector3.up);
                // Smooth it
                transform.rotation = Quaternion.Lerp(transform.rotation,
                                                      targetRot,
                                                      Time.deltaTime * rotationLerpSpeed);
            }
        }
    }

    private void ShootBullet()
    {
        Instantiate(bulletPrefab,
                    bulletSpawnPoint.position,
                    bulletSpawnPoint.rotation);
        
        if (SFX.Instance != null)
            SFX.Instance.PlayHitSound();


    }

    private void OnCollisionEnter(Collision collision)
    {
        // If colliding with walls/obstacles, project velocity away from penetration
        ContactPoint contact = collision.GetContact(0);
        Vector3 normal = contact.normal;
        rb.linearVelocity = Vector3.ProjectOnPlane(rb.linearVelocity, normal);
    }


    private void OnCollisionStay(Collision collision)
    {
        // Continually correct velocity while in contact
        ContactPoint contact = collision.GetContact(0);
        Vector3 normal = contact.normal;
        rb.linearVelocity = Vector3.ProjectOnPlane(rb.linearVelocity, normal);
    }
}