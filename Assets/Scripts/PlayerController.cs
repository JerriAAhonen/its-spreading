using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private TilesController tc;

	private InputController ic;
	private PlayerMovement movement;

	private bool hasFireflies;

	public bool HasFireflies => hasFireflies;

	private void Awake()
	{
		ic = GetComponent<InputController>();
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
