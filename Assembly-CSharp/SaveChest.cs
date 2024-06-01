using System;
using System.Collections.Generic;

// Token: 0x020000AD RID: 173
[Serializable]
public class SaveChest
{
	// Token: 0x04000414 RID: 1044
	public float x;

	// Token: 0x04000415 RID: 1045
	public float y;

	// Token: 0x04000416 RID: 1046
	public List<InventorySlot> inventory = new List<InventorySlot>(16);

	// Token: 0x04000417 RID: 1047
	public List<string> blockMetadata = new List<string>();
}
