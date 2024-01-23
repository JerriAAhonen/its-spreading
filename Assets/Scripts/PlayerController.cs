using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private TilesController tc;

	private IInputController ic;
	private PlayerMovement movement;
	private CapsuleCollider capsuleCollider;

	private bool hasFireflies;

	public TilesController TilesController => tc;

	public bool HasFireflies => hasFireflies;
	public float Width => capsuleCollider.radius * 2f;

	public event Action Die;

	private void Awake()
	{
		ic = GetComponent<IInputController>();
		movement = GetComponent<PlayerMovement>();
		movement.Init(tc, ic);
		capsuleCollider = GetComponent<CapsuleCollider>();
	}

	public void CollectFireflies()
	{
		hasFireflies = true;
	}

	public void DepositFireflies()
	{
		hasFireflies = false;
	}

	public void CollideWithEnemy()
	{
		Debug.Log("Dead");
		Die?.Invoke();
	}
}
