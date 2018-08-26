using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum	LaserBehaviourType
{
	CircularLaser,
}

public class LaserBehaviour : MonoBehaviour
{
	public LaserBehaviourType	type;

	// Circular laser settings
	public float				radius;
	public float				rotationSpeed;

	Dictionary<LaserBehaviourType, Action>	behaviourUpdates;

	private void Start()
	{
		behaviourUpdates = new Dictionary<LaserBehaviourType, Action>
		{
			{LaserBehaviourType.CircularLaser, CircularLaserUpdate},
		};
	}

	void Update ()
	{
		behaviourUpdates[type]();
	}

	void CircularLaserUpdate()
	{
		
	}
}
