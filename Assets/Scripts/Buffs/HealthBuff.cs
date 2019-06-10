using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBuff : Buff
{
	private new void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
			other.GetComponent<Hero>().HealHealth(power);			
		base.OnTriggerEnter(other);
	}
}
