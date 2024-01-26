using System;
using UnityEngine;

public class LevelController : MonoBehaviour
{
	[SerializeField] private string tutorialText;
	[Space]
	[SerializeField] private GameObject playerFollowCam;
	[SerializeField] private GameObject playerDeathCam;

	private PlayerController player;
	private Enemy[] enemies;
	private LampPost[] lampPosts;
	private Fireflies[] fireflies;

	public event Action LevelFailed;
	public event Action LevelCompleted;

	private void Awake()
	{
		player = GetComponentInChildren<PlayerController>();
		player.Die += OnPlayerDied;
		player.DeathAnimComplete += OnPlayerDeathAnimComplete;

		enemies = GetComponentsInChildren<Enemy>();

		lampPosts = GetComponentsInChildren<LampPost>();
		foreach (LampPost lamp in lampPosts)
			lamp.Lit += OnLampLit;
		
		fireflies = GetComponentsInChildren<Fireflies>();
		foreach (Fireflies firefly in fireflies)
			firefly.Destroyed += OnFireflyDestroyed;

		playerFollowCam.SetActive(true); 
		playerDeathCam.SetActive(false);
	}

	private void Start()
	{
		player.ShowTutorialText(tutorialText);
	}

	private void OnPlayerDied()
	{
		Debug.Log("Player Died");

		playerFollowCam.SetActive(false);
		playerDeathCam.SetActive(true);
	}

	private void OnPlayerDeathAnimComplete()
	{
		LevelFailed?.Invoke();
	}

	private void OnFireflyDestroyed()
	{
		Debug.Log("Player Died");
		LevelFailed?.Invoke();
	}

	private void OnLampLit()
	{
		if (AllLampsLit())
		{
			Debug.Log("All lamps lit, show end animations");

			foreach (Enemy enemy in enemies)
				enemy.Die();

			var waitFor = EnvironmentController.Instance.Transition(EnvState.day, false);

			LeanTween.delayedCall(waitFor, () =>
			{
				if (this != null)
					LevelCompleted?.Invoke();
			});
		}
	}

	private bool AllLampsLit()
	{
		foreach(var lamp in lampPosts)
			if (!lamp.IsLit)
				return false;
		return true;
	}
}
