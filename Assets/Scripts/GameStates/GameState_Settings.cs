using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameState_Settings : GameState
{
	public const string MusicVolKey = "MusicVolume";
	public const string SoundVolKey = "SoundVolume";

	[SerializeField] private Slider music;
	[SerializeField] private Slider sound;
	[SerializeField] private MenuButton back;

	private void Awake()
	{
		if (PlayerPrefs.HasKey(MusicVolKey))
		{
			var vol = PlayerPrefs.GetFloat(MusicVolKey);
			music.value = vol;
		}

		if (PlayerPrefs.HasKey(SoundVolKey))
		{
			var vol = PlayerPrefs.GetFloat(SoundVolKey);
			sound.value = vol;
		}

		back.OnClick += OnBack;
	}

	#region GameState

	public override void Enter()
	{
		gameObject.SetActive(true);
	}

	public override void Exit()
	{
		gameObject.SetActive(false);
		// Save sound settings

		PlayerPrefs.SetFloat(MusicVolKey, music.value);
		PlayerPrefs.SetFloat(SoundVolKey, sound.value);
	}

	#endregion

	public void OnMusicVolumeChanged()
	{
		Debug.Log("Music vol: " + music.value);
	}

	public void OnSoundVolumeChanged()
	{
		Debug.Log("Sound vol: " + sound.value);
	}

	public void OnBack()
	{
		manager.Transition(GameStateType.MainMenu);
	}
}
