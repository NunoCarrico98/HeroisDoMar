using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public float MaximumHealth { get; set; }

    [SerializeField] private Image healthBar;

    private float maximumHealth;

    public void SetHealthbarSize(float damage)
    {
        float ratio = healthBar.rectTransform.localScale.x - damage / MaximumHealth;
        healthBar.rectTransform.localScale = new Vector3(ratio, 1, 1);
    }
}
