using UnityEngine;

public class OutOfBoundsGameOver : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStates playerStates = other.GetComponent<PlayerStates>();
            playerStates.Die.Invoke();
            Destroy(other.gameObject);
        }
    }

}
