using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PowerUpUI : MonoBehaviour
{
    [SerializeField] private Image powerUpIcon;

    private void Start()
    {
        powerUpIcon.enabled = false; 
    }

    public void ShowPowerUp(float duration)
    {
        StartCoroutine(ShowPowerUpCoroutine(duration));
    }

    private IEnumerator ShowPowerUpCoroutine(float duration)
    {
        powerUpIcon.enabled = true;             
        yield return new WaitForSeconds(duration);
        powerUpIcon.enabled = false;          
    }
}
