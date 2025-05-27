using UnityEngine;

public class Rotate : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the object around the y-axis at a constant rate
        transform.Rotate(0, 50 * Time.deltaTime, 0);
    }
}
