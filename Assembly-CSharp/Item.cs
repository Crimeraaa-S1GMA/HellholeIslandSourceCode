using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000058 RID: 88
[Serializable]
public class Item
{
	// Token: 0x04000233 RID: 563
	[Header("Metadata")]
	public string internalIdentifier;

	// Token: 0x04000234 RID: 564
	public Sprite sprite;

	// Token: 0x04000235 RID: 565
	public Sprite rodThrownSprite;

	// Token: 0x04000236 RID: 566
	public bool spriteIsUnlit;

	// Token: 0x04000237 RID: 567
	public string name;

	// Token: 0x04000238 RID: 568
	[TextArea]
	public string description1;

	// Token: 0x04000239 RID: 569
	[TextArea]
	public string description2;

	// Token: 0x0400023A RID: 570
	public CreativeMenuTab tab;

	// Token: 0x0400023B RID: 571
	[Header("Inventory")]
	public int stackLimit = 80;

	// Token: 0x0400023C RID: 572
	public ItemTier itemTier;

	// Token: 0x0400023D RID: 573
	[Header("Item Usage")]
	public ItemUseType useType;

	// Token: 0x0400023E RID: 574
	public bool canUse = true;

	// Token: 0x0400023F RID: 575
	public bool oneUse = true;

	// Token: 0x04000240 RID: 576
	public string specialUsage = "NotSpecified";

	// Token: 0x04000241 RID: 577
	public bool usableInMines = true;

	// Token: 0x04000242 RID: 578
	public float toolPower = 1f;

	// Token: 0x04000243 RID: 579
	public float size = 1f;

	// Token: 0x04000244 RID: 580
	public float animationSpeed = 2.5f;

	// Token: 0x04000245 RID: 581
	public float useCooldown;

	// Token: 0x04000246 RID: 582
	public int heal;

	// Token: 0x04000247 RID: 583
	public int lifeFlower;

	// Token: 0x04000248 RID: 584
	public string statusEffectId = string.Empty;

	// Token: 0x04000249 RID: 585
	public string useSound = "";

	// Token: 0x0400024A RID: 586
	public bool autoUse;

	// Token: 0x0400024B RID: 587
	public bool achievement;

	// Token: 0x0400024C RID: 588
	public int achievementId;

	// Token: 0x0400024D RID: 589
	public List<LootDrop> crateDrops = new List<LootDrop>();

	// Token: 0x0400024E RID: 590
	[Header("Weapon")]
	public bool isWeapon;

	// Token: 0x0400024F RID: 591
	public bool meleeHit = true;

	// Token: 0x04000250 RID: 592
	public float damage;

	// Token: 0x04000251 RID: 593
	public string ammoId = "null";

	// Token: 0x04000252 RID: 594
	public GameObject projectile;

	// Token: 0x04000253 RID: 595
	public float projectileOffset;

	// Token: 0x04000254 RID: 596
	public float[] projectileAngles = new float[1];

	// Token: 0x04000255 RID: 597
	[Header("Accesory")]
	public AccesoryBoosts accesoryBoosts;

	// Token: 0x04000256 RID: 598
	public int specialSlot;

	// Token: 0x04000257 RID: 599
	public Sprite[] armorSkinSprites = new Sprite[15];

	// Token: 0x04000258 RID: 600
	public bool isArmorGlowmask;

	// Token: 0x04000259 RID: 601
	public Sprite headAccesorySprite;

	// Token: 0x0400025A RID: 602
	public bool isMiningHelmet;

	// Token: 0x0400025B RID: 603
	[Header("Block")]
	public string blockId;

	// Token: 0x0400025C RID: 604
	public BlockType blockType;

	// Token: 0x0400025D RID: 605
	[Header("Buying")]
	public int price;

	// Token: 0x0400025E RID: 606
	[Header("Fishing")]
	public float fishingSpeed = 0.25f;

	// Token: 0x0400025F RID: 607
	[Header("Alchemy")]
	public bool isPotionIngredient;

	// Token: 0x04000260 RID: 608
	[Header("Experimental")]
	public bool experimentalItem;
}
