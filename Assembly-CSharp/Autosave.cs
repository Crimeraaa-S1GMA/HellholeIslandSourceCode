using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000017 RID: 23
public class Autosave : MonoBehaviour
{
	// Token: 0x0600006C RID: 108 RVA: 0x00003C33 File Offset: 0x00001E33
	private void Start()
	{
		base.StartCoroutine("AutoSave");
	}

	// Token: 0x0600006D RID: 109 RVA: 0x00003C41 File Offset: 0x00001E41
	private IEnumerator AutoSave()
	{
		for (;;)
		{
			yield return new WaitForSeconds(40f);
			if (!GameManager.instance.generatingWorld && GameManager.instance.autosave)
			{
				yield return WorldManager.instance.SaveWorld();
			}
		}
		yield break;
	}
}
