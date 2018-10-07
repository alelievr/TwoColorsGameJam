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
	public GameObject		leaderBoard;

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
		if (deathClip)
			AudioController.instance.PlayOneShotOnPlayer(deathClip, null);
		StartCoroutine(WaitAndLeaderBoard());
	}

	bool OnLeaderBoard = false;

	IEnumerator WaitAndLeaderBoard()
	{
		float time = 0;
		while (time < 1)
		{
			time += Time.deltaTime;
			if (Input.anyKey)
				break ;
			yield return new WaitForEndOfFrame();
		}
		OnLeaderBoard = true;
		deathScreen.SetActive(false);
		leaderBoard?.SetActive(true);
	}

	private void Update()
	{
		if (!dead)
			return ;

		if (Input.GetKeyDown(KeyCode.Space)
			|| Input.GetKeyDown(KeyCode.KeypadEnter))
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
