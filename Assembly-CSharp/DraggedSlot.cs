using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000032 RID: 50
public class DraggedSlot : MonoBehaviour
{
	// Token: 0x060000BB RID: 187 RVA: 0x00005734 File Offset: 0x00003934
	private void Update()
	{
		base.transform.position = Input.mousePosition;
		this.itemSprite.sprite = GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(GameManager.instance.inventory[GameManager.instance.draggedSlot].ITEM_ID).sprite;
		if (GameManager.instance.inventory[GameManager.instance.draggedSlot].ITEM_STACK == 1 && GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(GameManager.instance.inventory[GameManager.instance.draggedSlot].ITEM_ID).stackLimit == 1)
		{
			this.itemStack.text = "";
			return;
		}
		this.itemStack.text = GameManager.instance.inventory[GameManager.instance.draggedSlot].ITEM_STACK.ToString();
	}

	// Token: 0x040000D9 RID: 217
	[SerializeField]
	private Image itemSprite;

	// Token: 0x040000DA RID: 218
	[SerializeField]
	private Text itemStack;
}
