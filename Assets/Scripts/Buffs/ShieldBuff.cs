using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBuff : Buff
{
	private new void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			other.GetComponent<Hero>().CurrentShield += power;
			other.GetComponent<Hero>().VerifyMaxShield();
		}
		base.OnTriggerEnter(other);
	}
}
