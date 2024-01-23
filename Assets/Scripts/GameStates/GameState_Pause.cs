using UnityEngine;

public class GameState_Pause : GameState
{
	[SerializeField] private MenuButton continu;
	[SerializeField] private MenuButton settings;
	[SerializeField] private MenuButton mainMenu;

	private void Awake()
	{
		continu.OnClick += OnContinue;
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
		manager.CloseTopState();
	}

	private void OnSettings()
	{
		manager.OpenAdditive(GameStateType.Settings);
	}

	private void OnMainMenu()
	{
		manager.Transition(GameStateType.MainMenu);
	}
}
