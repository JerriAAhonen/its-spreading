using System;
using TMPro;
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

	private int index;

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
		
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		Debug.Log("Clicked level " + index);
		LevelSelected?.Invoke(index);
	}
}
