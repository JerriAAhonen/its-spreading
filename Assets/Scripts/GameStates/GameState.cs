using UnityEngine;

public abstract class GameState : MonoBehaviour
{
	protected GameStateManager manager;

	public GameState Init(GameStateManager manager)
	{
		this.manager = manager;
		gameObject.SetActive(false);
		return this;
	}

	public virtual void Enter() { }
	public virtual void Exit() { }
}
