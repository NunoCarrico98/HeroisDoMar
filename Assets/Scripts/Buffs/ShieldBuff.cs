using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBuff : Buff
{
	private new void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
			other.GetComponent<Hero>().HealShield(power);
		base.OnTriggerEnter(other);
	}
}
