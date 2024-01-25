using UnityEngine;

public abstract class GameState : MonoBehaviour
{
	protected GameStateManager manager;
	protected bool openedAdditively;

	public GameState Init(GameStateManager manager)
	{
		this.manager = manager;
		gameObject.SetActive(false);
		return this;
	}

	public virtual void Enter() { }
	public virtual void Exit() { }
	public virtual void OnFocusRestored() { }

	public void SetOpendedAdditively(bool additively)
	{
		openedAdditively = additively;
	}
}
