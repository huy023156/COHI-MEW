using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image healthBarBg;
    [SerializeField] private Image healthBarCurrent;

    [SerializeField] private HealthSystem healthSystem;

    private void Start()
    {
        healthBarBg.fillAmount = (float)healthSystem.GetMaxHealth() / 10;
        healthBarCurrent.fillAmount = (float)healthSystem.GetCurrentHealth() / 10;

        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHealed += HealthSystem_OnHealed;
    }

    private void HealthSystem_OnHealed()
    {
        healthBarCurrent.fillAmount = (float)healthSystem.GetCurrentHealth() / 10;
    }

    private void HealthSystem_OnDamaged()
    {
        healthBarCurrent.fillAmount = (float)healthSystem.GetCurrentHealth() / 10;
    }
}
