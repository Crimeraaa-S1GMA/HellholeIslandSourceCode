using System;
using UnityEngine;

// Token: 0x02000097 RID: 151
public class Table : InteractableBlock
{
	// Token: 0x060002AC RID: 684 RVA: 0x00015C23 File Offset: 0x00013E23
	private void Start()
	{
		this.block = base.GetComponent<PlacedBlock>();
		if (this.block.blockMetadata.Count <= 0)
		{
			this.block.blockMetadata.Add("null");
		}
	}

	// Token: 0x060002AD RID: 685 RVA: 0x00015C5C File Offset: 0x00013E5C
	private void Update()
	{
		this.block.tableItemSprite.sprite = GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(this.block.blockMetadata[0]).sprite;
		if (base.ReturnBlockInteraction())
		{
			if (this.block.blockMetadata[0] == "null" && GameManager.instance.ReturnSelectedSlot().ITEM_ID != "null" && GameManager.instance.ReturnSelectedSlot().ITEM_STACK > 0)
			{
				this.block.blockMetadata[0] = GameManager.instance.ReturnSelectedSlot().ITEM_ID;
				GameManager.instance.RemoveCurrentHeldItemOne();
				return;
			}
			GameManager.instance.InitializePickupItem(base.transform.position + new Vector2((float)(2 * UtilityMath.NegativePositiveOne()), (float)(2 * UtilityMath.NegativePositiveOne())), this.block.blockMetadata[0], 1, true);
			this.block.blockMetadata[0] = "null";
		}
	}

	// Token: 0x04000391 RID: 913
	private PlacedBlock block;
}
