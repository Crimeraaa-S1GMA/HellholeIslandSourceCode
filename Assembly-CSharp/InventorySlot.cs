using System;

// Token: 0x02000056 RID: 86
[Serializable]
public struct InventorySlot
{
	// Token: 0x060001BA RID: 442 RVA: 0x0000F2EC File Offset: 0x0000D4EC
	public static InventorySlot EmptySlot()
	{
		return new InventorySlot
		{
			ITEM_ID = "null",
			ITEM_STACK = 0
		};
	}

	// Token: 0x04000230 RID: 560
	public string ITEM_ID;

	// Token: 0x04000231 RID: 561
	public int ITEM_STACK;
}
