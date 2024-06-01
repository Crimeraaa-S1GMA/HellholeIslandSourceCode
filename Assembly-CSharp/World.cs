using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000A9 RID: 169
[Serializable]
public class World
{
	// Token: 0x06000331 RID: 817 RVA: 0x0001806F File Offset: 0x0001626F
	public int ScaleIntBasedOnWorldScale(int value)
	{
		return Mathf.RoundToInt((float)value * this.worldScale);
	}

	// Token: 0x06000332 RID: 818 RVA: 0x0001807F File Offset: 0x0001627F
	public int ScaleIntBasedOnWorldScaleTwice(int value)
	{
		return Mathf.RoundToInt((float)value * this.worldScale * this.worldScale);
	}

	// Token: 0x06000333 RID: 819 RVA: 0x00018096 File Offset: 0x00016296
	public float ScaleFloatBasedOnWorldScale(float value)
	{
		return value * this.worldScale;
	}

	// Token: 0x06000334 RID: 820 RVA: 0x000180A0 File Offset: 0x000162A0
	public float ScaleFloatBasedOnWorldScaleTwice(float value)
	{
		return value * this.worldScale * this.worldScale;
	}

	// Token: 0x040003E5 RID: 997
	[Header("World Metadata")]
	public string name;

	// Token: 0x040003E6 RID: 998
	public string storageName;

	// Token: 0x040003E7 RID: 999
	public string mapDescription = "";

	// Token: 0x040003E8 RID: 1000
	public string mapAuthor = "";

	// Token: 0x040003E9 RID: 1001
	public int worldIconId;

	// Token: 0x040003EA RID: 1002
	public int versionId;

	// Token: 0x040003EB RID: 1003
	[Header("World properties")]
	public int seed = 1;

	// Token: 0x040003EC RID: 1004
	public float worldScale = 1f;

	// Token: 0x040003ED RID: 1005
	public float time = 300f;

	// Token: 0x040003EE RID: 1006
	public bool rain;

	// Token: 0x040003EF RID: 1007
	public int health = 20;

	// Token: 0x040003F0 RID: 1008
	public int extraHealth;

	// Token: 0x040003F1 RID: 1009
	public int money;

	// Token: 0x040003F2 RID: 1010
	public float playerPosX;

	// Token: 0x040003F3 RID: 1011
	public float playerPosY;

	// Token: 0x040003F4 RID: 1012
	public bool rotativeCameraFeature;

	// Token: 0x040003F5 RID: 1013
	public List<string> defeatBosses = new List<string>();

	// Token: 0x040003F6 RID: 1014
	public List<ActiveStatusEffect> statusEffects = new List<ActiveStatusEffect>();

	// Token: 0x040003F7 RID: 1015
	[Header("Store world policy in fields")]
	public bool keepInventory;

	// Token: 0x040003F8 RID: 1016
	public bool creative;

	// Token: 0x040003F9 RID: 1017
	public bool spawning = true;

	// Token: 0x040003FA RID: 1018
	public bool advancedTooltips;

	// Token: 0x040003FB RID: 1019
	public bool dayNightCycle = true;

	// Token: 0x040003FC RID: 1020
	public bool hardcoreMode;

	// Token: 0x040003FD RID: 1021
	public bool infiniteBuildRange;

	// Token: 0x040003FE RID: 1022
	public bool allowSwitchingGamemodes;

	// Token: 0x040003FF RID: 1023
	public bool hellholeMode;

	// Token: 0x04000400 RID: 1024
	public bool trainwreckMode;

	// Token: 0x04000401 RID: 1025
	public bool nostalgiaSeed;

	// Token: 0x04000402 RID: 1026
	public bool sniperSkillsSeed;

	// Token: 0x04000403 RID: 1027
	public bool kyivSeed;

	// Token: 0x04000404 RID: 1028
	public bool experimentalFeatures;

	// Token: 0x04000405 RID: 1029
	[Header("Block data")]
	public List<SaveBlock> blocksPlaced = new List<SaveBlock>();

	// Token: 0x04000406 RID: 1030
	public List<SaveFloor> floorsPlaced = new List<SaveFloor>();

	// Token: 0x04000407 RID: 1031
	public List<SaveTree> treesPlaced = new List<SaveTree>();

	// Token: 0x04000408 RID: 1032
	public List<SaveChest> chestsPlaced = new List<SaveChest>();

	// Token: 0x04000409 RID: 1033
	[Header("Inventory")]
	public List<InventorySlot> inventory = new List<InventorySlot>(60);
}
