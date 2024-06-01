using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000042 RID: 66
[CreateAssetMenu(fileName = "NewFLT", menuName = "Registries/FishingLootTable", order = 5)]
public class FishingLootTable : ScriptableObject
{
	// Token: 0x04000134 RID: 308
	public List<LootDrop> drops = new List<LootDrop>();
}
