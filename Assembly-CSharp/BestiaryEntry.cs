using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200001D RID: 29
[Serializable]
public class BestiaryEntry
{
	// Token: 0x04000064 RID: 100
	public string entryName;

	// Token: 0x04000065 RID: 101
	[TextArea]
	public string entryDescription;

	// Token: 0x04000066 RID: 102
	public BestiarySpawnCondition spawnCondition;

	// Token: 0x04000067 RID: 103
	public Sprite entryPreview;

	// Token: 0x04000068 RID: 104
	public Color entryTint = Color.white;

	// Token: 0x04000069 RID: 105
	public int enemyHealth = 30;

	// Token: 0x0400006A RID: 106
	public List<LootDrop> drops = new List<LootDrop>();
}
