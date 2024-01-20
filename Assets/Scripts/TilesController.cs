using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesController : MonoBehaviour
{
	public class TileObject
	{
		public bool IsWalkable;

		public TileObject(bool isWalkable)
		{
			IsWalkable = isWalkable;
		}
	}

	private readonly HashSet<Vector3Int> tileCoords = new();

	private Grid<TileObject> grid;

	private void Awake()
	{
		float minX = float.MaxValue;
		float minZ = float.MaxValue;
		float maxX = float.MinValue;
		float maxZ = float.MinValue;

		foreach (Transform t in transform)
		{
			tileCoords.Add(t.position.ToVector3Int());
			if (t.position.x < minX) minX = t.position.x;
			if (t.position.x > maxX) maxX = t.position.x;
			if (t.position.z < minZ) minZ = t.position.z;
			if (t.position.z > maxZ) maxZ = t.position.z;
		}

		var gridSizeX = maxX - minX;
		var gridSizeZ = maxZ - minZ;
		var gridSize = new Vector2Int((int)gridSizeX + 1, (int)gridSizeZ + 1);
		var origin = new Vector3(minX, 0f, minZ);
		var centerOffset = new Vector3(0.5f, 0f, 0.5f);
		origin -= centerOffset;
		grid = new Grid<TileObject>(gridSize, 1f, origin, true, true, (Vector2Int gridPos, Vector3 worldPos, int index) => new TileObject(tileCoords.Contains(worldPos.ToVector3Int())));
		grid.SetDebugData((TileObject tileObj) => tileObj.IsWalkable ? "W" : "E");
	}

	private void OnDrawGizmos()
	{
		grid?.OnDrawGizmos_DrawDebugData();
	}

	public bool IsWalkable(Vector3 pos)
	{
		if (grid.GetValue(pos, out var tileObject))
		{
			return tileObject.IsWalkable;
		}

		return false;
	}
}
