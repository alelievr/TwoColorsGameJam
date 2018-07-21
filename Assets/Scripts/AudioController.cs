using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
	public AudioMixer		mixer;

	[Header("Audio fade settings")]
	public float			fadeDistance = 40;

	[Header("Boss zones")]
	public BossZone[]		bosses;

	[Header("Audio sources")]
	public AudioSource		bossAudioStart;
	public AudioSource		bossAudioLoop;

	public static AudioController	instance;

	const string			backgroundVolume = "BackgroundVolume";

	private void Awake()
	{
		instance = this;
	}

	public void StopBossMusic()
	{
	}

	void Update ()
	{
		float fadeFactor = 1;
		string volumeController = null;

		foreach (var boss in bosses)
		{
			if (boss.dead)
				continue ;
			
			float bossDistance = (transform.position - boss.transform.position).magnitude - boss.radius;
			
			if (bossDistance < fadeDistance)
			{
				fadeFactor = bossDistance / fadeDistance;
				volumeController = boss.volumeControlName;
			}
		}

		mixer.SetFloat(backgroundVolume, fadeFactor);
	}
}
