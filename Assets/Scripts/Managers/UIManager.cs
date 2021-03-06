using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
	[Header("Player Stats")]
	[SerializeField] private Image[] healthBars;
	[SerializeField] private Image[] healthRedBars;
	[SerializeField] private Image[] shieldBars;
	[SerializeField] private Image[] shieldRedBars;

	[Header("Player Character Image")]
	[SerializeField] private Image[]  playerCharacterImage;

	[Header("Abilities Images")]
	[SerializeField] private Image[] basicAbilitySpot;
	[SerializeField] private Image[] movementAbilitySpot;
	[SerializeField] private Image[] secondaryAbilitySpot;
	[SerializeField] private Image[] ultimateAbilitySpot;

	[Header("Player Cooldowns")]
	[SerializeField] private Image[] basicUICooldownPanel;
	[SerializeField] private Image[] movementUICooldownPanel;
	[SerializeField] private Image[] secondaryAbilityUICooldownPanel;
	[SerializeField] private Image[] ultimateUICooldownPanel;

	public Image[] HealthBars => healthBars;
	public Image[] HealthRedBars => healthRedBars;
	public Image[] ShieldBars => shieldBars;
	public Image[] ShieldRedBars => shieldRedBars;
	public Image[] BasicUICooldownPanel => basicUICooldownPanel;
	public Image[] MovementUICooldownPanel => movementUICooldownPanel;
	public Image[] SecondaryAbilityUICooldownPanel => secondaryAbilityUICooldownPanel;
	public Image[] UltimateUICooldownPanel => ultimateUICooldownPanel;

	public void SetupCanvas(int pNumber, Sprite characterImage, Sprite[] image)
	{
		playerCharacterImage[pNumber].sprite = characterImage;
		basicAbilitySpot[pNumber].sprite = image[0];
		movementAbilitySpot[pNumber].sprite = image[1];
		secondaryAbilitySpot[pNumber].sprite = image[2];
		ultimateAbilitySpot[pNumber].sprite = image[3];
	}

	public void SetYBarSize(Image bar, float currentStat, float maxStat)
	{
		float ratio = currentStat / maxStat;
		bar.rectTransform.localScale = new Vector3(1, ratio, 1);
	}

	public void ResetYBarSize(Image bar) => bar.rectTransform.localScale = new Vector3(1, 1, 1);

	public void SetXBarSize(Image bar, Image redBar, float currentStat, float maxStat)
	{
		float ratio = currentStat / maxStat;
		Sequence reduceBars = DOTween.Sequence();
		reduceBars.Append(bar.transform.DOScaleX(ratio, 0.2f))
			.Append(redBar.transform.DOScaleX(ratio, 1f));
	}

	public void ResetBar(Image bar) => bar.rectTransform.localScale = new Vector3(1, 1, 1);

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
