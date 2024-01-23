using UnityEngine;

public class GameState_Level : GameState
{
	[SerializeField] private PauseButton pauseButton;

	private GameObject levelInstance;
	private LevelController levelController;

	private void Awake()
	{
		pauseButton.OnClick += OnPause;
	}

	#region GameState

	public override void Enter()
	{
		gameObject.SetActive(true);

		levelController = LevelDatabase.Get().GetLevel(manager.CurrentLevelIndex);
		levelController.LevelFailed += OnLevelFailed;
		levelController.LevelCompleted += OnLevelCompleted;

		levelInstance = Instantiate(levelController.gameObject);
		levelInstance.transform.SetParent(transform);
	}

	public override void Exit()
	{
		Destroy(levelInstance);
		gameObject.SetActive(false);
	}

	#endregion

	private void OnPause()
	{
		if (manager.IsStateOpen<GameState_Pause>())
			manager.CloseTopState();
		else
			manager.OpenAdditive(GameStateType.Pause);
	}

	private void OnLevelFailed()
	{
		manager.OpenAdditive(GameStateType.GameOver);
	}

	private void OnLevelCompleted()
	{
		manager.OnLevelCompleted();
	}
}
