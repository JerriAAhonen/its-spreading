using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesController : MonoBehaviour
{
	public class TileData
	{
		public bool IsWalkable;
		public bool CanPushObstacleInto;

		public TileData(bool isWalkable, bool canPushObstacleInto)
		{
			IsWalkable = isWalkable;
			CanPushObstacleInto = canPushObstacleInto;
		}
	}

	[SerializeField] private bool DEBUG_showGridDebug;

	private readonly HashSet<Vector3Int> tileCoords = new();
	private readonly Dictionary<Vector3Int, TileObject> tiles = new();

	private Grid<TileData> grid;

	private void Awake()
	{
		float minX = float.MaxValue;
		float minZ = float.MaxValue;
		float maxX = float.MinValue;
		float maxZ = float.MinValue;

		foreach (Transform t in transform)
		{
			tileCoords.Add(t.position.ToVector3Int());
			tiles.Add(t.position.ToVector3Int(), t.gameObject.GetComponent<TileObject>());

			// Calculate map bounds
			if (t.position.x < minX) minX = t.position.x;
			if (t.position.x > maxX) maxX = t.position.x;
			if (t.position.z < minZ) minZ = t.position.z;
			if (t.position.z > maxZ) maxZ = t.position.z;
		}

		var gridSizeX = maxX - minX;
		var gridSizeZ = maxZ - minZ;
		
		// Add 2 to extend the grid 1 tile further from the edges for border padding
		gridSizeX += 2;
		gridSizeZ += 2;

		var gridSize = new Vector2Int((int)gridSizeX + 1, (int)gridSizeZ + 1);
		var origin = new Vector3(minX, 0f, minZ);
		
		// Move the origin one tile back to take into account the the extra border padding
		origin -= Vector3.one.With(y: 0f);
		
		var centerOffset = new Vector3(0.5f, 0f, 0.5f);
		origin -= centerOffset;
		grid = new Grid<TileData>(gridSize, 1f, origin, true, true, CreateTileData);
		grid.SetDebugData((TileData data) => data.IsWalkable ? "W" : data.CanPushObstacleInto ? "C" : "E");
	
		TileData CreateTileData(Vector2Int gridPos, Vector3 worldPos, int index)
		{
			if (tiles.TryGetValue(worldPos.ToVector3Int(), out TileObject tileObj))
			{
				return new TileData(tileObj.IsWalkable, tileObj.CanPushObstacleInto);
			}

			return new TileData(false, false);

		}
	}

	private void OnDrawGizmos()
	{
		if (DEBUG_showGridDebug)
			grid?.OnDrawGizmos_DrawDebugData();
	}

	public bool IsWalkable(Vector3 pos)
	{
		if (grid.GetValue(pos, out var tileObject, true))
		{
			return tileObject.IsWalkable;
		}

		return false;
	}
	
	public bool GetTileCenter(Vector3 worldPos, out Vector3 gridTileCenter)
	{
		return grid.GetGridAlignedPosition(worldPos, out gridTileCenter);
	}
}
