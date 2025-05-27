using UnityEngine;

public class HomingMine : MonoBehaviour {
    float speed = 2f;
    Transform target;
    public void Init(Transform player) {
        target = player;
        gameObject.SetActive(true);
    }
    void Update() {
        if (!target) return;
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }
}

