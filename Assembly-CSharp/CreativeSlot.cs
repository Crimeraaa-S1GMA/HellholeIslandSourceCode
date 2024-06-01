using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200002E RID: 46
public class CreativeSlot : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x060000B0 RID: 176 RVA: 0x0000509C File Offset: 0x0000329C
	private void Update()
	{
		if (this.itemIndex + GameManager.instance.creativeTab * 24 < GameManager.instance.creativeItemIds.Count)
		{
			this.displayItem.sprite = GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(GameManager.instance.creativeItemIds[this.itemIndex + GameManager.instance.creativeTab * 24]).sprite;
		}
		this.displayItem.enabled = (this.itemIndex + GameManager.instance.creativeTab * 24 < GameManager.instance.creativeItemIds.Count);
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x00005140 File Offset: 0x00003340
	public void AddIntoInventory()
	{
		if (this.itemIndex + GameManager.instance.creativeTab * 24 < GameManager.instance.creativeItemIds.Count)
		{
			if (Input.GetKey(KeyCode.LeftShift))
			{
				GameManager.instance.inventory[61] = new InventorySlot
				{
					ITEM_ID = GameManager.instance.creativeItemIds[this.itemIndex + GameManager.instance.creativeTab * 24],
					ITEM_STACK = GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(GameManager.instance.creativeItemIds[this.itemIndex + GameManager.instance.creativeTab * 24]).stackLimit
				};
			}
			else if (GameManager.instance.inventory[61].ITEM_ID != GameManager.instance.creativeItemIds[this.itemIndex + GameManager.instance.creativeTab * 24])
			{
				GameManager.instance.inventory[61] = new InventorySlot
				{
					ITEM_ID = GameManager.instance.creativeItemIds[this.itemIndex + GameManager.instance.creativeTab * 24],
					ITEM_STACK = 1
				};
			}
			else
			{
				GameManager.instance.inventory[61] = new InventorySlot
				{
					ITEM_ID = GameManager.instance.creativeItemIds[this.itemIndex + GameManager.instance.creativeTab * 24],
					ITEM_STACK = Mathf.Clamp(GameManager.instance.inventory[61].ITEM_STACK + 1, 0, GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(GameManager.instance.creativeItemIds[this.itemIndex + GameManager.instance.creativeTab * 24]).stackLimit)
				};
			}
			GameManager.instance.draggingSlot = true;
			GameManager.instance.pickingOutItems = true;
			GameManager.instance.creativePickingOut = true;
			GameManager.instance.pickedSlot = 61;
			GameManager.instance.draggedSlot = 61;
			GameManager.instance.dragStart = 61;
		}
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x00005384 File Offset: 0x00003584
	public void OnPointerEnter(PointerEventData pointerEventData)
	{
		GameManager.instance.hoveringButtonInv = true;
		GameManager.instance.selectingButtonInv = false;
		if (this.itemIndex + GameManager.instance.creativeTab * 24 < GameManager.instance.creativeItemIds.Count)
		{
			GameManager.instance.tooltipCustom = "<size=30>" + GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(GameManager.instance.creativeItemIds[this.itemIndex + GameManager.instance.creativeTab * 24]).name + "</size>";
		}
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x0000541C File Offset: 0x0000361C
	public void OnPointerExit(PointerEventData pointerEventData)
	{
		GameManager.instance.tooltipCustom = "";
		GameManager.instance.hoveringButtonInv = false;
		GameManager.instance.selectingButtonInv = false;
	}

	// Token: 0x040000B9 RID: 185
	[SerializeField]
	private Image displayItem;

	// Token: 0x040000BA RID: 186
	[SerializeField]
	private int itemIndex = 1;
}
