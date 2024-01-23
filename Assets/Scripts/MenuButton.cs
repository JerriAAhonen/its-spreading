using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	private TextMeshProUGUI text;
	private Image image;
	private Color defaultColor = Color.white;
	private Color defaultHighlightColor = new(1f, 199f / 255f, 118f / 255f);
	private Color defaultDisabledColor = Color.gray;

	public event Action OnClick;

	private void Awake()
	{
		text = GetComponentInChildren<TextMeshProUGUI>();
		image = GetComponent<Image>();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		//image.color = defaultHighlightColor;
		text.color = defaultHighlightColor;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		//image.color = defaultColor;
		text.color = defaultColor;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		Debug.Log("Click");
		OnClick?.Invoke();
	}
}
