using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsteroidDeath : MonoBehaviour
{
	public bool dead = false;

	public int				life = 3;

	public AudioClip		deathClip;
	public GameObject		playerHit;
	public GameObject		deathScreen;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Laser")
		{
			Instantiate(playerHit, transform.position, Quaternion.identity);
			AudioController.instance.PlayDamageOnPlayer();
			life--;
			if (life == 0)
				Die();
		}
	}

	void Die()
	{
		deathScreen.SetActive(true);
		dead = true;
		AudioController.instance.PlayOneShotOnPlayer(deathClip, null);
	}

	private void Update()
	{
		if (!dead)
			return ;

		if (Input.GetKeyDown(KeyCode.Return)
			|| Input.GetKeyDown(KeyCode.Space)
			|| Input.GetKeyDown(KeyCode.KeypadEnter)
			|| Input.GetMouseButtonDown(0))
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
