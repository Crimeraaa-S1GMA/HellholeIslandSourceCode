using System;
using UnityEngine;

// Token: 0x0200001C RID: 28
[CreateAssetMenu(fileName = "NewBestiaryReg", menuName = "Registries/BestiaryRegistry", order = 8)]
public class BestiaryRegistry : ScriptableObject
{
	// Token: 0x04000063 RID: 99
	public BestiaryEntry[] bestiaryEntries;
}
