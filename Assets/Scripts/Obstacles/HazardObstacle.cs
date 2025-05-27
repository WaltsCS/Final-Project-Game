using UnityEngine;

public class HazardObstacle : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerStates playerStates = other.gameObject.GetComponent<PlayerStates>();
            playerStates.Die.Invoke();
        }
    }
}