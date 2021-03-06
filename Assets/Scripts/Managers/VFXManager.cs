using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public GameObject InstantiateVFX(GameObject vfx, Transform obj, float duration)
	{
		GameObject goVFX = Instantiate(vfx, obj.position, obj.rotation);
		ParticleSystem thisVFX = goVFX.GetComponent<ParticleSystem>();
		if (thisVFX != null)
		{
			SetVFXDuration(thisVFX, duration);

			foreach(Transform t in thisVFX.transform)
			{
				ParticleSystem childVFX = t.GetComponent<ParticleSystem>();
				if (childVFX != null) SetVFXDuration(childVFX, duration);
			}
		}
		return goVFX;
	}

	public void InstantiateVFXWithYOffset(GameObject vfx, Transform other, float duration, float YOffset)
	{
		Vector3 stunPos = new Vector3(other.position.x, other.position.y + YOffset, other.position.z);
		GameObject newT = new GameObject();
		newT.transform.position = stunPos;
		newT.transform.rotation = other.rotation;
		newT.transform.localScale = other.localScale;
		InstantiateVFX(vfx, newT.transform, duration);
		Destroy(newT);
	}

	public void EnableVFX(GameObject vfx, bool flag) => vfx.SetActive(flag);

	public void EnableVFX(GameObject vfx, float delay)
		=> StartCoroutine(EnableAndDisableVFX(vfx, delay));

	private IEnumerator EnableAndDisableVFX(GameObject vfx, float delay)
	{
		vfx.SetActive(true);
		yield return new WaitForSeconds(delay);
		vfx.SetActive(false);
	}

	private void SetVFXDuration(ParticleSystem ps, float duration)
	{
		ps.Stop();
		var main = ps.main;
		main.duration = duration;
		ps.Play();
	}
}
