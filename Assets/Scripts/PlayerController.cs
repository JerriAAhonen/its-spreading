using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private TilesController tc;

	private IInputController ic;
	private PlayerMovement movement;

	private bool hasFireflies;

	public bool HasFireflies => hasFireflies;

	private void Awake()
	{
		ic = GetComponent<IInputController>();
		movement = GetComponent<PlayerMovement>();
		movement.Init(tc, ic);
	}

	public void CollectFireflies()
	{
		hasFireflies = true;
	}

	public void DepositFireflies()
	{
		hasFireflies = false;
	}

	
}
