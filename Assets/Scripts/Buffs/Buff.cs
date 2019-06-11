using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
	private float spawnVFXDuration;
	private GameObject model;
	private Animator modelAnim;
	private VFXManager vfxManager;

	private void Awake()
	{
		model = transform.GetChild(0).gameObject;
		modelAnim = model.GetComponent<Animator>();
		vfxManager = FindObjectOfType<VFXManager>();
		catchVFXDuration = catchVFX.GetComponent<ParticleSystem>().main.duration;
		spawnVFXDuration = spawnVFX.GetComponent<ParticleSystem>().main.duration;
	}

	private void Start()
	{
		used = false;
	}

	private void ProduceSound()
	{
		// Produce Sound
	}

	private IEnumerator ManageBuff()
	{
		// Deactivate buff
		yield return new WaitForSeconds(catchVFXDuration);
		vfxManager.EnableVFX(idleVFX, true);

		// Spawn Buff
		yield return new WaitForSeconds(timeToRespawn);
		vfxManager.EnableVFX(idleVFX, false);
		vfxManager.EnableVFX(spawnVFX, spawnVFXDuration);
		model.transform.DOLocalMoveY(0f, spawnVFXDuration);
		model.SetActive(true);

		// Activate Buff
		yield return new WaitForSeconds(spawnVFXDuration);
		modelAnim.enabled = true;
		used = false;
	}

	protected void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player" && !used)
		{
			used = true;

			vfxManager.EnableVFX(catchVFX, true);
			ProduceSound();

			model.transform.DOLocalMoveY(-1.5f, 0);
			modelAnim.enabled = false;
			model.SetActive(false);
			StartCoroutine(ManageBuff());
		}
	}
}
