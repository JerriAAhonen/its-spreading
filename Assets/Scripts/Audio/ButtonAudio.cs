using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAudio : MonoBehaviour
{
	[SerializeField] private AudioEvent sfx;

	private Button button;
	
	private void Awake()
	{
		button = GetComponent<Button>();
		button.onClick.AddListener(OnButtonClick);
	}

	private void OnButtonClick()
	{
		AudioManager.Instance.PlayOnce(sfx);
	}
}
