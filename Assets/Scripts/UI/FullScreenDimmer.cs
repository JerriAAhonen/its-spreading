using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenDimmer : MonoBehaviour
{
	[SerializeField] private Image dimm;
	[SerializeField] private float dur;

	public float Show(bool show)
	{
		LeanTween.value(dimm.color.a, show ? 1f : 0f, dur)
			.setOnUpdate(v => dimm.SetAlpha(v))
			.setIgnoreTimeScale(true);

		dimm.raycastTarget = show;

		return dur;
	}
}
