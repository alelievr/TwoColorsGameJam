using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Benchmark : ScriptableObject
{
	public static Benchmark	instance;

	public int		_maxSimultaneousAudioSources;
	public int		maxSimultaneousAudioSources
	{
		get { return _maxSimultaneousAudioSources; }
		set { _maxSimultaneousAudioSources = Mathf.Max(_maxSimultaneousAudioSources, value); }
	}
	public int		_maxSimultaneousLasers;
	public int		maxSimultaneousLasers
	{
		get { return _maxSimultaneousLasers; }
		set { _maxSimultaneousLasers = Mathf.Max(_maxSimultaneousLasers, value); }
	}

	private void OnEnable()
	{
		instance = this;
	}
}
