using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DebritManager))]
public class DebritManagerEditor : Editor
{
	DebritManager manager;

	private void OnEnable()
	{
		manager = target as DebritManager;
	}

	public void OnSceneGUI()
	{
		for (int i = 0; i < manager.debritCount; i++)
		{
			Vector3 position = manager.GetDebritPosition(i);
			Handles.DotHandleCap(0, position, Quaternion.identity, .1f, EventType.Repaint);
			Handles.Label(position, i.ToString());
		}
	}
}
