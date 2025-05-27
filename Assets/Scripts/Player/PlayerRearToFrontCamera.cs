using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRearToFrontCamera : MonoBehaviour
{
    [SerializeField] private Transform player;      //ref to player, assign camera to follow
    [SerializeField] private Vector3 offset = Vector3.zero;         //adjust in Inspector relative to plane

    [SerializeField] private float followSpeed = 10f;        //adjust how fast camera follows or how much lagging behind
    [SerializeField] private float rotationSpeed = 10f;      //adjust camera rotation speed to match plane movement and direction
    [SerializeField] private float tilt = -75f;     //adjust camera tilt

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetPosition = player.position + player.transform.rotation * offset;      //ENSURES fixed camera even w/ rotation
        //linear interpolation; move following camera to new position smoothly
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);

        //camera looks from its pos to plane's pos (aligned with world's Up)
        Quaternion targetRotation = Quaternion.LookRotation(player.position - targetPosition, Vector3.up);

        //while camera follow plane, apply tilt to camera
        targetRotation *= Quaternion.Euler(tilt, 0, 0);

        //spherical interpolaton; rotate camera gradually to match forward plane direction
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

    }
}


