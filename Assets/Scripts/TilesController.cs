using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesController : MonoBehaviour
{
	private readonly HashSet<Vector3Int> tileCoords = new();

	private Grid<object> grid;

	private void Awake()
	{
		float minX = float.MaxValue; 
		float minZ = float.MaxValue;
		float maxX = float.MinValue;
		float maxZ = float.MinValue;
		
		foreach (Transform t in transform)
		{
			tileCoords.Add(new Vector3Int((int)t.position.x, (int)t.position.y, (int)t.position.z));
			if (t.position.x < minX) minX = t.position.x;
			if (t.position.x > maxX) maxX = t.position.x;
			if (t.position.z < minZ) minZ = t.position.z;
			if (t.position.z > maxZ) maxZ = t.position.z;
		}

		var gridSizeX = maxX - minX;
		var gridSizeZ = maxZ - minZ;
		var gridSize = new Vector2Int((int)gridSizeX + 1, (int)gridSizeZ + 1);
		var origin = new Vector3(minX, 0f, minZ);
		origin -= new Vector3(0.5f, 0f, 0.5f);
		grid = new Grid<object>(gridSize, 1f, origin, true, false, (Vector2Int gridPos, Vector3 worldPos, int index) => new object());
	}

	private void OnDrawGizmos()
	{
		grid?.OnDrawGizmos_DrawDebugData();
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
