using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Benchmark : ScriptableObject
{
	static Benchmark		_instance;
	public static Benchmark	instance
	{
		get
		{
			if (_instance == null)
				_instance = Resources.Load<Benchmark>("Benchmark");
			return _instance;
		}
		set { _instance = value; }
	}

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

	private void OnEnable()
	{
		instance = this;
	}
}
