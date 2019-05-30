using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public class AttributeBars
{
	[Header("")]
    [SerializeField] private Image healthBar;
    [SerializeField] private Image shieldBar;

    public void SetHealthBarSize(float currentHealth, float maxHealth)
    {
        float ratio = currentHealth / maxHealth;
        healthBar.rectTransform.localScale = new Vector3(ratio, 1, 1);
    }

    public void SetShieldBarSize(float currentShield, float maxShield)
    {
		float ratio = currentShield / maxShield;
        shieldBar.rectTransform.localScale = new Vector3(ratio, 1, 1);
    }

	private void SetYBarSize(Image bar, float cooldown, float currentTime)
	{
		float ratio = currentTime / cooldown;
		bar.rectTransform.localScale = new Vector3(1, ratio, 1);
	}
}
