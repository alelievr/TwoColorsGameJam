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

	public GameObject		winPanel;

	public static GameManager instance;
	public GameObject player;

	[HideInInspector] public Transform playerTransform;
	public float	playerSizeSqr;
	public Vector3	playerPos;

	private	Vector3		baseCamPosition;
	int currentboss = -1;
	public int enemylimit = 10;

	private void Awake()
	{
		instance = this;
		baseCamPosition = mainCamera.transform.position;
		playerTransform = player.transform;
	}

	private void Start()
	{
		foreach(GameObject i in bossFightCamera)
		{
			if (i != null)
				i.SetActive(false);
		}
	}

	private void Update()
	{
		mainCamera.transform.position = new Vector3(baseCamPosition.x, baseCamPosition.y, baseCamPosition.z + playerSizeSqr);
	}

	public void DefeatBoss()
	{
		if (currentboss != -1)
		{
			AudioController.instance.StopBossMusic();
			gameState++;
			isBossFight = false;
			mainCamera.gameObject.SetActive(true);
			// Debug.Log("Boss fight camera: " + currentboss);
			if (bossFightCamera[currentboss] && bossFightCamera[currentboss].gameObject)
				bossFightCamera[currentboss].gameObject.SetActive(false);
			if (bossZones[currentboss] && bossZones[currentboss].gameObject)
				bossZones[currentboss].gameObject.SetActive(false);		
			currentboss = -1;
		}

		if (currentboss == 2)
		{
			winPanel.SetActive(true);
		}
	}

	public void EnterBossFight(int zoneNumber)
	{
		isBossFight = true;
		currentboss = zoneNumber;
		bossFightCamera[zoneNumber].gameObject.SetActive(true);
		mainCamera.gameObject.SetActive(false);

		// Destroy every basic enemies in the scene:
		foreach (var enemy in GameManager.FindObjectsOfType< BasicEnemy >())
			Destroy(enemy.gameObject);
	}

	private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, Mathf.Sqrt(playerSizeSqr));
    }
}
