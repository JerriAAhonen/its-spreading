using UnityEngine;

public class GameState_GameOver : GameState
{
	[SerializeField] private MenuButton tryAgain;
	[SerializeField] private MenuButton mainMenu;

	private void Awake()
	{
		tryAgain.OnClick += OnTryAgain;
		mainMenu.OnClick += OnMainMenu;
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

	private void OnTryAgain()
	{
		manager.Transition(GameStateType.Level, true);
	}

	private void OnMainMenu()
	{
		manager.Transition(GameStateType.MainMenu, true);
	}
}
