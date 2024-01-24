#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class GameState_MainMenu : GameState
{
	[SerializeField] private MenuButton startGame;
	[SerializeField] private MenuButton levels;
	[SerializeField] private MenuButton settings;
	[SerializeField] private MenuButton exit;

	private void Awake()
	{
		startGame.OnClick += OnStartGame;
		levels.OnClick += OnLevels;
		settings.OnClick += OnSettings;
		exit.OnClick += OnExitGame;
	}

	#region GameState

	public override void Enter() 
	{ 
		gameObject.SetActive(true);
	}

	public override void Exit() 
	{
		gameObject.SetActive(false);
	}

	#endregion

	private void OnStartGame()
	{
		Debug.Log("OnStartGame");
		manager.Transition(GameStateType.Level, true);
	}

	private void OnLevels()
	{
		Debug.Log("OnLevels");
		manager.Transition(GameStateType.LevelSelection, false);
	}

	private void OnSettings()
	{
		Debug.Log("OnSettings");
		manager.Transition(GameStateType.Settings, false);
	}

	private void OnExitGame()
	{
		Debug.Log("OnExit");
#if UNITY_EDITOR
		if (EditorApplication.isPlaying)
		{
			EditorApplication.isPlaying = false;
		}
#endif
		Application.Quit();

	}
}
