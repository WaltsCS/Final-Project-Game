using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;        // Avatar/player transform
    [SerializeField] private Vector3 offset = new Vector3(0, 10f, 0); // Top-down camera position
    [SerializeField] private float smoothSpeed = 5f;  // Damping factor

    void FixedUpdate()
    {
        if (target == null) return;

        // Desired position: just above the player
        Vector3 desiredPosition = target.position + offset;

        // Smoothly interpolate to that position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.fixedDeltaTime);

    }
}
