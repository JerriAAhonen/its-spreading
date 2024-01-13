using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesController : MonoBehaviour
{
	private HashSet<Vector3Int> tileCoords = new();

	private void Awake()
	{
		foreach (Transform t in transform)
		{
			tileCoords.Add(new Vector3Int((int)t.position.x, (int)t.position.y, (int)t.position.z));
		}
	}

	public bool IsTile(Vector3Int pos)
	{
		return tileCoords.Contains(pos);
	}

	public bool IsTile(Vector3 pos)
	{
		return IsTile(new Vector3Int((int)pos.x, (int)pos.y, (int)pos.z));
	}
}
