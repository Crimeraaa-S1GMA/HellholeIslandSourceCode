using System;
using UnityEngine;

// Token: 0x02000096 RID: 150
[CreateAssetMenu(fileName = "NewStructureReg", menuName = "Registries/StructureRegistry", order = 6)]
public class StructureRegistry : ScriptableObject
{
	// Token: 0x060002AA RID: 682 RVA: 0x00015BB8 File Offset: 0x00013DB8
	public Structure FindStructureByInternalIdentifier(string identifier)
	{
		Structure result = Array.Find<Structure>(this.structures, (Structure s) => s.structureName == "no_structure");
		Structure structure = Array.Find<Structure>(this.structures, (Structure s) => s.structureName == identifier);
		if (structure != null)
		{
			return structure;
		}
		return result;
	}

	// Token: 0x04000390 RID: 912
	public Structure[] structures;
}
