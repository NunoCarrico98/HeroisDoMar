using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
	[SerializeField] protected float power;
	[SerializeField] protected float timeToRespawn;
	[SerializeField] private GameObject catchVFX;
	[SerializeField] private GameObject spawnVFX;
	[SerializeField] private GameObject cooldownVFX;
	[SerializeField] private GameObject idleVFX;

	private bool used;
	private float catchVFXDuration;

	private MeshRenderer model;
	private VFXManager vfxManager;

	private void Awake()
	{
		model = transform.GetChild(0).GetComponent<MeshRenderer>();
		vfxManager = FindObjectOfType<VFXManager>();
		catchVFXDuration = catchVFX.GetComponent<ParticleSystem>().main.duration;
	}

	private void Start()
	{
		used = false;
	}

	private void ProduceSound()
	{
		// Produce Sound
	}

	private IEnumerator DeactivateBuff()
	{
		yield return new WaitForSeconds(catchVFXDuration);
		vfxManager.EnableVFX(idleVFX, true);
		StartCoroutine(ActivateBuff());
	}

	private IEnumerator ActivateBuff()
	{
		yield return new WaitForSeconds(timeToRespawn);
		vfxManager.EnableVFX(idleVFX, false);
		model.enabled = true;
		used = false;
	}

	protected void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player" && !used)
		{
			used = true;

			vfxManager.EnableVFX(catchVFX, true);
			ProduceSound();

			model.enabled = false;
			StartCoroutine(DeactivateBuff());
		}
	}

}
