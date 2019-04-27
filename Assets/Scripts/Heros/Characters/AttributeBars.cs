using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AttributeBars : MonoBehaviour
{
    public float MaximumHealth { get; set; }
    public float MaximumShield { get; set; }

    [SerializeField] private Image healthBar;
    [SerializeField] private Image shieldBar;

    public void SetHealthBarSize(float currentHealth)
    {
        float ratio = currentHealth / MaximumHealth;
        healthBar.rectTransform.localScale = new Vector3(ratio, 1, 1);
    }

    public void SetShieldBarSize(float currentShield)
    {
        float ratio = currentShield / MaximumShield;
        shieldBar.rectTransform.localScale = new Vector3(ratio, 1, 1);
    }
}
