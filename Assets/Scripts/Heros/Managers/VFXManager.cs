using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
	[Header("Stun Effect")]
	[SerializeField] private GameObject stunVFX;
	[SerializeField] private float stunYOffset;

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

	public void InstantiateStunVFX(Transform other, float stunDuration)
	{
		Vector3 stunPos = new Vector3(other.position.x, other.position.y + stunYOffset, other.position.z);
		GameObject newT = new GameObject();
		newT.transform.position = stunPos;
		newT.transform.rotation = other.rotation;
		newT.transform.localScale = other.localScale;
		InstantiateVFX(stunVFX, newT.transform, stunDuration);
	}

	public void ControlVFX(GameObject vfx, bool flag) => vfx.SetActive(flag);

	private void SetVFXDuration(ParticleSystem ps, float duration)
	{
		ps.Stop();
		var main = ps.main;
		main.duration = duration;
		ps.Play();
	}
}
