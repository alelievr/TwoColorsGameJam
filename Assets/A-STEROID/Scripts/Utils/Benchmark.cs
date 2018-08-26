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

	public int		_maxSimultaneousExplosions;
	public int		maxSimultaneousExplosions
	{
		get { return _maxSimultaneousExplosions; }
		set { _maxSimultaneousExplosions = Mathf.Max(_maxSimultaneousExplosions, value); }
	}

	public int		_maxSimultaneousDebriss;
	public int		maxSimultaneousDebriss
	{
		get { return _maxSimultaneousDebriss; }
		set { _maxSimultaneousDebriss = Mathf.Max(_maxSimultaneousDebriss, value); }
	}

	private void OnEnable()
	{
		instance = this;
	}
}
