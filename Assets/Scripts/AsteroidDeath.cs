using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsteroidDeath : MonoBehaviour
{
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("other: " + other);
		if (other.tag == "Laser")
		{
			// Debug.Log("DEATH");
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}
}
