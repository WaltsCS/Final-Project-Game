using System;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    private bool isAlive = true;


    public bool IsAlive
    {
        get { return isAlive; }
        set { isAlive = value; }
    }

}
