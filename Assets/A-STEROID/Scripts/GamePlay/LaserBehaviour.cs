using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum	LaserBehaviourType
{
	Straight,
	CircularLaser,
}

public class LaserBehaviour : Projectile
{
	public LaserBehaviourType	type;

	// Circular laser settings
	public float				radius;
	public float				rotationSpeed;
	public float				speed;

	// Global params
	public float				startTime;

	float		currentTime;
	Vector3		startPosition;

	Dictionary<LaserBehaviourType, Action>	behaviourUpdates;
	protected override void Start()
	{
		behaviourUpdates = new Dictionary<LaserBehaviourType, Action>
		{
			{LaserBehaviourType.Straight, StraightUpdate},
			{LaserBehaviourType.CircularLaser, CircularLaserUpdate},
		};
	}

	public void OnLaserSpawned()
	{
		startPosition = transform.position;
		startTime = Time.time;
		AudioController.instance.PlayLaserAtPosition(startPosition);
	}

	void FixedUpdate ()
	{
		currentTime = Time.time - startTime;
		behaviourUpdates[type]();
	}

	void StraightUpdate()
	{
		transform.position = startPosition + transform.right * currentTime * speed;
	}

	void CircularLaserUpdate()
	{
		Vector2 circlePos = new Vector2(Mathf.Sin(currentTime * rotationSpeed), Mathf.Cos(currentTime * rotationSpeed)) * radius;
		transform.position = startPosition + transform.right * currentTime * speed + (Vector3)circlePos;
	}

	protected override void DestroyProjectile()
	{
		LaserPool.instance.FreeLaser(this);
	}
}
