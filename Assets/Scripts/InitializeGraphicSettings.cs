using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeGraphicSettings
{
	[RuntimeInitializeOnLoadMethod]
	static void Init()
	{
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 60;
	}
}
