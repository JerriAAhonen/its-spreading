using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	[SerializeField] private Color defaultColor = Color.white;
	[SerializeField] private Color defaultHighlightColor = new(1f, 199f / 255f, 118f / 255f);
	[SerializeField] private Color defaultDisabledColor = Color.gray;
	[SerializeField] private Sprite pauseIcon;
	[SerializeField] private Sprite playIcon;
	[Space]
	[SerializeField] private float onEnterScale;
	[SerializeField] private float onEnterDur;
	[Space]
	[SerializeField] private AudioEvent onEnter;
	[SerializeField] private AudioEvent onClick;

	private Image image;
	private int? scaleTweenId;

	public event Action OnClick;

	private void Awake()
	{
		image = GetComponent<Image>();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		image.color = defaultHighlightColor;
		AudioManager.Instance.PlayOnce(onEnter);

		if (scaleTweenId.HasValue)
			LeanTween.cancel(scaleTweenId.Value);

		scaleTweenId = LeanTween.scale(gameObject, Vector3.one * onEnterScale, onEnterDur)
			.setEase(LeanTweenType.easeOutBack)
			.setIgnoreTimeScale(true)
			.uniqueId;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		image.color = defaultColor;

		if (scaleTweenId.HasValue)
			LeanTween.cancel(scaleTweenId.Value);

		scaleTweenId = LeanTween.scale(gameObject, Vector3.one, onEnterDur)
			.setEase(LeanTweenType.easeOutQuad)
			.setIgnoreTimeScale(true)
			.uniqueId;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		AudioManager.Instance.PlayOnce(onClick);
		OnClick?.Invoke();
		OnPointerExit(null);
	}
}
