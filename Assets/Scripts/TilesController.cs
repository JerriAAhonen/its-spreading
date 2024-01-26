using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesController : MonoBehaviour
{
	public class TileData
	{
		public TileType TileType;
		public Vector3 WorldPos;

		public TileData(TileType type, Vector3 worldPos)
		{
			TileType = type;
			WorldPos = worldPos;
		}
	}

	[SerializeField] private bool DEBUG_showGridDebug;
	[SerializeField] private PhysicMaterial wallPhysicMat;

	private readonly HashSet<Vector3Int> tileCoords = new();
	private readonly Dictionary<Vector3Int, TileObject> tiles = new();
	private readonly Dictionary<Vector3Int, Collider> colliders = new();

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
		grid = new Grid<TileData>(gridSize, 1f, origin, true, DEBUG_showGridDebug, CreateTileData);
		GenerateColliders();

		#region GRID_DEBUG

		if (DEBUG_showGridDebug)
		{
			grid.SetDebugData((TileData data) =>
			{
				return data.TileType switch
				{
					TileType.Empty => "E",
					TileType.Solid => "S",
					TileType.Water => "W",
					TileType.Obstructed => "O",
					_ => throw new System.NotImplementedException(),
				};
			});
		}

		#endregion

		TileData CreateTileData(Vector2Int gridPos, Vector3 worldPos, int index)
		{
			if (tiles.TryGetValue(worldPos.ToVector3Int(), out TileObject tileObj))
			{
				return new TileData(tileObj.Type, worldPos);
			}

			return new TileData(TileType.Empty, worldPos);
		}

		void GenerateColliders()
		{
			foreach (TileData data in grid)
			{
				switch (data.TileType)
				{
					case TileType.Empty:
					case TileType.Obstructed:
						// Create collider for player
						// Create collider for obstacle
						var colGo = new GameObject("Collider");
						colGo.transform.parent = transform;
						colGo.transform.position = data.WorldPos;
						colGo.layer = 11;									// <-- Set Collider layer
						var col = colGo.AddComponent<BoxCollider>();
						col.material = wallPhysicMat;
						colliders.Add(data.WorldPos.ToVector3Int(), col);
						break;
					case TileType.Solid: 
						break;
					case TileType.Water:
						// Create collider for player
						colGo = new GameObject("Collider");
						colGo.transform.parent = transform;
						colGo.transform.position = data.WorldPos;
						colGo.layer = 10;                                   // <-- Set Collider layer
						col = colGo.AddComponent<BoxCollider>();
						col.material = wallPhysicMat;
						colliders.Add(data.WorldPos.ToVector3Int(), col);
						break;
				}
			}
		}
	}

	private void OnDrawGizmos()
	{
		if (DEBUG_showGridDebug)
			grid?.OnDrawGizmos_DrawDebugData();
	}

	public void UpdateGrid_NewSolid(Vector3 worldPos)
	{
		if (colliders.TryGetValue(worldPos.ToVector3Int(), out var col))
		{
			col.gameObject.SetActive(false);
		}

		grid.SetValue(worldPos, new TileData(TileType.Solid, worldPos));
	}

	public void UpdateGrid_HideTile(Vector3 worldPos)
	{
		if (tiles.TryGetValue(worldPos.ToVector3Int(), out TileObject tileObj))
		{
			tileObj.gameObject.SetActive(false);
		}
	}

	public bool CanPushObstacleInto(Vector3 pos, out bool isWater)
	{
		if (grid.GetValue(pos, out var tileObject, true))
		{
			isWater = tileObject.TileType == TileType.Water;
			return tileObject.TileType is TileType.Water or TileType.Solid;
		}

		isWater = false;
		return false;
	}
	
	public bool GetTileCenter(Vector3 worldPos, out Vector3 gridTileCenter)
	{
		return grid.GetGridAlignedPosition(worldPos, out gridTileCenter);
	}
}
