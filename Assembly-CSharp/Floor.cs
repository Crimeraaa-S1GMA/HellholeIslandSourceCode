using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000045 RID: 69
[Serializable]
public class Floor
{
	// Token: 0x04000139 RID: 313
	public string floorInternalId;

	// Token: 0x0400013A RID: 314
	public Sprite floorSprite;

	// Token: 0x0400013B RID: 315
	public float maxFloorHealth = 3f;

	// Token: 0x0400013C RID: 316
	public List<LootDrop> loot = new List<LootDrop>();

	// Token: 0x0400013D RID: 317
	public bool immuneToExplosions;

	// Token: 0x0400013E RID: 318
	public bool isStonePath;

	// Token: 0x0400013F RID: 319
	public bool isSlimeFloor;
}
