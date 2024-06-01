using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000021 RID: 33
[Serializable]
public class Block
{
	// Token: 0x04000078 RID: 120
	public string blockInternalId;

	// Token: 0x04000079 RID: 121
	public Sprite[] spriteFrames;

	// Token: 0x0400007A RID: 122
	public bool animateBlock;

	// Token: 0x0400007B RID: 123
	public float frameChangeInterval = 1f;

	// Token: 0x0400007C RID: 124
	public int sortingLayer;

	// Token: 0x0400007D RID: 125
	public SpriteSortPoint sortPoint;

	// Token: 0x0400007E RID: 126
	public float maxBlockHealth = 3f;

	// Token: 0x0400007F RID: 127
	public float minimalToolPower;

	// Token: 0x04000080 RID: 128
	public bool isCraftingStation;

	// Token: 0x04000081 RID: 129
	public CraftingStation craftingStationType = CraftingStation.Workbench;

	// Token: 0x04000082 RID: 130
	public List<LootDrop> loot = new List<LootDrop>();

	// Token: 0x04000083 RID: 131
	public bool collisionEnabled = true;

	// Token: 0x04000084 RID: 132
	public bool isTrigger;

	// Token: 0x04000085 RID: 133
	public Vector2 colliderSize;

	// Token: 0x04000086 RID: 134
	public Vector2 colliderOffset;

	// Token: 0x04000087 RID: 135
	public bool allowProjectilePasstrough;

	// Token: 0x04000088 RID: 136
	public bool emitsLight;

	// Token: 0x04000089 RID: 137
	public Color lightColor;

	// Token: 0x0400008A RID: 138
	public float lightIntensity;

	// Token: 0x0400008B RID: 139
	public float lightRadius;

	// Token: 0x0400008C RID: 140
	public bool immuneToExplosions;

	// Token: 0x0400008D RID: 141
	public bool destroyedByFloor;

	// Token: 0x0400008E RID: 142
	public SpecialBlockFeature blockFeature;
}
