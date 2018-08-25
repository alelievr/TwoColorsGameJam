using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.Linq;

public class AudioController : MonoBehaviour
{
	public AudioMixer		mixer;

	[Header("Audio fade settings")]
	public float			fadeDistance = 40;
	public float			backgroundMusicResetTime = 2;

	[Header("Boss zones")]
	public BossZone[]		bosses;

	[Header("Audio sources")]
	public AudioSource		bossAudioStart;
	public AudioSource		bossAudioLoop;
	public AudioSource		backgroundStart;
	public AudioSource		backgroundLoop;

	public static AudioController	instance;

	const string			backgroundVolume = "BackgroundVolume";
	string[]				bossVolumes;
	BossZone				currentBoss;
	bool					isTransitioning;

	bool					firstFrame = true;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		bossVolumes = bosses.Select(b => b.volumeControlName).ToArray();
	}

	public void StopBossMusic()
	{
		StartCoroutine(ResetBackgroundMusic());
		currentBoss.dead = true;
	}

	public bool PlayOneShot(AudioClip clip)
	{
		return true;
	}

	IEnumerator ResetBackgroundMusic()
	{
		isTransitioning = true;
		float t = Time.time;
		while (Time.time - t < backgroundMusicResetTime)
		{
			float ratio = (Time.time - t) / backgroundMusicResetTime;
			mixer.SetFloat(backgroundVolume, LinearToDecibel(ratio));
			foreach (var bossVolume in bossVolumes)
				mixer.SetFloat(bossVolume, LinearToDecibel(1.0f - ratio));
			yield return null;
		}

		// stop boss music audio sources
		bossAudioStart.Stop();
		bossAudioStart.clip = null;
		bossAudioLoop.Stop();
		bossAudioLoop.clip = null;
		isTransitioning = false;
	}

	void UpdateSoundTransition()
	{
		if (!backgroundStart.isPlaying && !firstFrame && !backgroundLoop.isPlaying)
			backgroundLoop.Play();
		
		if (!bossAudioStart.isPlaying && !bossAudioLoop.isPlaying && bossAudioLoop.clip != null)
			bossAudioLoop.Play();
	}

	void Update ()
	{
		UpdateSoundTransition();
		UpdateBossVolumes();

		firstFrame = false;
	}

	private float LinearToDecibel(float linear)
	{
		float dB;

		if (linear != 0)
			dB = 20.0f * Mathf.Log10(linear);
		else
			dB = -144.0f;

		return dB;
	}

	void StartBossMusic(BossZone zone)
	{
		bossAudioLoop.outputAudioMixerGroup = zone.audioGroup;
		bossAudioStart.outputAudioMixerGroup = zone.audioGroup;

		bossAudioStart.clip = zone.startClip;
		bossAudioLoop.clip = zone.loopClip;

		bossAudioStart.Play();
	}

	void UpdateBossVolumes()
	{
		float fadeFactor = 1;
		string volumeController = null;

		foreach (var boss in bosses)
		{
			if (boss.dead)
				continue ;
			
			float bossDistance = (transform.position - boss.transform.position).magnitude - boss.radius;

			if (bossDistance < 0)
			{
				if (currentBoss != boss)
					StartBossMusic(boss);
				currentBoss = boss;
			}
			
			if (bossDistance < fadeDistance && bossDistance > 0)
			{
				fadeFactor = bossDistance / fadeDistance;
				volumeController = boss.volumeControlName;
			}
		}

		if (volumeController == null || isTransitioning)
			return ;

		mixer.SetFloat(volumeController, LinearToDecibel(1));

		mixer.SetFloat(backgroundVolume, LinearToDecibel(fadeFactor));
	}
}
