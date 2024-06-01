using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200008F RID: 143
[Serializable]
public class Structure
{
	// Token: 0x0600028D RID: 653 RVA: 0x0001515C File Offset: 0x0001335C
	public StructureBlock FindBlock(Vector2 pos, StructureBlockType type)
	{
		return Array.Find<StructureBlock>(this.structureBlocks.ToArray(), (StructureBlock b) => b.pos == pos && b.blockType == type);
	}

	// Token: 0x04000368 RID: 872
	public string structureName;

	// Token: 0x04000369 RID: 873
	public List<StructureBlock> structureBlocks = new List<StructureBlock>();
}
