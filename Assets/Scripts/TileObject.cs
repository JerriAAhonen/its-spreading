using NaughtyAttributes;
using UnityEngine;

public enum TileType
{
	Grass, Water, Obstructed
}

[DefaultExecutionOrder(-99)]
public class TileObject : MonoBehaviour
{
	[SerializeField] TileType type;
	[SerializeField] private GameObject grass;
	[SerializeField] private GameObject water;

	public bool IsWalkable => type == TileType.Grass;
	public bool CanPushObstacleInto => type == TileType.Water;
	public Vector3Int WorldPos { get; private set; }

	private void Awake()
	{
		WorldPos = transform.position.ToVector3Int();
	}

	[Button]
	private void EDITOR_SetGrass()
	{
		type = TileType.Grass;
		grass.SetActive(true);
		water.SetActive(false);
	}

	[Button]
	private void EDITOR_SetWater()
	{
		type = TileType.Water;
		grass.SetActive(false);
		water.SetActive(true);
	}
}
