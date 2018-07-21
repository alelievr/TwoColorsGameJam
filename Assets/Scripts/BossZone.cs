using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class BossZone : MonoBehaviour
{
	public string		bossName;
	public string		volumeControlName;
	public AudioClip	startClip;
	public AudioClip	loopClip;
	public bool			dead;

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

}
