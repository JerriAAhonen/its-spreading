using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private TilesController tc;
	[SerializeField] private LayerMask obstacleMask;
	[SerializeField] private float obstacleDetectionRadius;
	[SerializeField] private ParticleSystem lanternFF;
	[Header("Death")]
	[SerializeField] private float deathAnimDur;
	[SerializeField] private ParticleSystem deathPS;
	[SerializeField] private GameObject bodyModel;
	[SerializeField] private GameObject lantern;
	[SerializeField] private HingeJoint lanternHinge;
	[SerializeField] private MeshRenderer lampRenderer;
	[SerializeField] private Material glassUnlit;
	[SerializeField] private Light lanternLight;
	[SerializeField] private ParticleSystem escapingFF;

	private IInputController ic;
	private PlayerMovement movement;
	private CapsuleCollider capsuleCollider;

	private bool alive = true;
	private bool hasFireflies;

	private Material lampMat1;

	public TilesController TilesController => tc;

	public bool MovementEnabled { get; private set; }
	public bool HasFireflies => hasFireflies;
	public float Width => capsuleCollider.radius * 2f;

	public event Action Die;

	private void Awake()
	{
		ic = GetComponent<IInputController>();
		movement = GetComponent<PlayerMovement>();
		movement.Init(ic, this);
		capsuleCollider = GetComponent<CapsuleCollider>();
		MovementEnabled = true;

		lanternFF.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		lampMat1 = lampRenderer.materials[0];
	}

	private Obstacle currentlySelectedObstacle;

	private void Update()
	{
		Collider closestObstacle = null;
		var pos = transform.position;
		var obstacles = Physics.OverlapSphere(transform.position, obstacleDetectionRadius, obstacleMask);
		if (!obstacles.IsNullOrEmpty())
		{
			float closestDistance = Mathf.Infinity;
			foreach (var obstacleCol in obstacles)
			{
				// Calculate the distance from the center of the sphere to the collider
				float distance = Vector3.Distance(pos, obstacleCol.transform.position);

				// If this distance is smaller than the currently stored minimum distance, update the closest collider and distance
				if (distance < closestDistance)
				{
					closestObstacle = obstacleCol;
					closestDistance = distance;
				}
			}

			var obstacle = closestObstacle.GetComponent<Obstacle>();
			UpdateClosestObstacle(obstacle);
		}
		else
			UpdateClosestObstacle(null);
	}

	public void UpdateClosestObstacle(Obstacle closestObstacle)
	{
		// No obstacles anywhere near
		if (closestObstacle == null && currentlySelectedObstacle == null)
		{
			return;
		}

		// We are no longer near an obstacle
		if (closestObstacle == null && currentlySelectedObstacle != null)
		{
			currentlySelectedObstacle.ActivateOutline(false);
			currentlySelectedObstacle = null;
			return;
		}

		if (closestObstacle != currentlySelectedObstacle)
		{
			if (currentlySelectedObstacle != null)
				currentlySelectedObstacle.ActivateOutline(false);
			
			currentlySelectedObstacle = closestObstacle;
			currentlySelectedObstacle.ActivateOutline(true);
		}
	}

	public void CollectFireflies()
	{
		hasFireflies = true;
		lanternFF.Play();
	}

	public void DepositFireflies()
	{
		hasFireflies = false;
		lanternFF.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
	}

	public void CollideWithEnemy()
	{
		if (!alive) return;
		alive = false;

		Debug.Log("Dead");
		DisableControls();
		AnimateDeath();
		LeanTween.delayedCall(deathAnimDur, () => 
		{ 
			Die?.Invoke();
		});
	}

	private void DisableControls()
	{
		MovementEnabled = false;
	}

	private void AnimateDeath()
	{
		bodyModel.SetActive(false);
		deathPS.Play();
		Destroy(lanternHinge);
		lantern.transform.SetParent(transform.parent);


		lampRenderer.materials = new Material[]
			{
				lampMat1, glassUnlit
			};

		LeanTween.value(lanternLight.intensity, 0f, deathAnimDur)
			.setOnUpdate(v => lanternLight.intensity = v);

		// Animate fireflies escaping
		if (hasFireflies)
		{
			lanternFF.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
			escapingFF.Play();
		}
	}
}
