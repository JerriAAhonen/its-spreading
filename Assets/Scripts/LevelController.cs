using System;
using UnityEngine;

public class LevelController : MonoBehaviour
{
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
		enemies = GetComponentsInChildren<Enemy>();
		lampPosts = GetComponentsInChildren<LampPost>();
		foreach (LampPost lamp in lampPosts)
			lamp.Lit += OnLampLit;
		fireflies = GetComponentsInChildren<Fireflies>();
		foreach (Fireflies firefly in fireflies)
			firefly.Destroyed += OnFireflyDestroyed;
	}

	private void OnPlayerDied()
	{
		Debug.Log("Player Died");
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
