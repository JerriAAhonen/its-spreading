using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.TryGetComponent<PlayerController>(out var pc))
			OnInteract(pc);
	}

	protected abstract void OnInteract(PlayerController pc);
}
