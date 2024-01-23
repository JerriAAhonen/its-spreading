#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class GameState_MainMenu : GameState
{

	public override void Enter() 
	{ 
		gameObject.SetActive(true);
	}

	public override void Exit() 
	{
		gameObject.SetActive(false);
	}

	private void Quit()
	{
#if UNITY_EDITOR
		if (EditorApplication.isPlaying)
		{
			EditorApplication.isPlaying = false;
		}
#endif
		Application.Quit();

	}
}
