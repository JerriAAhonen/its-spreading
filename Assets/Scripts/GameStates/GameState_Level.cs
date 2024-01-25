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

		levelInstance = Instantiate(LevelDatabase.Get().GetLevel(manager.CurrentLevelIndex).gameObject);
		levelInstance.transform.SetParent(transform);
		
		levelController = levelInstance.GetComponent<LevelController>();
		levelController.LevelFailed += OnLevelFailed;
		levelController.LevelCompleted += OnLevelCompleted;

		EnvironmentController.Instance.Transition(EnvState.night, true);

		ResumeTime();
	}

	public override void Exit()
	{
		EnvironmentController.Instance.Transition(EnvState.night, true);
		Destroy(levelInstance);
		gameObject.SetActive(false);
	}

	#endregion

	private void OnPause()
	{
		if (manager.IsStateOpen<GameState_Pause>())
		{
			manager.CloseTopState();
			ResumeTime();
		}
		else
		{
			StopTime();
			manager.OpenAdditive(GameStateType.Pause);
		}
	}

	private void OnLevelFailed()
	{
		HidePauseButton();
		manager.OpenAdditive(GameStateType.GameOver);
	}

	private void OnLevelCompleted()
	{
		HidePauseButton();
		manager.OnLevelCompleted();
	}

	public static void StopTime()
	{
		Time.timeScale = 0f;
	}

	public static void ResumeTime()
	{
		Time.timeScale = 1f;
	}

	private void HidePauseButton()
	{
		pauseButton.gameObject.SetActive(false);
	}
}
