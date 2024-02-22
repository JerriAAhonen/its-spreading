using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum EnvState { night, day }

public class EnvironmentController : Singleton<EnvironmentController>
{
	[Header("Camera Background")]
	[SerializeField] private Camera backgroundCamera;
	[SerializeField] private Color cameraBgNightColor;
	[SerializeField] private Color cameraBgDayColor;
	[Space]
	[SerializeField] private Image backgroundTrees;
	[SerializeField] private Color treesNightColor;
	[SerializeField] private Color treesDayColor;
	[Header("Env lighting")]
	[ColorUsage(false, true)]
	[SerializeField] private Color envNightColor;
	[ColorUsage(false, true)]
	[SerializeField] private Color envDayColor;
	[Header("Settings")]
	[SerializeField] private float transitionDur;

	private Coroutine transitionRoutine;

	public float Transition(EnvState state, bool instant)
	{
		var fromBgColor = backgroundCamera.backgroundColor;
		var toBgColor = state == EnvState.night ? cameraBgNightColor : cameraBgDayColor;

		var fromTreeColor = backgroundTrees.color;
		var toTreeColor = state == EnvState.night ? treesNightColor : treesDayColor;

		var fromEnvColor = RenderSettings.ambientLight;
		var toEnvColor = state == EnvState.night ? envNightColor : envDayColor;

		if (instant)
		{
			Set();
		}
		else
		{
			if (transitionRoutine != null)
			{
				StopCoroutine(transitionRoutine);
				transitionRoutine = null;
			}

			transitionRoutine = StartCoroutine(Routine());
		}

		return instant ? 0f : transitionDur;

		IEnumerator Routine()
		{
			float elapsed = 0f;
			while (elapsed < transitionDur)
			{
				var step = elapsed / transitionDur;

				backgroundCamera.backgroundColor = Color.Lerp(fromBgColor, toBgColor, step);
				backgroundTrees.color = Color.Lerp(fromTreeColor, toTreeColor, step);
				RenderSettings.ambientLight = Color.Lerp(fromEnvColor, toEnvColor, step);

				elapsed += Time.deltaTime;
				yield return null;
			}

			Set();
		}

		void Set()
		{
			if (transitionRoutine != null)
			{
				StopCoroutine(transitionRoutine);
				transitionRoutine = null;
			}

			backgroundCamera.backgroundColor = toBgColor;
			backgroundTrees.color = toTreeColor;
			RenderSettings.ambientLight = toEnvColor;
		}
	}

}
