#if UNITY_EDITOR
#endif
public class GameState_Pause : GameState
{
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
}
