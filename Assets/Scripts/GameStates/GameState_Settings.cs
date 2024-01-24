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

	private void Start()
	{
		music.value = PlayerPrefs.GetFloat(MusicVolKey, 0.75f);
		sound.value = PlayerPrefs.GetFloat(SoundVolKey, 0.75f);

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

		PlayerPrefs.SetFloat(MusicVolKey, music.value);
		PlayerPrefs.SetFloat(SoundVolKey, sound.value);
	}

	#endregion

	public void OnMusicVolumeChanged()
	{
		Debug.Log("[Settings] Music value: " + music.value);
		AudioManager.Instance.SetMusicVolume(Mathf.Log10(music.value) * 20);
	}

	public void OnSoundVolumeChanged()
	{
		Debug.Log("[Settings] SFX value: " + sound.value);
		AudioManager.Instance.SetSFXVolume(Mathf.Log10(sound.value) * 20);
	}

	public void OnBack()
	{
        if (openedAdditively)
			manager.CloseTopState();
		else
			manager.Transition(GameStateType.MainMenu);
	}
}
