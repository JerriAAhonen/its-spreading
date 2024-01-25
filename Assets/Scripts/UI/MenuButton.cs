using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	[SerializeField] private Color defaultColor = Color.white;
	[SerializeField] private Color defaultHighlightColor = new(1f, 199f / 255f, 118f / 255f);
	[SerializeField] private Color defaultDisabledColor = Color.gray;
	[Space]
	[SerializeField] private float onEnterScale;
	[SerializeField] private float onEnterDur;

	private bool buttonEnabled = true;
	private TextMeshProUGUI text;
	private int? scaleTweenId;

	public event Action OnEnter;
	public event Action OnClick;

	private void Awake()
	{
		text = GetComponentInChildren<TextMeshProUGUI>();
	}

	public void SetButtonText(string text)
	{
		this.text ??= GetComponentInChildren<TextMeshProUGUI>();
		this.text.text = text;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!buttonEnabled) return;

		OnEnter?.Invoke();
		text.color = defaultHighlightColor;

		if (scaleTweenId.HasValue)
			LeanTween.cancel(scaleTweenId.Value);

		scaleTweenId = LeanTween.scale(gameObject, Vector3.one * onEnterScale, onEnterDur)
			.setEase(LeanTweenType.easeOutBack)
			.setIgnoreTimeScale(true)
			.uniqueId;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!buttonEnabled) return;

		text.color = defaultColor;

		if (scaleTweenId.HasValue)
			LeanTween.cancel(scaleTweenId.Value);

		scaleTweenId = LeanTween.scale(gameObject, Vector3.one, onEnterDur)
			.setEase(LeanTweenType.easeOutQuad)
			.setIgnoreTimeScale(true)
			.uniqueId;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (!buttonEnabled) return;

		OnClick?.Invoke();
		OnPointerExit(null); // Reset button state after click
	}
}
