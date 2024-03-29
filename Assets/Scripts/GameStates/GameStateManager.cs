using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum GameStateType { MainMenu, LevelSelection, Settings, Level, Pause, GameOver, ThanksForPlaying }

public class GameStateManager : Singleton<GameStateManager>
{
	[SerializeField] private GameState_MainMenu mainMenu;
	[SerializeField] private GameState_LevelSelection levelSelection;
	[SerializeField] private GameState_Settings settings;
	[SerializeField] private GameState_Level level;
	[SerializeField] private GameState_Pause pause;
	[SerializeField] private GameState_GameOver gameOver;
	[SerializeField] private GameState_ThanksForPlaying thanksForPlaying;
	[Space]
	[SerializeField] private FullScreenDimmer screenDimmer;

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
		thanksForPlaying.Init(this);

		CurrentLevelIndex = SaveDataManager.GetNextLevel();
		//Debug.Log("[StateManager] CurrentLevelIndex = " + CurrentLevelIndex);

		Transition(GameStateType.MainMenu, true);
	}

	public void Transition(GameStateType newType, bool useDimming)
	{
		StartCoroutine(Routine());

		IEnumerator Routine()
		{
			if (useDimming)
				yield return StartCoroutine(WaitForUtil.RealSeconds(screenDimmer.Show(true))); // Have to use this because the game can be paused and timescale set to 0f when transitioning between states

			while (states.Count > 0)
			{
				states.Peek().Exit();
				states.Peek().SetOpendedAdditively(false);
				states.Pop();
			}

			var state = GetState(newType);
			states.Push(state);
			state.Enter();

			if (useDimming)
				screenDimmer.Show(false);
		}
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

		states.Peek().OnFocusRestored();
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
		//Debug.Log("[StateManager] OnLevelCompleted " + CurrentLevelIndex);
		SaveDataManager.SetLevelCompleted(CurrentLevelIndex);
		CurrentLevelIndex++;

		//Debug.Log("[StateManager] SetNextLevel = " + CurrentLevelIndex);
		SaveDataManager.SetNextLevel(CurrentLevelIndex);

		//Debug.Log("[StateManager] CurrentLevelIndex = " + CurrentLevelIndex + ", MaxLevelIndex = " + LevelDatabase.Get().MaxLevelIndex);
		if (CurrentLevelIndex > LevelDatabase.Get().MaxLevelIndex)
		{
			//Debug.Log("Last level completed, thanks for playing!");
			
			CurrentLevelIndex = 0;
			SaveDataManager.SetNextLevel(CurrentLevelIndex);

			Transition(GameStateType.ThanksForPlaying, true);
			return;
		}

		Transition(GameStateType.Level, true);
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
			GameStateType.ThanksForPlaying => thanksForPlaying,
			_ => throw new System.NotImplementedException(),
		};
	}
}
