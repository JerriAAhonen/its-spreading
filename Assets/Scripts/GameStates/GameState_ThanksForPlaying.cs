using UnityEngine;

public class GameState_ThanksForPlaying : GameState
{
	[SerializeField] private MenuButton mainMenu;

	private void Awake()
	{
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

	private void OnMainMenu()
	{
		manager.Transition(GameStateType.MainMenu, true);
	}
}
