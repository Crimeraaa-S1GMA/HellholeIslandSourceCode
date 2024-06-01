using System;
using UnityEngine;

// Token: 0x02000018 RID: 24
public class Bed : InteractableBlock
{
	// Token: 0x0600006F RID: 111 RVA: 0x00003C54 File Offset: 0x00001E54
	private void Update()
	{
		if (base.ReturnBlockInteraction() && !GameManager.instance.isInMines)
		{
			GameManager.instance.playerBeforeBed = base.transform.position - Vector2.up;
			GameManager.instance.respawnPos = base.transform.position - Vector2.up;
			GameManager.instance.bed = this;
		}
	}
}
