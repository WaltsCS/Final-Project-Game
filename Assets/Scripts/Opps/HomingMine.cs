using System.Collections;
using UnityEngine;

public class HomingMine : MonoBehaviour
{
    float speed = 2f;
    Transform target;
    public void Init(Transform player)
    {
        target = player;
        gameObject.SetActive(true);
    }
    void Update()
    {
        if (!target) return;
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        StartCoroutine(CheckForFinalBoss());
    }

    private IEnumerator CheckForFinalBoss()
    {
        while (GameObject.FindGameObjectsWithTag("FinalBoss").Length > 0)
            yield return null;
        Destroy(gameObject);

    }
}

