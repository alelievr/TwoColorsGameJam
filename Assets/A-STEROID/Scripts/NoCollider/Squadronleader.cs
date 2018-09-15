using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squadronleader : MonoBehaviour
{

    float squadSize = 10f;

    public List<NoColDebritController> debritList = new List<NoColDebritController>();

    public enum colorEnum
    {
        red,
        blue,
        green,
		grey,
        yellow,
        lightspeedPanther,
        KKK,
        stealth,
        crimsonPhoenix,
    }

	Dictionary<colorEnum, Color> colorDictionary = new Dictionary<colorEnum, Color>()
	{
		{colorEnum.red, Color.red},
		{colorEnum.blue, Color.blue},
		{colorEnum.green, Color.green},
		{colorEnum.grey, Color.grey},
		{colorEnum.yellow, Color.yellow},
		{colorEnum.lightspeedPanther, Color.cyan},
		{colorEnum.KKK, Color.white},
		{colorEnum.stealth, Color.black},
		{colorEnum.crimsonPhoenix, Color.magenta},
	};

    public colorEnum elColor;

    private void OnDrawGizmos()
    {
        Gizmos.color = colorDictionary[elColor];
        Gizmos.DrawWireSphere(transform.position, squadSize);
    }

    private void Update()
    {
        foreach(var i in debritList)
        {
            Debug.DrawLine(transform.position, i.transform.position, Color.red, Time.deltaTime);
        }

    }
}
