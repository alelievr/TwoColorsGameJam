using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public int		gameState;
	public bool 	isBossFight;
	public	GameObject	mainCamera;
	public	GameObject[]	bossFightCamera;
	public	GameObject[]	bossZones;

	public static GameManager instance;
	public GameObject player;
	public float	playerSize;

	private	Vector3		baseCamPosition;
	public FleshScrit fleshsc;
	int fleshnext = 0;
	int currentboss = -1;

	private void Awake()
	{
		instance = this;
		baseCamPosition = mainCamera.transform.position;
	}

	private void Start()
	{
		GetFleshNext();
	}

	private void Update()
	{
		mainCamera.transform.position = new Vector3(baseCamPosition.x, baseCamPosition.y, baseCamPosition.z + playerSize);
	}

	public void DefeatBoss()
	{
		gameState++;
		isBossFight = false;
		mainCamera.gameObject.SetActive(true);
		if (bossFightCamera[currentboss])
			bossFightCamera[currentboss].gameObject.SetActive(false);
		if (bossZones[currentboss])
			bossZones[currentboss].gameObject.SetActive(false);		
		currentboss = -1;
		if (bossZones[fleshnext])
			GetFleshNext();
	}

	public void GetFleshNext()
	{
		Debug.Log("fleshe new di");
		fleshsc.nextBossZone = bossZones[fleshnext].transform;
		fleshnext++;
	}

	public void EnterBossFight(int zoneNumber)
	{
		isBossFight = true;
		currentboss = zoneNumber;
		bossFightCamera[zoneNumber].gameObject.SetActive(true);
		mainCamera.gameObject.SetActive(false);
	}
}
