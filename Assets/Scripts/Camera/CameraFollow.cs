using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target and Offset")]
    [SerializeField] private Transform target;        // Avatar/player transform
    [SerializeField] private Vector3 offset = new Vector3(0, 10f, 0); // Top-down camera position
    [SerializeField] private Vector3 rotationEulerOffset = new Vector3(0f, 0f, 0f);

    [Header("Movement Smoothness")]
    [SerializeField] private float smoothSpeed = 5f;  // Damping factor

    void FixedUpdate()
    {
        if (target == null) return;

        // Desired position: just above the player
        Vector3 desiredPosition = target.position + offset;

        // Smoothly interpolate to that position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.fixedDeltaTime);

        // Compute desired rotation based on Euler offset
        Quaternion desiredRotation = Quaternion.Euler(rotationEulerOffset);
        // Smoothly interpolate to desired rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.fixedDeltaTime);

    }
}
