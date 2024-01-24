using UnityEngine;

public class GameState_Pause : GameState
{
	[SerializeField] private MenuButton continu;
	[SerializeField] private MenuButton retry;
	[SerializeField] private MenuButton settings;
	[SerializeField] private MenuButton mainMenu;

	private void Awake()
	{
		continu.OnClick += OnContinue;
		retry.OnClick += OnRetry;
		settings.OnClick += OnSettings;
		mainMenu.OnClick += OnMainMenu;
	}

	#region GameState

	public override void Enter()
	{
		gameObject.SetActive(true);

		// TODO: Pause Time
	}

	public override void Exit()
	{
		gameObject.SetActive(false);
		// TODO: Resume Time
	}

	#endregion

	private void OnContinue()
	{
		GameState_Level.ResumeTime();
		manager.CloseTopState();
	}

	private void OnRetry()
	{
		manager.Transition(GameStateType.Level);
	}

	private void OnSettings()
	{
		manager.OpenAdditive(GameStateType.Settings);
	}

	private void OnMainMenu()
	{
		GameState_Level.ResumeTime();
		manager.Transition(GameStateType.MainMenu);
	}
}
