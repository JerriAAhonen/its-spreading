using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public enum GameState
{
	MainMenu,
	LevelSelection,
	Settings,
	InLevel
}

public class GameStateManager : Singleton<GameStateManager>
{
	private GameObject levelInstance;

	protected override void Awake()
	{
		base.Awake();
		OpenMainMenu();
	}

	public void OpenMainMenu()
	{

	}

	public void StartLevel()
	{
		var levelIndex = SaveDataManager.GetNextLevelIndex();
		var levelPrefab = LevelDatabase.Get().GetLevel(levelIndex);
		levelInstance = Instantiate(levelPrefab);
	}

	public void StartLevel(int levelIndex)
	{
		var levelPrefab = LevelDatabase.Get().GetLevel(levelIndex);
		levelInstance = Instantiate(levelPrefab);
	}

	public void ExitLevel()
	{
		Destroy(levelInstance);
	}

	public void OpenLevelSelect()
	{

	}

	public void OpenSettings()
	{

	}

	public void Quit()
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
