using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class SquadGeneral : MonoBehaviour
{

    float squadSize = 7f;

    float circle = 0;
    float circleSize = 0;
    public NoColDebritManager debritManagerScript;
	public int length;
    public List<Squadronleader> squadList;


  void InitSquadList()
    {
        squadList = new List<Squadronleader>();
        foreach (Transform child in transform)
        {
            squadList.Add(child.transform.gameObject.GetComponent<Squadronleader>());
        }
        SetSquadPosition();
    }


    float GetCircleSize()
    {
        if (circle == 0)
            return 0;
        else
            return (circle * 2) + 1;
    }

    public void SetSquadPosition()
    {
        circle = 0;
        circleSize = GetCircleSize();
        length = squadList.Count;
        int x = 0;
        int y = 0;
        int i = 1;

		squadList[0].transform.position = transform.position;
        while (i < length)
        {
            while (x < circleSize && i < length)
            {
                while (y < circleSize && i < length)
                {
					if (x == 0 || x == circleSize - 1 || y == 0 || y == circleSize - 1)
					{
                    	squadList[i].transform.position = new Vector3(transform.position.x - (circle * squadSize) + x * squadSize, transform.position.y - (circle * squadSize) + y * squadSize, transform.position.z);
						i++;
					}
					y++;
                }
                y = 0;
                x++;
            }
            x = 0;
            circle++;
            circleSize = GetCircleSize();
        }
    }

	private void Update()
	{
		InitSquadList();
	}

}

