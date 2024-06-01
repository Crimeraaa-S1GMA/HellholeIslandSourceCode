using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

// Token: 0x02000070 RID: 112
public class PlacedBlock : MonoBehaviour
{
	// Token: 0x14000004 RID: 4
	// (add) Token: 0x06000203 RID: 515 RVA: 0x00011134 File Offset: 0x0000F334
	// (remove) Token: 0x06000204 RID: 516 RVA: 0x0001116C File Offset: 0x0000F36C
	public event Action OnBlockBreak;

	// Token: 0x06000205 RID: 517 RVA: 0x000111A4 File Offset: 0x0000F3A4
	public void PrepareBlock(string key)
	{
		this.blockKey = key;
		this.actualBlockHealth = this.ReturnBlock().maxBlockHealth;
		this.boxCollider.enabled = this.ReturnBlock().collisionEnabled;
		this.boxCollider.isTrigger = this.ReturnBlock().isTrigger;
		this.boxCollider.offset = this.ReturnBlock().colliderOffset;
		this.boxCollider.size = this.ReturnBlock().colliderSize;
		if (this.ReturnBlock().collisionEnabled)
		{
			GameManager.instance.spawnBlockerPositions.Add(base.transform.position);
		}
		this.sprite.sortingOrder = this.ReturnBlock().sortingLayer;
		this.sprite.spriteSortPoint = this.ReturnBlock().sortPoint;
		if (this.ReturnBlock().allowProjectilePasstrough)
		{
			base.tag = "BlockProj";
		}
		else
		{
			base.tag = "Block";
		}
		if (this.ReturnBlock().emitsLight)
		{
			this.lightComp.gameObject.SetActive(true);
			this.lightComp.color = this.ReturnBlock().lightColor;
			this.lightComp.intensity = this.ReturnBlock().lightIntensity;
			this.lightComp.pointLightOuterRadius = this.ReturnBlock().lightRadius;
		}
		if (this.ReturnBlock().isCraftingStation)
		{
			base.gameObject.AddComponent<StationBlock>().stationToOpen = this.ReturnBlock().craftingStationType;
			if (this.ReturnBlock().craftingStationType == CraftingStation.Campfire)
			{
				base.gameObject.AddComponent<CampfireRain>();
			}
		}
		switch (this.ReturnBlock().blockFeature)
		{
		case SpecialBlockFeature.Sign:
			base.gameObject.AddComponent<Sign>();
			break;
		case SpecialBlockFeature.Teleporter:
			base.gameObject.AddComponent<Teleporter>();
			break;
		case SpecialBlockFeature.Bed:
			base.gameObject.AddComponent<Bed>();
			this.bedHalf.GetComponent<SpriteRenderer>().sprite = this.ReturnBlock().spriteFrames[1];
			this.bedHalf.SetActive(true);
			break;
		case SpecialBlockFeature.Door:
			base.gameObject.AddComponent<DoorMechanic>();
			break;
		case SpecialBlockFeature.Farmland:
			this.cropSprite.gameObject.SetActive(true);
			base.gameObject.AddComponent<Farmland>();
			break;
		case SpecialBlockFeature.Lake:
			base.gameObject.AddComponent<Lake>();
			this.lakeCollider.enabled = true;
			this.lakeLocks.SetActive(true);
			break;
		case SpecialBlockFeature.BossStatue:
			base.gameObject.AddComponent<BossStatue>();
			break;
		case SpecialBlockFeature.Chest:
			if (this.chest == null)
			{
				this.chest = base.gameObject.AddComponent<Chest>();
				this.chest.GenerateChestSlots();
			}
			break;
		case SpecialBlockFeature.Wire:
			base.gameObject.AddComponent<Wire>();
			GameManager.instance.wirePositions.Add(base.transform.position);
			break;
		case SpecialBlockFeature.Explosives:
			base.gameObject.AddComponent<Explosives>();
			break;
		case SpecialBlockFeature.Table:
			base.gameObject.AddComponent<Table>();
			break;
		case SpecialBlockFeature.WaterBlock:
			base.gameObject.AddComponent<Lake>();
			base.gameObject.AddComponent<WaterBlock>();
			GameManager.instance.waterPositions.Add(base.transform.position);
			break;
		case SpecialBlockFeature.Chair:
			base.gameObject.AddComponent<Chair>();
			break;
		case SpecialBlockFeature.Sunflower:
			GameManager.instance.sunflowerPositions.Add(this);
			break;
		case SpecialBlockFeature.MinesEntrance:
			this.minesEntranceLocks.SetActive(true);
			break;
		case SpecialBlockFeature.MinesExitRail:
			this.minesEntranceLocks.SetActive(true);
			this.railLocks.SetActive(true);
			break;
		case SpecialBlockFeature.LeafBlock:
			base.gameObject.AddComponent<LeafBlock>();
			GameManager.instance.leafBlockPositions.Add(base.transform.position);
			break;
		case SpecialBlockFeature.SS2EventStatue:
			base.gameObject.AddComponent<InvasionStatue>();
			break;
		case SpecialBlockFeature.CoinMachine:
			base.gameObject.AddComponent<CoinMachine>();
			break;
		case SpecialBlockFeature.StructureBlock:
			base.gameObject.AddComponent<Sign>().isStructureBlock = true;
			break;
		case SpecialBlockFeature.Cauldron:
			base.gameObject.AddComponent<Cauldron>();
			break;
		case SpecialBlockFeature.Boat:
			this.boatLocks.SetActive(true);
			break;
		}
		this.SetAnimationFrame(this.ReturnFrame());
		if (this.ReturnBlock().animateBlock && GameManager.instance.blockAnimations)
		{
			base.gameObject.AddComponent<BlockAnimator>();
		}
		base.GetComponent<OptimizeRenderer>().StartUpdateRender();
	}

	// Token: 0x06000206 RID: 518 RVA: 0x0001163D File Offset: 0x0000F83D
	public Block ReturnBlock()
	{
		return GameManager.instance.blockRegistryReference.FindBlockByInternalIdentifier(this.blockKey);
	}

	// Token: 0x06000207 RID: 519 RVA: 0x00011654 File Offset: 0x0000F854
	private void Awake()
	{
		this.boxCollider = base.GetComponent<BoxCollider2D>();
		this.sprite = base.GetComponent<SpriteRenderer>();
		GameManager.instance.placedBlocks.Add(this);
	}

	// Token: 0x06000208 RID: 520 RVA: 0x0001167E File Offset: 0x0000F87E
	public int ReturnFrame()
	{
		return this.animationFrame;
	}

	// Token: 0x06000209 RID: 521 RVA: 0x00011686 File Offset: 0x0000F886
	public void FrameMove(int movedFrames)
	{
		this.SetAnimationFrame(this.animationFrame + movedFrames);
	}

	// Token: 0x0600020A RID: 522 RVA: 0x00011696 File Offset: 0x0000F896
	public void NextFrame()
	{
		this.FrameMove(1);
	}

	// Token: 0x0600020B RID: 523 RVA: 0x000116A0 File Offset: 0x0000F8A0
	public void SetAnimationFrame(int frame)
	{
		this.animationFrame = frame;
		if (this.ReturnBlock().blockFeature != SpecialBlockFeature.Wire)
		{
			if (this.animationFrame >= this.ReturnBlock().spriteFrames.Length)
			{
				this.animationFrame %= this.ReturnBlock().spriteFrames.Length;
				return;
			}
			this.sprite.sprite = this.ReturnBlock().spriteFrames[this.animationFrame];
		}
	}

	// Token: 0x0600020C RID: 524 RVA: 0x00011710 File Offset: 0x0000F910
	private void Update()
	{
		if (this.ReturnBlock().destroyedByFloor && GameManager.instance.floorPositions.Contains(base.transform.position))
		{
			this.actualBlockHealth = 0f;
			this.CheckForDeletion();
		}
	}

	// Token: 0x0600020D RID: 525 RVA: 0x0001175C File Offset: 0x0000F95C
	public void CheckForDeletion()
	{
		if (this.actualBlockHealth <= 0f)
		{
			if (this.ReturnBlock().blockFeature != SpecialBlockFeature.Chest || (this.ReturnBlock().blockFeature == SpecialBlockFeature.Chest && this.blockMetadata[2] != "locked"))
			{
				GameManager.instance.blockPositions.Remove(base.transform.position);
				GameManager.instance.highlightableBlocks.Remove(base.transform.position);
				GameManager.instance.wirePositions.Remove(base.transform.position);
				GameManager.instance.waterPositions.Remove(base.transform.position);
				GameManager.instance.leafBlockPositions.Remove(base.transform.position);
				GameManager.instance.spawnBlockerPositions.Remove(base.transform.position);
				if (this.OnBlockBreak != null)
				{
					this.OnBlockBreak();
				}
				this.IterDrops();
				if (this.ReturnBlock().blockFeature == SpecialBlockFeature.Table)
				{
					GameManager.instance.InitializePickupItem(base.transform.position + new Vector2((float)(2 * UtilityMath.NegativePositiveOne()), (float)(2 * UtilityMath.NegativePositiveOne())), this.blockMetadata[0], 1, true);
				}
				if (this.chest != null)
				{
					foreach (InventorySlot inventorySlot in this.chest.chestInventory)
					{
						if (inventorySlot.ITEM_ID != "null")
						{
							GameManager.instance.InitializePickupItem(base.transform.position + new Vector2(Random.Range(-2f, 1f), Random.Range(-2f, 1f)), inventorySlot.ITEM_ID, inventorySlot.ITEM_STACK, true);
						}
					}
				}
				Object.Destroy(base.gameObject);
				return;
			}
			this.actualBlockHealth = this.ReturnBlock().maxBlockHealth;
		}
	}

	// Token: 0x0600020E RID: 526 RVA: 0x000119B4 File Offset: 0x0000FBB4
	public void TakeBlockDamage(float damage)
	{
		if (WorldPolicy.creativeMode)
		{
			this.actualBlockHealth = 0f;
		}
		else
		{
			this.actualBlockHealth -= damage;
		}
		this.CheckForDeletion();
	}

	// Token: 0x0600020F RID: 527 RVA: 0x000119E0 File Offset: 0x0000FBE0
	private void IterDrops()
	{
		foreach (LootDrop lootDrop in this.ReturnBlock().loot)
		{
			if (UtilityMath.RandomChance(1f / (lootDrop.DROP_CHANCE - 1f)) && lootDrop.DROP_ID != "null" && lootDrop.DROP_STACK_MIN != 0 && lootDrop.DROP_STACK_MAX != 0)
			{
				GameManager.instance.InitializePickupItem(base.transform.position + new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f)), lootDrop.DROP_ID, Random.Range(lootDrop.DROP_STACK_MIN, lootDrop.DROP_STACK_MAX), true);
			}
		}
	}

	// Token: 0x06000210 RID: 528 RVA: 0x00011AD0 File Offset: 0x0000FCD0
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && (this.ReturnBlock().blockFeature == SpecialBlockFeature.MinesEntrance || this.ReturnBlock().blockFeature == SpecialBlockFeature.MinesExitRail) && GameManager.instance.chair == null && GameManager.instance.bed == null && GameManager.instance.selectedTeleporter == null && (this.ReturnBlock().blockInternalId != "mines_0" || (this.ReturnBlock().blockInternalId == "mines_0" && !GameManager.instance.blockPositions.Contains(base.transform.position - Vector2.up))))
		{
			WorldManager.instance.MinesTeleportation();
		}
	}

	// Token: 0x06000211 RID: 529 RVA: 0x00011BAA File Offset: 0x0000FDAA
	private void OnDestroy()
	{
		GameManager.instance.placedBlocks.Remove(this);
		if (this.ReturnBlock().blockFeature == SpecialBlockFeature.Sunflower)
		{
			GameManager.instance.sunflowerPositions.Remove(this);
		}
	}

	// Token: 0x040002DE RID: 734
	public string blockKey = "";

	// Token: 0x040002DF RID: 735
	[HideInInspector]
	public float actualBlockHealth;

	// Token: 0x040002E0 RID: 736
	public Chest chest;

	// Token: 0x040002E1 RID: 737
	public SpriteRenderer cropSprite;

	// Token: 0x040002E2 RID: 738
	public SpriteRenderer tableItemSprite;

	// Token: 0x040002E3 RID: 739
	public Light2D lightComp;

	// Token: 0x040002E4 RID: 740
	public GameObject bedHalf;

	// Token: 0x040002E5 RID: 741
	public GameObject lakeLocks;

	// Token: 0x040002E6 RID: 742
	public GameObject minesEntranceLocks;

	// Token: 0x040002E7 RID: 743
	public GameObject railLocks;

	// Token: 0x040002E8 RID: 744
	public GameObject boatLocks;

	// Token: 0x040002E9 RID: 745
	public BoxCollider2D campfireCollider;

	// Token: 0x040002EA RID: 746
	public List<string> blockMetadata = new List<string>();

	// Token: 0x040002EC RID: 748
	public SpriteRenderer[] ingredientSprites;

	// Token: 0x040002ED RID: 749
	public CapsuleCollider2D lakeCollider;

	// Token: 0x040002EE RID: 750
	private BoxCollider2D boxCollider;

	// Token: 0x040002EF RID: 751
	private SpriteRenderer sprite;

	// Token: 0x040002F0 RID: 752
	private int animationFrame;
}
