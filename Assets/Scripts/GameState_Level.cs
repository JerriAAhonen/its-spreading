using UnityEngine;

public class GameState_Level : GameState
{
	private GameObject levelInstance;

	public override void Enter()
	{
		// TODO: Load level
		levelInstance = Instantiate(LevelDatabase.Get().GetLevel(manager.CurrentLevelIndex));
		levelInstance.transform.SetParent(transform);
	}

	public override void Exit()
	{
		Destroy(levelInstance);
	}
}
