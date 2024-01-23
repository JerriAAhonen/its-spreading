using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum GameStateType { MainMenu, LevelSelection, Settings, Level, Pause, GameOver }

public class GameStateManager : Singleton<GameStateManager>
{
	[SerializeField] private GameState_MainMenu mainMenu;
	[SerializeField] private GameState_LevelSelection levelSelection;
	[SerializeField] private GameState_Settings settings;
	[SerializeField] private GameState_Level level;
	[SerializeField] private GameState_Pause pause;
	[SerializeField] private GameState_GameOver gameOver;

	private readonly Stack<GameState> states = new();

	public int CurrentLevelIndex { get; set; }

	protected override void Awake()
	{
		base.Awake();

		mainMenu.Init(this);
		levelSelection.Init(this);
		settings.Init(this);
		level.Init(this);
		pause.Init(this);
		gameOver.Init(this);

		Transition(GameStateType.MainMenu);
	}

	public void Transition(GameStateType newType)
	{
		while (states.Count > 0) 
		{
			states.Peek().Exit();
			states.Peek().SetOpendedAdditively(false);
			states.Pop();
		}

		var state = GetState(newType);
		states.Push(state);
		state.Enter();
	}

	public void OpenAdditive(GameStateType additiveType)
	{
		var state = GetState(additiveType);
		states.Push(state);
		state.Enter();
		state.SetOpendedAdditively(true);
	}

	public void CloseTopState()
	{
		if (states.Count > 0)
		{
			states.Peek().Exit();
			states.Peek().SetOpendedAdditively(false);
			states.Pop();
		}
	}

	public bool IsStateOpen<T>() where T : GameState
	{
		foreach (var state in states)
			if (state is T)
				return true;
		return false;
	}

	public void OnLevelCompleted()
	{
		SaveDataManager.SetLevelCompleted(CurrentLevelIndex);

		CurrentLevelIndex++;
		if (CurrentLevelIndex > LevelDatabase.Get().MaxLevelIndex)
		{
			Debug.Log("Last level completed, thanks for playing!");
			Transition(GameStateType.MainMenu);
			// TODO: Transition to Thanks for Playing screen
			return;
		}

		Transition(GameStateType.Level);
	}

	private GameState GetState(GameStateType type)
	{
		return type switch
		{
			GameStateType.MainMenu => mainMenu,
			GameStateType.LevelSelection => levelSelection,
			GameStateType.Settings => settings,
			GameStateType.Level => level,
			GameStateType.Pause => pause,
			GameStateType.GameOver => gameOver,
			_ => throw new System.NotImplementedException(),
		};
	}
}
