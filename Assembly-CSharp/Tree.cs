using System;
using UnityEngine;

// Token: 0x0200009D RID: 157
[Serializable]
public class Tree
{
	// Token: 0x040003AE RID: 942
	public string treeInternalId;

	// Token: 0x040003AF RID: 943
	public float maxTreeHealth;

	// Token: 0x040003B0 RID: 944
	public float growthSpeed;

	// Token: 0x040003B1 RID: 945
	public bool dropExtraItems = true;

	// Token: 0x040003B2 RID: 946
	public string saplingId;

	// Token: 0x040003B3 RID: 947
	public string woodId;

	// Token: 0x040003B4 RID: 948
	public Sprite saplingSprite;

	// Token: 0x040003B5 RID: 949
	public Sprite treeSprite;
}
