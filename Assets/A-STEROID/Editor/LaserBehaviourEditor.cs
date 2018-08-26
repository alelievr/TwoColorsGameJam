using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// [CustomEditor(typeof(LaserBehaviour))]
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
		laserBehaviourInspectors[laserBehaviour.type]();
	}

	void CircularLaserInspector()
	{
		// EditorGUILayout.
	}

	void StraightInspector()
	{

	}
}