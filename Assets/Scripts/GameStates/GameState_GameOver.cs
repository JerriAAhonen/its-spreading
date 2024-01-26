using UnityEngine;

public class GameState_GameOver : GameState
{
	[SerializeField] private MenuButton tryAgain;
	[SerializeField] private MenuButton mainMenu;
	[SerializeField] private CanvasGroup cg;

	private int? tweenId;

	private void Awake()
	{
		tryAgain.OnClick += OnTryAgain;
		mainMenu.OnClick += OnMainMenu;
		cg.alpha = 0f;
	}

	#region GameState

	public override void Enter()
	{
		cg.alpha = 0f;
		gameObject.SetActive(true);
		
		tweenId = LeanTween.value(0f, 1f, 1f).setOnUpdate(v => cg.alpha = v).uniqueId;
	}

	public override void Exit()
	{
		gameObject.SetActive(false);
		if (tweenId.HasValue) LeanTween.cancel(tweenId.Value);
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
