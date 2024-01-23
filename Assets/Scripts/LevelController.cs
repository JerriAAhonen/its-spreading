using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
	private PlayerController player;
	private LampPost[] lampPosts;

	private void Awake()
	{
		player = GetComponentInChildren<PlayerController>();
		player.Die += OnPlayerDied;
		lampPosts = GetComponentsInChildren<LampPost>();
		foreach (LampPost lamp in lampPosts)
			lamp.Lit += OnLampLit;
	}

	private void OnPlayerDied()
	{
		Debug.Log("Player Died");
	}

	private void OnLampLit()
	{
		if (AllLampsLit())
		{
			Debug.Log("All lamps lit");
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
