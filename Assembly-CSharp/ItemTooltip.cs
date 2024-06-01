using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200005E RID: 94
public class ItemTooltip : MonoBehaviour
{
	// Token: 0x060001C0 RID: 448 RVA: 0x0000F4D7 File Offset: 0x0000D6D7
	private void Start()
	{
		this.tooltip = base.GetComponent<Text>();
	}

	// Token: 0x060001C1 RID: 449 RVA: 0x0000F4E8 File Offset: 0x0000D6E8
	private void Update()
	{
		if (GameManager.instance.tooltipIndex != -1 && GameManager.instance.inventory[GameManager.instance.tooltipIndex].ITEM_ID != "null" && !GameManager.instance.draggingSlot)
		{
			Item item = GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(GameManager.instance.inventory[GameManager.instance.tooltipIndex].ITEM_ID);
			string text;
			if (item.stackLimit == 1)
			{
				text = "<size=30>" + item.name + "</size>";
			}
			else
			{
				text = string.Format("<size=30>{0} ({1})</size>", item.name, GameManager.instance.inventory[GameManager.instance.tooltipIndex].ITEM_STACK);
			}
			if (GameManager.instance.fullInventoryOpen && item.internalIdentifier != "money")
			{
				if (WorldPolicy.advancedTooltips)
				{
					text = text + "\nItem ID: " + GameManager.instance.inventory[GameManager.instance.tooltipIndex].ITEM_ID;
				}
				if (item.isWeapon)
				{
					if (GameManager.instance.extraDamage > 0f)
					{
						text += string.Format("\n{0} base damage", item.damage);
					}
					text += string.Format("\n{0} damage", item.damage + GameManager.instance.extraDamage);
				}
				if (item.description1 != "")
				{
					text = text + "\n" + item.description1;
				}
				if (item.blockId != string.Empty)
				{
					text += "\nCan be placed";
				}
				if (item.specialSlot != 0)
				{
					text += "\nCan be equipped";
				}
				if (item.description2 != "")
				{
					text = text + "\n" + item.description2;
				}
				if (GameManager.instance.npcShop != null && item.price > 0)
				{
					text += string.Format("\nSell price: ${0}", item.price * GameManager.instance.inventory[GameManager.instance.tooltipIndex].ITEM_STACK);
				}
			}
			this.tooltip.text = text;
		}
		else if (!GameManager.instance.draggingSlot)
		{
			int tooltipIndex = GameManager.instance.tooltipIndex;
			switch (tooltipIndex)
			{
			case 36:
				this.tooltip.text = "Trash";
				break;
			case 37:
				this.tooltip.text = "Helmet";
				break;
			case 38:
				this.tooltip.text = "Chestplate";
				break;
			case 39:
				this.tooltip.text = "Legs";
				break;
			case 40:
				this.tooltip.text = "Accesory";
				break;
			case 41:
				this.tooltip.text = "Accesory";
				break;
			case 42:
				this.tooltip.text = "Accesory";
				break;
			case 43:
				this.tooltip.text = "Accesory";
				break;
			default:
				if (tooltipIndex != 60)
				{
					this.tooltip.text = GameManager.instance.tooltipCustom;
				}
				else
				{
					this.tooltip.text = "Sell";
				}
				break;
			}
		}
		else
		{
			this.tooltip.text = GameManager.instance.tooltipCustom;
		}
		base.transform.position = ((Input.mousePosition.x > (float)(Screen.width - 405)) ? (Input.mousePosition + Vector3.left * 400f) : Input.mousePosition);
		this.tooltip.alignment = ((Input.mousePosition.x > (float)(Screen.width - 405)) ? TextAnchor.UpperRight : TextAnchor.UpperLeft);
	}

	// Token: 0x04000285 RID: 645
	private Text tooltip;
}
