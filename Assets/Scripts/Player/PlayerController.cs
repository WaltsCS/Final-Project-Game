using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Properties")]
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float rotationSpeed = 50f;

    [Header("Shoot Properties")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;

    private Rigidbody rb;
    private PlayerStates playerStates;
    private float nextFireTime;


    void Awake()
    {
        playerStates = GetComponent<PlayerStates>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (playerStates.IsAlive && Input.GetButton("Fire1") && Time.time > nextFireTime)
        {
            ShootBullet();
            nextFireTime = Time.time + playerStates.FireRate;
        }
    }

    void FixedUpdate()
    {
        if (!playerStates.IsAlive) return;

        // Get inputs
        float verticalInput = Input.GetAxis("Vertical Movement");
        float horizontalInput = Input.GetAxis("Horizontal Movement");
        float horizontalRotation = Input.GetAxis("Horizontal Rotation");

        Vector3 movement = (transform.forward * verticalInput + transform.right * horizontalInput) * movementSpeed * Time.fixedDeltaTime;

        // Apply movement using Rigidbody
        Vector3 newPosition = rb.position + movement;
        rb.MovePosition(newPosition);

        // Apply rotation using Rigidbody
        if (Mathf.Abs(horizontalRotation) > 0.1f)
        {
            float rotationAmount = horizontalRotation * rotationSpeed * Time.fixedDeltaTime;
            Quaternion deltaRotation = Quaternion.Euler(0, rotationAmount, 0);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
    }

    private void ShootBullet()
    {
        Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
    }
}