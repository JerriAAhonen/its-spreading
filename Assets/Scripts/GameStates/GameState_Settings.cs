using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameState_Settings : GameState
{
	[SerializeField] private Slider music;
	[SerializeField] private Slider sound;
	[SerializeField] private MenuButton back;

	private void Start()
	{
		AudioManager.Instance.GetSavedVolumes(out float musicVol, out float sfxVol);
		music.value = musicVol;
		sound.value = sfxVol;

		back.OnClick += OnBack;
	}

	#region GameState

	public override void Enter()
	{
		gameObject.SetActive(true);
	}

	public override void Exit()
	{
		AudioManager.Instance.SaveVolumes(music.value, sound.value);
		gameObject.SetActive(false);
	}

	#endregion

	public void OnMusicVolumeChanged()
	{
		Debug.Log("[Settings] Music value: " + music.value);
		AudioManager.Instance.SetMusicVolume(music.value);
	}

	public void OnSoundVolumeChanged()
	{
		Debug.Log("[Settings] SFX value: " + sound.value);
		AudioManager.Instance.SetSFXVolume(sound.value);
	}

	public void OnBack()
	{
        if (openedAdditively)
			manager.CloseTopState();
		else
			manager.Transition(GameStateType.MainMenu);
	}
}
