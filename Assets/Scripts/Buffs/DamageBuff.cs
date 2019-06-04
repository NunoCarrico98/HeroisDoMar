using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBuff : Buff
{
	private new void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
			other.GetComponent<Hero>().DamageMultiplier += power;
		base.OnTriggerEnter(other);
	}


}
