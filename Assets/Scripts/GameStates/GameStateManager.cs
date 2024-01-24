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

		Transition(GameStateType.MainMenu, true);
	}

	public void Transition(GameStateType newType, bool useDimming)
	{
		StartCoroutine(Routine());

		IEnumerator Routine()
		{
			if (useDimming)
				yield return StartCoroutine(WaitForRealSeconds(screenDimmer.Show(true)));

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

		// Have to use this because the game can be paused and timescale set to 0f when transitioning between states
		IEnumerator WaitForRealSeconds(float seconds)
		{
			float timeRemaining = seconds;

			while (timeRemaining > 0)
			{
				timeRemaining -= Time.unscaledDeltaTime;
				yield return null;
			}
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
			Transition(GameStateType.ThanksForPlaying, true);
			CurrentLevelIndex = 0;
			// TODO: Transition to Thanks for Playing screen
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
