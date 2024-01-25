using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
	

	protected abstract void OnInteract(PlayerController pc);
}
