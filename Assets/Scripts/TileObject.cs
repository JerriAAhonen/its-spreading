using NaughtyAttributes;
using UnityEngine;

public enum TileType
{
	Empty, Solid, Water, Obstructed
}

[DefaultExecutionOrder(-99)]
public class TileObject : MonoBehaviour
{
	[SerializeField] TileType type;
	[SerializeField] private GameObject grass;
	[SerializeField] private GameObject water;

	public TileType Type => type;
	public Vector3Int WorldPos { get; private set; }

	private void Awake()
	{
		WorldPos = transform.position.ToVector3Int();
	}

	[Button]
	private void EDITOR_SetGrass()
	{
		type = TileType.Solid;
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
