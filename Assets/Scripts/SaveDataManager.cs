using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveDataManager
{
	private const string LastCompletedLevelKey = "SaveDataManager_LastCompletedLevel";
	private const string NextLevelKey = "SaveDataManager_NextLevel";

	public static int GetLastCompletedLevel()
	{
		return PlayerPrefs.GetInt(LastCompletedLevelKey, -1);
	}

	public static void SetLevelCompleted(int index)
	{
		var lastCompletedLevelIndex = index.AtLeast(PlayerPrefs.GetInt(LastCompletedLevelKey, -1));
		PlayerPrefs.SetInt(LastCompletedLevelKey, lastCompletedLevelIndex);
	}

	public static int GetNextLevel()
	{
		return PlayerPrefs.GetInt(NextLevelKey, 0);
	}

	public static void SetNextLevel(int index)
	{
		PlayerPrefs.SetInt(NextLevelKey, index);
	}
}
