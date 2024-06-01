using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200002B RID: 43
public class Chest : InteractableBlock
{
	// Token: 0x060000A5 RID: 165 RVA: 0x00004C60 File Offset: 0x00002E60
	private void Start()
	{
		this.placedBlock = base.GetComponent<PlacedBlock>();
		if (this.placedBlock.blockMetadata.Count < 2)
		{
			this.placedBlock.blockMetadata.Add("");
			this.placedBlock.blockMetadata.Add("0");
		}
		if (this.placedBlock.blockMetadata.Count < 3)
		{
			this.placedBlock.blockMetadata.Add("");
		}
		this.placedBlock.OnBlockBreak += this.SaveSlots;
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x00004CF5 File Offset: 0x00002EF5
	public void GenerateChestSlots()
	{
		this.chestInventory.Clear();
		while (this.chestInventory.Count < 16)
		{
			this.chestInventory.Add(InventorySlot.EmptySlot());
		}
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x00004D23 File Offset: 0x00002F23
	private void SaveSlots()
	{
		if (GameManager.instance.openedChest == this)
		{
			GameManager.instance.SaveChestSlots();
		}
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x00004D44 File Offset: 0x00002F44
	private void Update()
	{
		this.placedBlock.SetAnimationFrame((this.placedBlock.blockMetadata[2] == "locked") ? 1 : 0);
		if (base.ReturnBlockInteraction() && GameManager.instance.openedChest != this && Vector2.Distance(base.transform.position, GameManager.instance.playerPos) <= 5f)
		{
			if (this.placedBlock.blockMetadata[2] == "locked" && GameManager.instance.ReturnSelectedSlot().ITEM_ID == "key")
			{
				GameManager.instance.RemoveCurrentHeldItemOne();
				this.placedBlock.blockMetadata[2] = string.Empty;
				AudioManager.instance.Play("Chest");
			}
			else if (this.placedBlock.blockMetadata[2] != "locked")
			{
				GameManager.instance.fullInventoryOpen = true;
				GameManager.instance.CloseAllMenus();
				GameManager.instance.openedChest = this;
				AudioManager.instance.Play("Chest");
				GameManager.instance.LoadChestSlots();
				Object.FindObjectOfType<InventoryUiEvents>().LoadChestName();
			}
		}
		this.DetectSelSquare();
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x00004E98 File Offset: 0x00003098
	private void UpdateTooltip()
	{
		if (this.isSelecting && GameManager.instance.selectedTeleporter == null && GameManager.instance.sign == null && this.placedBlock.blockMetadata[2] != "locked")
		{
			GameManager.instance.tooltipCustom = this.placedBlock.blockMetadata[0];
			return;
		}
		GameManager.instance.tooltipCustom = "";
	}

	// Token: 0x060000AA RID: 170 RVA: 0x00004F1C File Offset: 0x0000311C
	private void DetectSelSquare()
	{
		if (base.transform.position == GameManager.instance.selectionSquare)
		{
			if (!this.isSelecting)
			{
				this.isSelecting = true;
				this.UpdateTooltip();
				return;
			}
		}
		else if (this.isSelecting)
		{
			this.isSelecting = false;
			this.UpdateTooltip();
		}
	}

	// Token: 0x040000B5 RID: 181
	[HideInInspector]
	public PlacedBlock placedBlock;

	// Token: 0x040000B6 RID: 182
	public List<InventorySlot> chestInventory = new List<InventorySlot>(16);

	// Token: 0x040000B7 RID: 183
	private bool isSelecting;
}
