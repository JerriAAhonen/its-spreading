using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public enum GameStateType { MainMenu, Settings, Level }

public class GameStateManager : Singleton<GameStateManager>
{
	[SerializeField] private GameState_MainMenu mainMenu;
	[SerializeField] private GameState_Level level;

	private readonly Stack<GameState> states = new();

	public int CurrentLevelIndex { get; private set; }

	protected override void Awake()
	{
		base.Awake();

		mainMenu.Init(this);
		level.Init(this);

		Transition(GameStateType.MainMenu);
	}

	public void Transition(GameStateType newType)
	{
		if (states.Count > 0)
		{
			states.Peek().Exit();
			states.Pop();
		}

		var state = GetState(newType);
		states.Push(state);
		state.Enter();
	}

	private GameState GetState(GameStateType type)
	{
		return type switch
		{
			GameStateType.MainMenu => mainMenu,
			GameStateType.Level => level,
			GameStateType.Settings => throw new System.NotImplementedException(),
			_ => throw new System.NotImplementedException(),
		};
	}
}
