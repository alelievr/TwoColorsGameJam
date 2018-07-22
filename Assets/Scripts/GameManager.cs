using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public int		gameState;
	public bool 	isBossFight;
	public	GameObject	mainCamera;
	public	GameObject	bossFightCamera;

	public static GameManager instance;
	public GameObject player;

	private void Awake()
	{
		instance = this;
	}

	private void Update()
	{
		// if (Input.GetKeyDown(KeyCode.LeftArrow)) {
		// 	DefeatBoss();
		// } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
		// 	EnterBossFight();
		// }
	}

	public void DefeatBoss()
	{
		gameState++;
		isBossFight = false;
		mainCamera.gameObject.SetActive(true);
		bossFightCamera.gameObject.SetActive(false);
	}

	public void EnterBossFight()
	{
		isBossFight = true;
		bossFightCamera.gameObject.SetActive(true);
		mainCamera.gameObject.SetActive(false);
	}
}
