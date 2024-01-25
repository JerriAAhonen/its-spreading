using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum LevelButtonState { Completed, Current, Locked }

public class LevelSelection_LevelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	[SerializeField] private TextMeshProUGUI levelNumLabel;
	[SerializeField] private Image buttonImage;
	[SerializeField] private Sprite completedSprite;
	[SerializeField] private Sprite currentSprite;
	[SerializeField] private Sprite lockedSprite;
	[Space]
	[SerializeField] private Transform scaleContainer;
	[SerializeField] private float onEnterScale;
	[SerializeField] private float onEnterDur;
	[Space]
	[SerializeField] private AudioEvent onEnter;
	[SerializeField] private AudioEvent onClick;

	private int index;
	private int? scaleTweenId;

	public event Action<int> LevelSelected;

	public void Init(int index)
	{
		this.index = index;
		levelNumLabel.text = (index + 1).ToString();
	}

	public void RefreshState(LevelButtonState state)
	{
		buttonImage.sprite = state switch
		{
			LevelButtonState.Completed => completedSprite,
			LevelButtonState.Current => currentSprite,
			LevelButtonState.Locked => lockedSprite,
			_ => throw new NotImplementedException(),
		};
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
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
		if (scaleTweenId.HasValue)
			LeanTween.cancel(scaleTweenId.Value);

		scaleTweenId = LeanTween.scale(gameObject, Vector3.one, onEnterDur)
			.setEase(LeanTweenType.easeOutQuad)
			.setIgnoreTimeScale(true)
			.uniqueId;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		Debug.Log("Clicked level " + index);
		AudioManager.Instance.PlayOnce(onClick);
		LevelSelected?.Invoke(index);
		OnPointerExit(null);
	}
}
