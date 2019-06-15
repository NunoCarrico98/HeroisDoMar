using UnityEngine;
using System.Collections;

public class ClothOnWind : MonoBehaviour
{

	private Cloth clothMod;
	private Vector3 TargetWindForce;

	[SerializeField] private Vector3 MaxWindForce;
	[SerializeField] private float WindForceIntervals = 1;
	[SerializeField] private Vector3 NegativeWindForce;

	// Use this for initialization
	void Start()
	{
		clothMod = GetComponent<Cloth>();
		clothMod.externalAcceleration = NegativeWindForce;
		TargetWindForce = MaxWindForce;
		if (WindForceIntervals <= 0)
		{
			WindForceIntervals = 0.1f;
		}
	}

	// Update is called once per frame
	void Update()
	{
		clothMod.externalAcceleration = Vector3.MoveTowards(clothMod.externalAcceleration, TargetWindForce, WindForceIntervals * Time.deltaTime);

		if (clothMod.externalAcceleration == MaxWindForce)
		{
			TargetWindForce = NegativeWindForce;
		}
		if (clothMod.externalAcceleration == NegativeWindForce)
		{
			TargetWindForce = MaxWindForce;
		}
	}
}
