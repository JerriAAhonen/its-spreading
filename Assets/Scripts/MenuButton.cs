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
	
	private TextMeshProUGUI text;

	public event Action OnClick;

	private void Awake()
	{
		text = GetComponentInChildren<TextMeshProUGUI>();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		text.color = defaultHighlightColor;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		text.color = defaultColor;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		OnClick?.Invoke();
		OnPointerExit(null); // Reset button state after click
	}
}
