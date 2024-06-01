using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000068 RID: 104
public class NostalgiaSeedFont : MonoBehaviour
{
	// Token: 0x060001E1 RID: 481 RVA: 0x00010414 File Offset: 0x0000E614
	private void Start()
	{
		if (GameManager.instance.nostalgiaSeed)
		{
			Text[] array = Object.FindObjectsOfType<Text>(true);
			for (int i = 0; i < array.Length; i++)
			{
				array[i].font = GameManager.instance.fallbackFont;
			}
		}
	}
}
