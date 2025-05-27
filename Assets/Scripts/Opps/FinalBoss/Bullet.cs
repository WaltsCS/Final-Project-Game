using System.Collections;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    float speed = 5f;
    Vector3 dir;
    public void Init(Vector3 direction)
    {
        dir = direction.normalized;
        gameObject.SetActive(true);
    }
    void Update()
    {
        transform.position += dir * speed * Time.deltaTime;
        if (OutOfBounds()) ObjectPooler.Instance.ReturnToPool("Bullet", gameObject);
    }

    bool OutOfBounds()
    {
        // Check if the bullet is out of bounds (e.g., beyond a certain distance from the origin)
        return Vector3.Distance(Vector3.zero, transform.position) > 50f; // Example threshold
    }
}
