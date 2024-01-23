using UnityEngine;

public class GameState_Level : GameState
{
	private GameObject levelInstance;

	public override void Enter()
	{
		// TODO: Load level
		var levelController = LevelDatabase.Get().GetLevel(manager.CurrentLevelIndex);
		levelInstance = Instantiate(levelController.gameObject);
		levelInstance.transform.SetParent(transform);
	}

	public override void Exit()
	{
		Destroy(levelInstance);
	}
}
