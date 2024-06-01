using System;
using UnityEngine;

// Token: 0x02000094 RID: 148
public class StructureBuilderBlock : MonoBehaviour
{
	// Token: 0x0600029B RID: 667 RVA: 0x00015788 File Offset: 0x00013988
	private void Update()
	{
		if (this.structureBuilder.ReturnCursor() == base.transform.position && Input.GetMouseButton(1) && this.structureBuilder.gui == BuilderGui.NoGui)
		{
			this.structureBuilder.structure.structureBlocks.Remove(this.structureBuilder.structure.FindBlock(base.transform.position, this.type));
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x04000384 RID: 900
	[HideInInspector]
	public StructureBuilder structureBuilder;

	// Token: 0x04000385 RID: 901
	[HideInInspector]
	public StructureBlockType type;
}
