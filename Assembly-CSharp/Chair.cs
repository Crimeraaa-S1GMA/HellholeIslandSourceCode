using System;
using UnityEngine;

// Token: 0x0200002A RID: 42
public class Chair : InteractableBlock
{
	// Token: 0x060000A3 RID: 163 RVA: 0x00004C1F File Offset: 0x00002E1F
	private void Update()
	{
		if (base.ReturnBlockInteraction())
		{
			GameManager.instance.playerBeforeBed = base.transform.position - Vector2.up;
			GameManager.instance.chair = this;
		}
	}
}
