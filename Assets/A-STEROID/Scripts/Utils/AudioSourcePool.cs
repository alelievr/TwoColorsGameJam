using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSourcePool : GameObjectPool
{
	public static AudioSourcePool	instance;

	protected Queue<AudioSource>	audioSourcePool = null;

	protected override void Awake()
	{
		base.Awake();
		audioSourcePool = new Queue<AudioSource>();
		instance = this;
		AllocPool();
	}

	protected override void OnNewObjectReserved(GameObject go)
	{
		audioSourcePool.Enqueue(go.GetComponent<AudioSource>());
	}

	public AudioSource NewAudioSource(AudioClip clip, AudioMixerGroup mixerGroup, float volume = 1)
	{
		base.GetGameObject();

		AudioSource audio = audioSourcePool.Dequeue();

		audio.clip = clip;
		audio.outputAudioMixerGroup = mixerGroup;
		audio.volume = volume;

		Benchmark.instance.maxSimultaneousAudioSources = allocatedObjectCount;

		audio.gameObject.SetActive(true);

		return audio;
	}

	public void FreeAudioSource(AudioSource o)
	{
		FreeGameObject(o.gameObject);
		audioSourcePool.Enqueue(o);
	}
}
