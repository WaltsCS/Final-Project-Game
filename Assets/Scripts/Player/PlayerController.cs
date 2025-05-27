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

        Vector3 moveDelta = (transform.forward * verticalInput + transform.right * horizontalInput)
                              * movementSpeed * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + moveDelta);
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
    }
}