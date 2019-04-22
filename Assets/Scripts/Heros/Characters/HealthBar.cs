using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public float MaximumHealth { get; set; }

    [SerializeField] private Image healthBar;

    public void SetHealthBarSize(float currentHealth)
    {
        float ratio = currentHealth / MaximumHealth;
        healthBar.rectTransform.localScale = new Vector3(ratio, 1, 1);
    }
}
