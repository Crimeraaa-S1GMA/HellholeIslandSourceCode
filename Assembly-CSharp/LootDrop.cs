using System;
using UnityEngine;

// Token: 0x02000039 RID: 57
[Serializable]
public class LootDrop
{
	// Token: 0x0400010F RID: 271
	public string DROP_ID = "null";

	// Token: 0x04000110 RID: 272
	public int DROP_STACK_MIN;

	// Token: 0x04000111 RID: 273
	public int DROP_STACK_MAX;

	// Token: 0x04000112 RID: 274
	[Header("Random range")]
	public float DROP_CHANCE = 2f;

	// Token: 0x04000113 RID: 275
	public bool bestiaryIsHellholeModeDrop;
}
