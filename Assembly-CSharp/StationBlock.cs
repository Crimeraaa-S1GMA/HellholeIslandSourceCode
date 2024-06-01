using System;
using UnityEngine;

// Token: 0x0200008A RID: 138
public class StationBlock : InteractableBlock
{
	// Token: 0x0600027C RID: 636 RVA: 0x00014DB0 File Offset: 0x00012FB0
	private void Update()
	{
		if (base.ReturnBlockInteraction())
		{
			GameManager.instance.fullInventoryOpen = true;
			GameManager.instance.CloseAllMenus();
			GameManager.instance.station = this.stationToOpen;
			GameManager.instance.openedStationBlock = this;
			GameManager.instance.ReloadCraftingMenu(this.stationToOpen);
		}
		if (GameManager.instance.openedStationBlock == this && Vector2.Distance(base.transform.position, GameManager.instance.playerPos) > 5f)
		{
			GameManager.instance.CloseAllMenus();
		}
	}

	// Token: 0x0400035A RID: 858
	public CraftingStation stationToOpen = CraftingStation.Workbench;
}
