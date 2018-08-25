using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.Linq;

public class AudioController : MonoBehaviour
{
	public AudioMixer		mixer;

	public int				maxSimultaneousSound = 15;

	[Header("Audio fade settings")]
	public float			fadeDistance = 40;
	public float			backgroundMusicResetTime = 2;

	[Space, Header("Boss zones")]
	public BossZone[]		bosses;

	[Space, Header("Audio sources")]
	public AudioSource		bossAudioStart;
	public AudioSource		bossAudioLoop;
	public AudioSource		backgroundStart;
	public AudioSource		backgroundLoop;

	[Space, Header("Audio clips")]
	public AudioClip		damageClip;
	public AudioMixerGroup	damageMixerGroup;
	public AudioClip		explosionClip;
	public AudioMixerGroup	explosionMixerGroup;
	public AudioClip		aggregateClip;
	public AudioMixerGroup	aggregateMixerGroup;
	public AudioClip		laserClip;
	public AudioMixerGroup	laserMixerGroup;

	public static AudioController	instance;

	const string			backgroundVolume = "BackgroundVolume";
	string[]				bossVolumes;
	BossZone				currentBoss;
	bool					isTransitioning;

	List<AudioSource>		oneShotPlaySources;

	bool					firstFrame = true;

	private void Awake()
	{
		instance = this;

		oneShotPlaySources = Enumerable.Range(0, maxSimultaneousSound).Select(i => gameObject.AddComponent<AudioSource>()).ToList();
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

	public bool PlayOneShotOnPlayer(AudioClip clip, AudioMixerGroup mixerGroup, float volume = 1)
	{
		var audioSource = oneShotPlaySources.FirstOrDefault(a => !a.isPlaying);

		if (audioSource == null)
			return false;

		audioSource.clip = clip;
		audioSource.outputAudioMixerGroup = mixerGroup;
		audioSource.volume = volume;
		audioSource.Play();

		return true;
	}

	public bool PlayExplosionOnPlayer(float volume = 1) { return PlayOneShotOnPlayer(explosionClip, explosionMixerGroup, volume); }
	public bool PlayDamageOnPlayer(float volume = 1) { return PlayOneShotOnPlayer(damageClip, damageMixerGroup, volume); }
	public bool PlayAggregateOnPlayer(float volume = 1) { return PlayOneShotOnPlayer(aggregateClip, aggregateMixerGroup, volume); }
	public bool PlayLaserOnPlayer(float volume = 1) { return PlayOneShotOnPlayer(laserClip, laserMixerGroup, volume); }

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
