using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public float MaximumHealth { get; set; }
    public float Health { get; set; }

    [SerializeField] private Image healthBar;

    public void SetHealthbarSize(float damage)
    {
        Health -= damage;
        float ratio = Health / MaximumHealth;
        healthBar.rectTransform.localScale = new Vector3(ratio, 1, 1);
    }
}
