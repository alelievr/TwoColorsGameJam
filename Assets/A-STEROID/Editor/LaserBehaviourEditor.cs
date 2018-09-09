using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LaserBehaviour))]
public class LaserBehaviourEditor : Editor
{
	LaserBehaviour	laserBehaviour;
	Dictionary<LaserBehaviourType, Action>	laserBehaviourInspectors;

	private void OnEnable()
	{
		laserBehaviour = target as LaserBehaviour;
		laserBehaviourInspectors = new Dictionary<LaserBehaviourType, Action>
		{
			{LaserBehaviourType.Straight, StraightInspector},
			{LaserBehaviourType.CircularLaser, CircularLaserInspector}
		};
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		laserBehaviourInspectors[laserBehaviour.type]();
		if (GUILayout.Button("Sync with sprite"))
		{
			var spriteRenderer = laserBehaviour.GetComponent<SpriteRenderer>();
			var boxCollider = laserBehaviour.GetComponent<BoxCollider2D>();
			var circleCollider = laserBehaviour.GetComponent<CircleCollider2D>();
			laserBehaviour.laserSprite = spriteRenderer.sprite;
			laserBehaviour.laserColor = spriteRenderer.color;
			laserBehaviour.spawnScale = laserBehaviour.transform.localScale;
			laserBehaviour.useCircleCollider = circleCollider != null && circleCollider.enabled;
			laserBehaviour.circleColliderOffset = circleCollider.offset;
			laserBehaviour.boxColliderOffset = boxCollider.offset;
			laserBehaviour.colliderSize = boxCollider.size;
			laserBehaviour.colliderRadius = circleCollider.radius;
		}
	}

	void CircularLaserInspector()
	{
	}

	void StraightInspector()
	{

	}
}