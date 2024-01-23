using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	[SerializeField] private Color defaultColor = Color.white;
	[SerializeField] private Color defaultHighlightColor = new(1f, 199f / 255f, 118f / 255f);
	[SerializeField] private Color defaultDisabledColor = Color.gray;

	private Image image;

	public event Action OnClick;

	private void Awake()
	{
		image = GetComponent<Image>();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		image.color = defaultHighlightColor;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		image.color = defaultColor;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		OnClick?.Invoke();
		OnPointerExit(null); // Reset button state after click
	}
}
