using System;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    private bool isAlive = true;
    private float fireRate = 0.2f;
    private int healthBars = 3;


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

    public int HealthBars
    {
        get { return healthBars; }
        set
        {
            healthBars = value;
        }
    }

    public Action Shoot;
    public Action TakeDamage;
    public Action Die;

}
