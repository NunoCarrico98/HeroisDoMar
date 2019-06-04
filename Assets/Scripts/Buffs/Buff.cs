using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
	[SerializeField] protected float power;
	[SerializeField] protected float timeToRespawn;

	private bool used;
	private float vfxDuration;

	private GameObject vfx;
	private VFXManager vfxManager;

	private void Awake()
	{
		vfxManager = FindObjectOfType<VFXManager>();
		vfx = transform.GetChild(0).gameObject;
		vfxDuration = vfx.GetComponent<ParticleSystem>().main.duration;
	}

	private void Start()
	{
		used = false;
	}

	private void SpawnCatchVFX()
	{
		vfxManager.EnableVFX(vfx, true);
	}

	private void ProduceSound()
	{
		// Produce Sound
	}

	private IEnumerator DeactivateBuff()
	{
		yield return new WaitForSeconds(vfxDuration);
		StartCoroutine(ActivateBuff());
	}

	private IEnumerator ActivateBuff()
	{
		yield return new WaitForSeconds(timeToRespawn);
		GetComponent<MeshRenderer>().enabled = true;
		used = false;
	}

	protected void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player" && !used)
		{
			used = true;

			SpawnCatchVFX();
			ProduceSound();

			GetComponent<MeshRenderer>().enabled = false;
			StartCoroutine(DeactivateBuff());
		}
	}

}
