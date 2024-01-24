using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	[SerializeField] private Color defaultColor = Color.white;
	[SerializeField] private Color defaultHighlightColor = new(1f, 199f / 255f, 118f / 255f);
	[SerializeField] private Color defaultDisabledColor = Color.gray;
	[Space]
	[SerializeField] private float onEnterScale;
	[SerializeField] private float onEnterDur;
	
	private TextMeshProUGUI text;
	private int? scaleTweenId;

	public event Action OnEnter;
	public event Action OnClick;

	private void Awake()
	{
		text = GetComponentInChildren<TextMeshProUGUI>();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		OnEnter?.Invoke();
		text.color = defaultHighlightColor;

		if (scaleTweenId.HasValue)
			LeanTween.cancel(scaleTweenId.Value);

		scaleTweenId = LeanTween.scale(gameObject, Vector3.one * onEnterScale, onEnterDur)
			.setEase(LeanTweenType.easeOutBack)
			.uniqueId;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		text.color = defaultColor;

		if (scaleTweenId.HasValue)
			LeanTween.cancel(scaleTweenId.Value);

		scaleTweenId = LeanTween.scale(gameObject, Vector3.one, onEnterDur)
			.setEase(LeanTweenType.easeOutQuad)
			.uniqueId;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		OnClick?.Invoke();
		OnPointerExit(null); // Reset button state after click
	}
}
