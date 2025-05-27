using System;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    private float fireRate = 0.2f;
    private int healthBars = 3;

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
            if (healthBars <= 0)
            {
                Die?.Invoke();
            }
        }
    }

public void AddHealth(int amount)
{
    healthBars = Mathf.Min(healthBars + amount, 3); // Max of 3
    ReplenishHealth?.Invoke();
}


    public Action Shoot;
    public Action TakeDamage;
    public Action Die;
    public Action IncreaseFireRate;
    public Action ReplenishHealth;

}
