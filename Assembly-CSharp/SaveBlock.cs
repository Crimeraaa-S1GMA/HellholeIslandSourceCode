using System;
using System.Collections.Generic;

// Token: 0x020000AA RID: 170
[Serializable]
public class SaveBlock
{
	// Token: 0x0400040A RID: 1034
	public float x;

	// Token: 0x0400040B RID: 1035
	public float y;

	// Token: 0x0400040C RID: 1036
	public string blockKey;

	// Token: 0x0400040D RID: 1037
	public List<string> blockMetadata = new List<string>();
}
