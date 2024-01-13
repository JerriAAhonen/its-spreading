using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireflies : Interactable
{
	[SerializeField] private Material activeMat;
	[SerializeField] private Material deactiveMat;

	private bool active;

	private void Awake()
	{
		active = true;
	}

	protected override void OnInteract()
	{
		if (!active)
			return;
		active = false;

		GetComponent<MeshRenderer>().material = deactiveMat;
	}
}
