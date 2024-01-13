using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : Interactable
{
	[SerializeField] private Material activeMat;
	[SerializeField] private Material deactiveMat;

	private bool active;

	private void Awake()
	{
		active = false;
	}

	protected override void OnInteract()
	{
		if (active)
			return;
		active = true;

		GetComponent<MeshRenderer>().material = activeMat;
	}
}
