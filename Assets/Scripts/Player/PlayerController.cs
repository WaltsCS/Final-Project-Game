using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float rotationSpeed = 50f;
    private PlayerStates playerStates;

    void Awake()
    {
        playerStates = GetComponent<PlayerStates>();
    }

    void Update()
    {
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
        }
    }
}
