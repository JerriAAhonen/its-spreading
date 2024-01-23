#if UNITY_EDITOR
#endif
public class GameState_GameOver : GameState
{
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
}
