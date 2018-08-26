using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AudioController))]
public class AudioControllerEditor : Editor
{
	AudioController	controller;

	private void OnEnable()
	{
		controller = target as AudioController;
	}

	void OnSceneGUI()
	{
		Handles.color = Color.green;
		foreach (var bossZone in controller.bosses)
			Handles.CircleHandleCap(0, bossZone.transform.position, Quaternion.identity, controller.fadeDistance + bossZone.radius, EventType.Repaint);
	}
}
