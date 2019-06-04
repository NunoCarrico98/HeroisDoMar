using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBuff : Buff
{
	private new void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			other.GetComponent<Hero>().CurrentHealth += power;
			other.GetComponent<Hero>().VerifyMaxHealth();			
		}
		base.OnTriggerEnter(other);
	}
}
