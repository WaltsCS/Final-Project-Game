using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player States")]
    private PlayerStates playerStates;

    [Header("Movement Properties")]
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float rotationSpeed = 50f;

    [Header("Shoot Properties")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;


    void Awake()
    {
        playerStates = GetComponent<PlayerStates>();
    }

    void Update()
    {
        // Inputs
        // [Movement and Rotation]
        float verticalInput = Input.GetAxis("Vertical Movement");
        float horizontalInput = Input.GetAxis("Horizontal Movement");
        float horizontalRotation = Input.GetAxis("Horizontal Rotation");

        Vector3 verticalMovement = movementSpeed * verticalInput * Time.deltaTime * Vector3.forward;
        Vector3 horizontalMovement = movementSpeed * horizontalInput * Time.deltaTime * Vector3.right;
        Vector3 horizontalRotationMovement = rotationSpeed * horizontalRotation * Time.deltaTime * Vector3.up;

        if (playerStates.IsAlive)
        {
            transform.Translate(verticalMovement);
            transform.Translate(horizontalMovement);
            transform.Rotate(horizontalRotationMovement);

            if (Input.GetButtonDown("Fire1"))
            {
                ShootBullet();
            }

        }
    }

    private void ShootBullet()
    {
        Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
    }
}
