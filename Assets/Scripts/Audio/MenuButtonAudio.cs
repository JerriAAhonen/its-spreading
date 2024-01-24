using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonAudio : MonoBehaviour
{
	[SerializeField] private AudioEvent onEnter;
	[SerializeField] private AudioEvent onClick;

	private MenuButton button;
	
	private void Awake()
	{
		button = GetComponent<MenuButton>();
		button.OnEnter += OnEnter;
		button.OnClick += OnClick;
	}

	private void OnEnter()
	{
		AudioManager.Instance.PlayOnce(onEnter);
	}

	private void OnClick()
	{
		AudioManager.Instance.PlayOnce(onClick);
	}
}
