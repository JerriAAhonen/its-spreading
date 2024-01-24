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
	private bool isIconPauseIcon = true;

	public event Action OnClick;

	private void Awake()
	{
		image = GetComponent<Image>();
	}

	// This is horrible, and I hate myself for coming up with such an attrocity.
	// Alas I am running out of time and so this monster shall live, and haunt me until the end of times.
	// Or until I fix it. Let's not get our hopes up tho..
	private void Update()
	{
		if (Time.timeScale.Approximately(0f) && isIconPauseIcon)
		{
			SetIcon(false);
			isIconPauseIcon = false;
		}
		else if (Time.timeScale.Approximately(1f) && !isIconPauseIcon)
		{
			SetIcon(true);
			isIconPauseIcon = true;
		}
	}

	public void SetIcon(bool pause)
	{
		image.sprite = pause ? pauseIcon : playIcon;
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
	}
}
