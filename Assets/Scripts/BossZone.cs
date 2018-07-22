using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(CircleCollider2D))]
public class BossZone : MonoBehaviour
{
	public string			bossName;
	public string			volumeControlName;
	public AudioClip		startClip;
	public AudioClip		loopClip;
	public AudioMixerGroup	audioGroup;
	public bool				dead;

	CircleCollider2D	m_CircleCollider;
	CircleCollider2D	circleCollider
	{
		get
		{
			if (m_CircleCollider == null)
				m_CircleCollider = GetComponent<CircleCollider2D>();
			return m_CircleCollider;
		}
	}

	public float		radius
	{
		get { return circleCollider.radius; }
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
			GameManager.instance.EnterBossFight();
	}

}
