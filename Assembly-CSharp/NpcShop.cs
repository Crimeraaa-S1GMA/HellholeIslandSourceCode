using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000069 RID: 105
public class NpcShop : MonoBehaviour
{
	// Token: 0x060001E3 RID: 483 RVA: 0x0001045C File Offset: 0x0000E65C
	private void Start()
	{
		if (this.generateStore)
		{
			this.GenerateStore(Random.Range(7, Random.Range(7, 13)));
		}
	}

	// Token: 0x060001E4 RID: 484 RVA: 0x0001047A File Offset: 0x0000E67A
	private bool ReturnOpeningBlock()
	{
		return Input.GetKeyDown(KeyCode.E);
	}

	// Token: 0x060001E5 RID: 485 RVA: 0x00010484 File Offset: 0x0000E684
	private void Update()
	{
		if (Vector2.Distance(base.transform.position, GameManager.instance.playerPos) < 3f && !GameManager.instance.generatingWorld && GameManager.instance.bed == null && GameManager.instance.selectedTeleporter == null)
		{
			if (GameManager.instance.interactionTooltips && this.interactionTooltip != null)
			{
				this.interactionTooltip.enabled = true;
			}
			if (this.ReturnOpeningBlock())
			{
				this.OpenShop();
			}
		}
		else if (this.interactionTooltip != null)
		{
			this.interactionTooltip.enabled = false;
		}
		if (GameManager.instance.npcShop == this && Vector2.Distance(base.transform.position, GameManager.instance.playerPos) > 7f)
		{
			this.CloseShop();
		}
	}

	// Token: 0x060001E6 RID: 486 RVA: 0x00010575 File Offset: 0x0000E775
	public string ReturnStoreItemId(int index)
	{
		if (index >= this.soldItemIds.Count)
		{
			return "null";
		}
		return this.soldItemIds[index];
	}

	// Token: 0x060001E7 RID: 487 RVA: 0x00010597 File Offset: 0x0000E797
	public int ReturnStoreSize()
	{
		return this.soldItemIds.Count;
	}

	// Token: 0x060001E8 RID: 488 RVA: 0x000105A4 File Offset: 0x0000E7A4
	private void GenerateStore(int itemQuantity)
	{
		List<string> list = new List<string>();
		foreach (string item in this.storeItemIds)
		{
			list.Add(item);
		}
		for (int j = 0; j < itemQuantity; j++)
		{
			int index = Random.Range(0, list.Count);
			this.soldItemIds.Add(list[index]);
			list.Remove(list[index]);
		}
	}

	// Token: 0x060001E9 RID: 489 RVA: 0x00010618 File Offset: 0x0000E818
	private void OpenShop()
	{
		if (GameManager.instance.bed == null && GameManager.instance.selectedTeleporter == null)
		{
			GameManager.instance.ReloadCraftingMenu(CraftingStation.ByHand);
			if (GameManager.instance.openedChest != null)
			{
				GameManager.instance.SaveChestSlots();
			}
			GameManager.instance.fullInventoryOpen = true;
			GameManager.instance.tooltipIndex = -1;
			GameManager.instance.draggingSlot = false;
			GameManager.instance.draggedSlot = 0;
			GameManager.instance.dragStart = 0;
			GameManager.instance.dragEnd = 0;
			GameManager.instance.tooltipCustom = "";
			GameManager.instance.openedChest = null;
			GameManager.instance.sign = null;
			GameManager.instance.bed = null;
			GameManager.instance.chair = null;
			GameManager.instance.openedStationBlock = null;
			GameManager.instance.npcShop = this;
		}
	}

	// Token: 0x060001EA RID: 490 RVA: 0x0001070C File Offset: 0x0000E90C
	private void CloseShop()
	{
		GameManager.instance.ReloadCraftingMenu(CraftingStation.ByHand);
		if (GameManager.instance.openedChest != null)
		{
			GameManager.instance.SaveChestSlots();
		}
		GameManager.instance.npcShop = null;
		GameManager.instance.fullInventoryOpen = false;
		GameManager.instance.tooltipIndex = -1;
		GameManager.instance.draggingSlot = false;
		GameManager.instance.draggedSlot = 0;
		GameManager.instance.dragStart = 0;
		GameManager.instance.dragEnd = 0;
		GameManager.instance.tooltipCustom = "";
		GameManager.instance.openedChest = null;
		GameManager.instance.sign = null;
		GameManager.instance.bed = null;
		GameManager.instance.chair = null;
		GameManager.instance.openedStationBlock = null;
	}

	// Token: 0x040002C4 RID: 708
	[SerializeField]
	private bool generateStore;

	// Token: 0x040002C5 RID: 709
	[SerializeField]
	private string[] storeItemIds;

	// Token: 0x040002C6 RID: 710
	private List<string> soldItemIds = new List<string>();

	// Token: 0x040002C7 RID: 711
	[SerializeField]
	private SpriteRenderer interactionTooltip;
}
