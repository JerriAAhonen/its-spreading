using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LevelDatabase : ScriptableObject
{
	[SerializeField] private List<GameObject> levels;

	public GameObject GetLevel(int index)
	{
		if (index > levels.Count - 1)
		{
			Debug.LogError($"No level with index {index} exists, last index: {levels.Count - 1}");
			return null;
		}
		if (index < 0)
		{
			Debug.LogError($"No level with index {index} exists, first index is 0 :-D");
			return null;
		}

		return levels[index];
	}

	private static LevelDatabase db;

	public static LevelDatabase Get()
	{
		if (db == null)
			db = Resources.Load<LevelDatabase>("LevelDatabase");
		return db;
	}
}
