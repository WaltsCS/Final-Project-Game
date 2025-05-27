using UnityEngine;

public class GateKey : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("PlayerBullet"))
            return;

        Destroy(collision.gameObject);  // remove the bullet
        Destroy(gameObject);            // remove this key

        // Notify all GateManagers that care about this key's tag
        GateManager.NotifyKeyDestroyedForTag(gameObject.tag);
    }
}
