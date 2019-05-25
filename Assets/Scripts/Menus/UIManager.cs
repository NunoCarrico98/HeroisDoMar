using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
	public void FadeText(TextMeshProUGUI text, bool fadeOut, float _fadeDuration)
	{
		// If not to fade in
		if (fadeOut)
		{
			text.enabled = false;
			text.CrossFadeAlpha(0, _fadeDuration, true);
		}
		else
		{
			text.enabled = true;
			text.canvasRenderer.SetAlpha(0);
			text.CrossFadeAlpha(1, _fadeDuration, true);
		}
	}

	public void FadeUiElement(Graphic uiElement, bool fadeOut, float _fadeDuration)
	{
		if (fadeOut)
		{
			uiElement.enabled = false;
			uiElement.CrossFadeAlpha(0, _fadeDuration, true);
		}
		else
		{
			uiElement.enabled = true;
			uiElement.canvasRenderer.SetAlpha(0);
			uiElement.CrossFadeAlpha(1, _fadeDuration, true);
		}
	}
}
