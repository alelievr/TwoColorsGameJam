using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public int		gameState;
	public bool 	isBossFight;
	public	GameObject	mainCamera;
	public	GameObject[]	bossFightCamera;

	public static GameManager instance;
	public GameObject player;
	public float	playerSize;

	private	Vector3		baseCamPosition;

	private void Awake()
	{
		instance = this;
		baseCamPosition = mainCamera.transform.position;
	}

	private void Update()
	{
		mainCamera.transform.position = new Vector3(baseCamPosition.x, baseCamPosition.y, baseCamPosition.z + playerSize);
	}

	public void DefeatBoss(int zoneNumber)
	{
		gameState++;
		isBossFight = false;
		mainCamera.gameObject.SetActive(true);
		bossFightCamera[zoneNumber].gameObject.SetActive(false);
	}

	public void EnterBossFight(int zoneNumber)
	{
		isBossFight = true;
		bossFightCamera[zoneNumber].gameObject.SetActive(true);
		mainCamera.gameObject.SetActive(false);
	}
}
