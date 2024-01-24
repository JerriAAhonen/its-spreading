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

	public TileType Type => type;
	public Vector3Int WorldPos { get; private set; }

	private void Awake()
	{
		WorldPos = transform.position.ToVector3Int();
	}

	private void OnValidate()
	{
		gameObject.name = $"Tile-{type}";
	}
}
