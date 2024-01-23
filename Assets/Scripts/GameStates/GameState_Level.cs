using UnityEngine;

public class GameState_Level : GameState
{
	private GameObject levelInstance;
	private LevelController levelController;

	#region GameState

	public override void Enter()
	{
		gameObject.SetActive(true);
		levelController = LevelDatabase.Get().GetLevel(manager.CurrentLevelIndex);
		levelInstance = Instantiate(levelController.gameObject);
		levelInstance.transform.SetParent(transform);
	}

	public override void Exit()
	{
		Destroy(levelInstance);
		gameObject.SetActive(false);
	}

	#endregion
}
