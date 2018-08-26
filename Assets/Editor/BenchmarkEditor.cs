using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Benchmark))]
public class BenchmarkEditor : Editor
{
	Benchmark	bench;

	private void OnEnable()
	{
		bench = target as Benchmark;
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		if (GUILayout.Button("Reset"))
		{
			bench._maxSimultaneousAudioSources = 0;
			bench._maxSimultaneousLasers = 0;
		}
	}
}