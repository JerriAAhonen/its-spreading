using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveDataManager
{
	private const string LastCompletedLevelKey = "LastCompletedLevel";

	public static int GetNextLevelIndex()
	{
		return PlayerPrefs.GetInt(LastCompletedLevelKey, 0);
	}

	public static void SetLevelCompleted(int index)
	{
		var lastCompletedLevelIndex = index.AtLeast(PlayerPrefs.GetInt(LastCompletedLevelKey, 0));
		PlayerPrefs.SetInt(LastCompletedLevelKey, lastCompletedLevelIndex);
	}
}
