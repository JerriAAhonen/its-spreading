using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

public class AudioManager : Singleton<AudioManager>
{
	public const string MusicVolumeKey = "AudioManager_MusicVolumeKey";
	public const string SFXVolumeKey = "AudioManager_SFXVolumeKey";

	[SerializeField] private Transform sourceParent;
	[SerializeField] private AudioMixer audioMixer;
	[SerializeField] private AudioMixerGroup sfxAudioMixerGroup;
	[SerializeField] private AudioMixerGroup musicAudioMixerGroup;
	[SerializeField] private AudioClip music;
	[SerializeField] private float musicVolume;

	private Transform mainCamTm;
	private IObjectPool<AudioSource> pool;
	private readonly List<AudioSource> activeSources = new();
	private readonly Dictionary<Transform, AudioSource> loopingSounds = new(); // Only supports one looping sound per requester atm
	private readonly Dictionary<AudioEvent, float> cooldowns = new();

	private AudioSource musicSource;

	protected override void Awake()
	{
		base.Awake();
			
		pool = new ObjectPool<AudioSource>(
			CreateAudioSource,
			s =>
			{
				s.gameObject.SetActive(true);
				activeSources.Add(s);
			},
			s =>
			{
				s.Stop();
				s.clip = null;
				s.gameObject.SetActive(false);
				s.transform.SetParent(sourceParent);
				activeSources.Remove(s);
			},
			Destroy);

		musicSource = CreateAudioSource();
		musicSource.clip = music;
		musicSource.volume = musicVolume;
		musicSource.loop = true;
		musicSource.transform.SetParent(sourceParent);
		musicSource.transform.localPosition = Vector3.zero;
		musicSource.Play();
		musicSource.outputAudioMixerGroup = musicAudioMixerGroup;
	}

	private void Start()
	{
		mainCamTm = Camera.main.transform;
	}

	private void LateUpdate()
	{
		foreach (var s in activeSources)
		{
			if (!s.isPlaying)
			{
				pool.Release(s);
				return;
			}
		}
	}

	public void SetSFXVolume(float value)
	{
		Debug.Log("[AudioManager] SFX vol: " + value);
		audioMixer.SetFloat("SFXVolume", value);
	}

	public void SetMusicVolume(float value)
	{
		Debug.Log("[AudioManager] Music vol: " + value);
		audioMixer.SetFloat("MusicVolume", value);
	}

	/// <summary>
	/// Plays given Audio event once
	/// </summary>
	/// <param name="ae">Audio Event</param>
	/// <returns>Duration of the audio clip</returns>
	public float PlayOnce(AudioEvent ae)
	{
		if (!CanBePlayed(ae))
			return 0f;

		var s = GetAndSetupSFXSource(ae);
		s.loop = false;
		s.spatialBlend = 0f;

		if (ae.Delay <= 0f)
			s.Play();
		else
			s.PlayDelayed(ae.Delay);
			
		return s.clip.length / Mathf.Abs(s.pitch);
	}

	/// <summary>
	/// Play a looping sound at Camera position
	/// </summary>
	/// <param name="ae"></param>
	/// <param name="requester"></param>
	public void PlayLooping(AudioEvent ae, Transform requester)
	{
		if (loopingSounds.ContainsKey(requester))
			return;

		var s = GetAndSetupSFXSource(ae);
		s.loop = true;
		s.spatialBlend = 0f;
		s.transform.parent = mainCamTm;
		s.transform.localPosition = Vector3.zero;
		s.Play();
		loopingSounds.Add(requester, s);
	}

	public void StopLooping(Transform requester)
	{
		if (!loopingSounds.ContainsKey(requester))
			return;

		if (loopingSounds[requester] == null)
			return;

		loopingSounds[requester].transform.SetParent(sourceParent);
		loopingSounds[requester].Stop();
		loopingSounds.Remove(requester);
	}

	private AudioSource GetAndSetupSFXSource(AudioEvent ae)
	{
		var s = pool.Get();
		s.clip = ae.Clip;
		s.volume = ae.Volume;
		s.pitch = ae.Pitch;
		s.outputAudioMixerGroup = sfxAudioMixerGroup;
		return s;
	}

	private AudioSource CreateAudioSource()
	{
		var go = new GameObject("AudioSource");
		go.transform.SetParent(sourceParent);
		go.transform.localPosition = Vector3.zero;

		var source = go.AddComponent<AudioSource>();
		source.playOnAwake = false;
		source.spatialBlend = 1f;
		source.minDistance = 10f;
		source.maxDistance = 30f;
		return source;
	}

	private bool CanBePlayed(AudioEvent ae)
	{
		if (ae.MINInterval <= 0f)
			return true;

		if (cooldowns.TryGetValue(ae, out float endsAt) && Time.realtimeSinceStartup < endsAt)
			return false;

		cooldowns[ae] = Time.realtimeSinceStartup + ae.MINInterval;
		return true;
	}
}