using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000086 RID: 134
public class SlotGui : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x0600026B RID: 619 RVA: 0x00014124 File Offset: 0x00012324
	private void Start()
	{
		this.slotFrame = base.GetComponent<Image>();
	}

	// Token: 0x0600026C RID: 620 RVA: 0x00014134 File Offset: 0x00012334
	private void Update()
	{
		if (this.slotSymbol != null)
		{
			this.slotSymbol.enabled = (GameManager.instance.inventory[this.slotNumber].ITEM_ID == "null" || GameManager.instance.CheckIfSlotDragged(this.slotNumber));
		}
		this.slotFrame.sprite = ((GameManager.instance.selectedItem == this.slotNumber) ? this.slotSprites[1] : this.slotSprites[0]);
		this.itemSprite.gameObject.SetActive(!GameManager.instance.draggingSlot || GameManager.instance.draggedSlot != this.slotNumber);
		this.itemStack.gameObject.SetActive(!GameManager.instance.draggingSlot || GameManager.instance.draggedSlot != this.slotNumber);
		if (!GameManager.instance.draggingSlot || GameManager.instance.draggedSlot != this.slotNumber)
		{
			this.itemSprite.sprite = GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(GameManager.instance.inventory[this.slotNumber].ITEM_ID).sprite;
			if (GameManager.instance.inventory[this.slotNumber].ITEM_STACK == 0 || (GameManager.instance.inventory[this.slotNumber].ITEM_STACK == 1 && GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(GameManager.instance.inventory[this.slotNumber].ITEM_ID).stackLimit == 1))
			{
				this.itemStack.text = "";
			}
			else
			{
				this.itemStack.text = GameManager.instance.inventory[this.slotNumber].ITEM_STACK.ToString();
			}
		}
		if (GameManager.instance.tooltipIndex == this.slotNumber && Input.GetMouseButtonDown(0))
		{
			this.ClickSlot();
		}
	}

	// Token: 0x0600026D RID: 621 RVA: 0x00014350 File Offset: 0x00012550
	private void ClickSlot()
	{
		if (GameManager.instance.fullInventoryOpen)
		{
			if (this.slotNumber >= 44 && this.slotNumber <= 59 && Input.GetKey(KeyCode.LeftShift) && (GameManager.instance.GetSlotForPickup(GameManager.instance.inventory[this.slotNumber].ITEM_ID) != -1 || GameManager.instance.inventory[this.slotNumber].ITEM_ID == "money") && GameManager.instance.inventory[this.slotNumber].ITEM_ID != "null")
			{
				GameManager.instance.InitializePickupItem(GameManager.instance.playerPos, GameManager.instance.inventory[this.slotNumber].ITEM_ID, GameManager.instance.inventory[this.slotNumber].ITEM_STACK, true);
				GameManager.instance.inventory[this.slotNumber] = InventorySlot.EmptySlot();
				return;
			}
			if (GameManager.instance.draggingSlot)
			{
				GameManager.instance.dragEnd = this.slotNumber;
				InventorySlot inventorySlot = GameManager.instance.inventory[GameManager.instance.dragStart];
				InventorySlot inventorySlot2 = GameManager.instance.inventory[GameManager.instance.dragEnd];
				if (GameManager.instance.dragEnd == 37 && GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(inventorySlot.ITEM_ID).specialSlot != 1)
				{
					return;
				}
				if (GameManager.instance.dragEnd == 38 && GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(inventorySlot.ITEM_ID).specialSlot != 2)
				{
					return;
				}
				if (GameManager.instance.dragEnd == 39 && GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(inventorySlot.ITEM_ID).specialSlot != 3)
				{
					return;
				}
				if ((GameManager.instance.dragEnd == 40 && GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(inventorySlot.ITEM_ID).specialSlot != 4) || (GameManager.instance.dragEnd == 40 && this.AccesoryCheck(inventorySlot, 40) && !GameManager.instance.IsAccesorySlot(GameManager.instance.dragStart)))
				{
					return;
				}
				if ((GameManager.instance.dragEnd == 41 && GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(inventorySlot.ITEM_ID).specialSlot != 4) || (GameManager.instance.dragEnd == 41 && this.AccesoryCheck(inventorySlot, 41) && !GameManager.instance.IsAccesorySlot(GameManager.instance.dragStart)))
				{
					return;
				}
				if ((GameManager.instance.dragEnd == 42 && GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(inventorySlot.ITEM_ID).specialSlot != 4) || (GameManager.instance.dragEnd == 42 && this.AccesoryCheck(inventorySlot, 42) && !GameManager.instance.IsAccesorySlot(GameManager.instance.dragStart)))
				{
					return;
				}
				if ((GameManager.instance.dragEnd == 43 && GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(inventorySlot.ITEM_ID).specialSlot != 4) || (GameManager.instance.dragEnd == 43 && this.AccesoryCheck(inventorySlot, 43) && !GameManager.instance.IsAccesorySlot(GameManager.instance.dragStart)))
				{
					return;
				}
				bool flag = ((inventorySlot.ITEM_ID == inventorySlot2.ITEM_ID || inventorySlot2.ITEM_ID == "null") && inventorySlot.ITEM_STACK + inventorySlot2.ITEM_STACK <= GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(inventorySlot.ITEM_ID).stackLimit) || GameManager.instance.creativePickingOut;
				if (GameManager.instance.pickingOutItems && !flag)
				{
					return;
				}
				if (inventorySlot.ITEM_ID == inventorySlot2.ITEM_ID && GameManager.instance.dragStart != GameManager.instance.dragEnd)
				{
					int stackLimit = GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(inventorySlot.ITEM_ID).stackLimit;
					int num = inventorySlot.ITEM_STACK + inventorySlot2.ITEM_STACK;
					if (num - stackLimit > 0)
					{
						GameManager.instance.inventory[GameManager.instance.dragStart] = new InventorySlot
						{
							ITEM_ID = inventorySlot.ITEM_ID,
							ITEM_STACK = num - stackLimit
						};
					}
					else
					{
						GameManager.instance.inventory[GameManager.instance.dragStart] = InventorySlot.EmptySlot();
					}
					GameManager.instance.inventory[GameManager.instance.dragEnd] = new InventorySlot
					{
						ITEM_ID = inventorySlot.ITEM_ID,
						ITEM_STACK = Mathf.Clamp(num, 0, stackLimit)
					};
				}
				else if (!GameManager.instance.IsAnySpecialSlot(GameManager.instance.dragStart) && !GameManager.instance.IsAnySpecialSlot(GameManager.instance.dragEnd))
				{
					GameManager.instance.inventory[GameManager.instance.dragStart] = inventorySlot2;
					GameManager.instance.inventory[GameManager.instance.dragEnd] = inventorySlot;
				}
				else if (this.ReturnSpecialSlotFromSlot(GameManager.instance.dragEnd) == GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(inventorySlot.ITEM_ID).specialSlot || this.ReturnSpecialSlotFromSlot(GameManager.instance.dragEnd) == 0)
				{
					if (inventorySlot2.ITEM_ID == "null")
					{
						if (GameManager.instance.IsArmorSlot(GameManager.instance.dragEnd))
						{
							AchievementManager.instance.AddAchievement(156197);
							if (inventorySlot.ITEM_ID == "monke_mask")
							{
								AchievementManager.instance.AddAchievement(175243);
							}
						}
						GameManager.instance.inventory[GameManager.instance.dragStart] = inventorySlot2;
						GameManager.instance.inventory[GameManager.instance.dragEnd] = inventorySlot;
					}
					else if (GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(inventorySlot2.ITEM_ID).specialSlot == this.ReturnSpecialSlotFromSlot(GameManager.instance.dragStart) || GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(inventorySlot.ITEM_ID).specialSlot == GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(inventorySlot2.ITEM_ID).specialSlot)
					{
						GameManager.instance.inventory[GameManager.instance.dragStart] = inventorySlot2;
						GameManager.instance.inventory[GameManager.instance.dragEnd] = inventorySlot;
					}
				}
				if (!GameManager.instance.pickingOutItems || GameManager.instance.inventory[61].ITEM_STACK <= 0 || !GameManager.instance.creativePickingOut)
				{
					GameManager.instance.draggingSlot = false;
					GameManager.instance.pickingOutItems = false;
					GameManager.instance.creativePickingOut = false;
					GameManager.instance.draggedSlot = 0;
					return;
				}
			}
			else if (GameManager.instance.inventory[this.slotNumber].ITEM_ID != "null")
			{
				GameManager.instance.inventory[61] = new InventorySlot
				{
					ITEM_ID = GameManager.instance.inventory[this.slotNumber].ITEM_ID,
					ITEM_STACK = GameManager.instance.inventory[this.slotNumber].ITEM_STACK
				};
				GameManager.instance.inventory[this.slotNumber] = InventorySlot.EmptySlot();
				GameManager.instance.draggingSlot = true;
				GameManager.instance.pickingOutItems = true;
				GameManager.instance.pickedSlot = this.slotNumber;
				GameManager.instance.creativePickingOut = true;
				GameManager.instance.draggedSlot = 61;
				GameManager.instance.dragStart = 61;
				return;
			}
		}
		else
		{
			GameManager.instance.selectedItem = this.slotNumber;
		}
	}

	// Token: 0x0600026E RID: 622 RVA: 0x00014B47 File Offset: 0x00012D47
	private int ReturnSpecialSlotFromSlot(int slot)
	{
		if (slot == 37)
		{
			return 1;
		}
		if (slot == 38)
		{
			return 2;
		}
		if (slot == 39)
		{
			return 3;
		}
		if (slot == 40)
		{
			return 4;
		}
		if (slot == 41)
		{
			return 4;
		}
		if (slot == 42)
		{
			return 4;
		}
		if (slot == 43)
		{
			return 4;
		}
		return 0;
	}

	// Token: 0x0600026F RID: 623 RVA: 0x00014B7B File Offset: 0x00012D7B
	public void OnPointerEnter(PointerEventData pointerEventData)
	{
		GameManager.instance.tooltipIndex = this.slotNumber;
		GameManager.instance.tooltipCustom = "";
	}

	// Token: 0x06000270 RID: 624 RVA: 0x00014B9C File Offset: 0x00012D9C
	public void OnPointerExit(PointerEventData pointerEventData)
	{
		GameManager.instance.tooltipIndex = -1;
		GameManager.instance.tooltipCustom = "";
	}

	// Token: 0x06000271 RID: 625 RVA: 0x00014BB8 File Offset: 0x00012DB8
	private bool AccesoryCheck(InventorySlot startSlot, int endSlotId)
	{
		if (GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(startSlot.ITEM_ID).specialUsage == "Shield")
		{
			return GameManager.instance.EquippedShield() && GameManager.instance.IsAccesorySlot(endSlotId);
		}
		return GameManager.instance.EquippedAccesory(startSlot.ITEM_ID);
	}

	// Token: 0x04000348 RID: 840
	[SerializeField]
	private int slotNumber;

	// Token: 0x04000349 RID: 841
	[SerializeField]
	private Image itemSprite;

	// Token: 0x0400034A RID: 842
	[SerializeField]
	private Text itemStack;

	// Token: 0x0400034B RID: 843
	[SerializeField]
	private Sprite[] slotSprites;

	// Token: 0x0400034C RID: 844
	[SerializeField]
	private Image slotSymbol;

	// Token: 0x0400034D RID: 845
	private Image slotFrame;
}
