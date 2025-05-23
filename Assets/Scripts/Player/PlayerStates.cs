using System;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    private bool isAlive = true;
    private float fireRate = 0.2f;


    public bool IsAlive
    {
        get { return isAlive; }
        set { isAlive = value; }
    }

    public float FireRate
    {
        get { return fireRate; }
        set { fireRate = value; }
    }

}
