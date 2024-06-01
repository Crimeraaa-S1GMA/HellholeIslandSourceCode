using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000AF RID: 175
public class WorldGeneration : MonoBehaviour
{
	// Token: 0x0600033D RID: 829 RVA: 0x0001823C File Offset: 0x0001643C
	private void Start()
	{
		base.StartCoroutine("GenerateLevelAsync");
	}

	// Token: 0x0600033E RID: 830 RVA: 0x0001824A File Offset: 0x0001644A
	private IEnumerator GenerateLevelAsync()
	{
		this.rng = new Random(WorldPolicy.worldSeed);
		GameManager.instance.generatingWorld = true;
		GameManager.instance.tooltipCustom = "";
		GameManager.instance.blockPositions.Clear();
		GameManager.instance.floorPositions.Clear();
		GameManager.instance.treePositions.Clear();
		if (WorldManager.instance.isNewWorld)
		{
			GameManager.instance.levelGenerationProgress = 0;
			GameManager.instance.SetWorldGenerationStep(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(1), "Generating world");
			this.SpawnMinesEntrance();
			if (!GameManager.instance.nostalgiaSeed && !GameManager.instance.sniperSkillsSeed)
			{
				this.SpawnStructure(GameManager.instance.structureRegistryReference.FindStructureByInternalIdentifier("boat"), new Vector2((float)(25 * this.NegativePositiveOne(this.rng)), (float)WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-148)));
			}
			yield return null;
			if (!GameManager.instance.sniperSkillsSeed)
			{
				GameManager.instance.SetWorldGenerationStep(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(380), "Generating island");
				int len = this.rng.Next(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-148) - 1, WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-148));
				for (int i = WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-190); i < WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(190); i++)
				{
					yield return null;
					if (this.rng.Next(1, 3) == 1)
					{
						len = Mathf.Clamp(len + this.rng.Next(-1, 2), WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-148) - 1, WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-148) + 1);
					}
					for (int j = WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-148) - 4; j < len; j++)
					{
						WorldManager.instance.SpawnBlock("water", new Vector2((float)i + 0.5f, (float)j));
					}
					GameManager.instance.levelGenerationProgress++;
				}
			}
			if (GameManager.instance.sniperSkillsSeed)
			{
				this.SpawnSquariumAltar();
			}
			GameManager.instance.SetWorldGenerationStep(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScaleTwice(1), "Spawning structures");
			yield return null;
			if (!GameManager.instance.nostalgiaSeed && !GameManager.instance.sniperSkillsSeed)
			{
				this.SpawnStructure(GameManager.instance.structureRegistryReference.FindStructureByInternalIdentifier("ruinedhouse"), new Vector2((float)this.rng.Next(-15, 16), (float)(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-148) + 8)));
			}
			yield return null;
			if (!GameManager.instance.sniperSkillsSeed && this.rng.Next(1, 4) == 1)
			{
				this.RandSpawnStructure(GameManager.instance.structureRegistryReference.FindStructureByInternalIdentifier("camp"));
			}
			int num;
			if (!GameManager.instance.nostalgiaSeed && !GameManager.instance.sniperSkillsSeed)
			{
				GameManager.instance.SetWorldGenerationStep(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScaleTwice(4), "Generating lakes");
				for (int len = 0; len < WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScaleTwice(4); len = num + 1)
				{
					if (len % 5 == 0)
					{
						yield return null;
					}
					Vector2 vector = new Vector2((float)this.rng.Next(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-80), WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(81)), (float)this.rng.Next(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-80), WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(81)));
					if (Vector2.Distance(vector, this.minesEntrancePosition) > 4f)
					{
						this.SpawnStructure(GameManager.instance.structureRegistryReference.FindStructureByInternalIdentifier("lake"), vector);
					}
					GameManager.instance.levelGenerationProgress++;
					num = len;
				}
			}
			yield return null;
			if (GameManager.instance.kyivSeed)
			{
				GameManager.instance.SetWorldGenerationStep(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScaleTwice(14), "Generating ukrainium shrines");
				for (int len = 0; len < WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScaleTwice(14); len = num + 1)
				{
					if (len % 5 == 0)
					{
						yield return null;
					}
					Vector2 vector2 = new Vector2((float)this.rng.Next(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-80), WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(81)), (float)this.rng.Next(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-80), WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(81)));
					if (Vector2.Distance(vector2, this.minesEntrancePosition) > 4f)
					{
						this.SpawnStructure(GameManager.instance.structureRegistryReference.FindStructureByInternalIdentifier("ukrashrine"), vector2);
					}
					GameManager.instance.levelGenerationProgress++;
					num = len;
				}
			}
			yield return null;
			if (!GameManager.instance.nostalgiaSeed && !GameManager.instance.sniperSkillsSeed)
			{
				GameManager.instance.SetWorldGenerationStep(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScaleTwice(8), "Generating big bushes");
				for (int len = 0; len < WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScaleTwice(8); len = num + 1)
				{
					if (len % 5 == 0)
					{
						yield return null;
					}
					Vector2 vector3 = new Vector2((float)this.rng.Next(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-80), WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(81)), (float)this.rng.Next(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-80), WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(81)));
					if (Vector2.Distance(vector3, this.minesEntrancePosition) > 4f)
					{
						this.SpawnStructure(GameManager.instance.structureRegistryReference.FindStructureByInternalIdentifier("big_bush"), vector3);
					}
					GameManager.instance.levelGenerationProgress++;
					num = len;
				}
			}
			yield return null;
			if (!GameManager.instance.sniperSkillsSeed)
			{
				GameManager.instance.SetWorldGenerationStep(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScaleTwice(24), "Generating stone clusters");
				for (int len = 0; len < WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScaleTwice(24); len = num + 1)
				{
					if (len % 5 == 0)
					{
						yield return null;
					}
					Vector2 vector4 = new Vector2((float)this.rng.Next(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-80), WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(81)), (float)this.rng.Next(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-80), WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(81)));
					if (Vector2.Distance(vector4, this.minesEntrancePosition) > 4f)
					{
						this.SpawnStructure(GameManager.instance.structureRegistryReference.FindStructureByInternalIdentifier("stonecluster"), vector4);
					}
					GameManager.instance.levelGenerationProgress++;
					num = len;
				}
			}
			yield return null;
			if (!GameManager.instance.nostalgiaSeed && !GameManager.instance.sniperSkillsSeed)
			{
				GameManager.instance.SetWorldGenerationStep(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScaleTwice(6), "Generating large sand rocks");
				for (int len = 0; len < WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScaleTwice(6); len = num + 1)
				{
					if (len % 5 == 0)
					{
						yield return null;
					}
					Vector2 vector5 = new Vector2((float)this.rng.Next(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-80), WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(81)), (float)this.rng.Next(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-80), WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(81)));
					if (Vector2.Distance(vector5, this.minesEntrancePosition) > 4f)
					{
						this.SpawnStructure(GameManager.instance.structureRegistryReference.FindStructureByInternalIdentifier("large_rock_with_sand"), vector5);
					}
					GameManager.instance.levelGenerationProgress++;
					num = len;
				}
			}
			yield return null;
			if (GameManager.instance.sniperSkillsSeed)
			{
				GameManager.instance.SetWorldGenerationStep(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScaleTwice(480), "Generating chests");
				for (int len = 0; len < WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScaleTwice(480); len = num + 1)
				{
					if (len % 5 == 0)
					{
						yield return null;
					}
					this.SpawnChest();
					GameManager.instance.levelGenerationProgress++;
					num = len;
				}
			}
			else
			{
				GameManager.instance.SetWorldGenerationStep(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScaleTwice(80), "Generating chests");
				for (int len = 0; len < WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScaleTwice(80); len = num + 1)
				{
					if (len % 5 == 0)
					{
						yield return null;
					}
					this.SpawnChest();
					GameManager.instance.levelGenerationProgress++;
					num = len;
				}
			}
			yield return null;
			if (GameManager.instance.sniperSkillsSeed)
			{
				GameManager.instance.SetWorldGenerationStep(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScaleTwice(900), "Generating CRATES");
				for (int len = 0; len < WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScaleTwice(900); len = num + 1)
				{
					if (len % 5 == 0)
					{
						yield return null;
					}
					this.SpawnCrate();
					GameManager.instance.levelGenerationProgress++;
					num = len;
				}
			}
			yield return null;
			GameManager.instance.SetWorldGenerationStep(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScaleTwice(800), "Generating ores");
			for (int len = 0; len < WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScaleTwice(800); len = num + 1)
			{
				if (len % 5 == 0)
				{
					yield return null;
				}
				this.SpawnOre();
				GameManager.instance.levelGenerationProgress++;
				num = len;
			}
			yield return null;
			if (!GameManager.instance.sniperSkillsSeed)
			{
				GameManager.instance.SetWorldGenerationStep(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScaleTwice(2000), "Generating trees");
				for (int len = 0; len < WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScaleTwice(2000); len = num + 1)
				{
					if (len % 5 == 0)
					{
						yield return null;
					}
					this.PlantTree();
					GameManager.instance.levelGenerationProgress++;
					num = len;
				}
			}
			yield return null;
			if (!GameManager.instance.nostalgiaSeed && !GameManager.instance.sniperSkillsSeed)
			{
				GameManager.instance.SetWorldGenerationStep(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScaleTwice(150), "Generating mushrooms");
				for (int len = 0; len < WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScaleTwice(150); len = num + 1)
				{
					if (len % 5 == 0)
					{
						yield return null;
					}
					this.PlantMushroom();
					GameManager.instance.levelGenerationProgress++;
					num = len;
				}
			}
			yield return null;
			if (!GameManager.instance.nostalgiaSeed && !GameManager.instance.sniperSkillsSeed)
			{
				GameManager.instance.SetWorldGenerationStep(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScaleTwice(1000), "Generating grass");
				for (int len = 0; len < WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScaleTwice(1000); len = num + 1)
				{
					if (len % 5 == 0)
					{
						yield return null;
					}
					this.PlantGrass();
					GameManager.instance.levelGenerationProgress++;
					num = len;
				}
			}
			yield return null;
			if (!GameManager.instance.sniperSkillsSeed)
			{
				GameManager.instance.SetWorldGenerationStep(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScaleTwice(9), "Generating life flowers");
				for (int len = 0; len < WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScaleTwice(9); len = num + 1)
				{
					if (len % 5 == 0)
					{
						yield return null;
					}
					this.PlantLifeFlower();
					GameManager.instance.levelGenerationProgress++;
					num = len;
				}
			}
			yield return null;
			if (!GameManager.instance.nostalgiaSeed && !GameManager.instance.sniperSkillsSeed)
			{
				GameManager.instance.SetWorldGenerationStep(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScaleTwice(700), "Generating plants");
				for (int len = 0; len < WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScaleTwice(700); len = num + 1)
				{
					if (len % 5 == 0)
					{
						yield return null;
					}
					this.SpawnPlant();
					GameManager.instance.levelGenerationProgress++;
					num = len;
				}
			}
			yield return null;
			if (!GameManager.instance.nostalgiaSeed && !GameManager.instance.sniperSkillsSeed)
			{
				GameManager.instance.SetWorldGenerationStep(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScaleTwice(450), "Generating bushes");
				for (int len = 0; len < WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScaleTwice(450); len = num + 1)
				{
					if (len % 5 == 0)
					{
						yield return null;
					}
					this.PlantBush();
					GameManager.instance.levelGenerationProgress++;
					num = len;
				}
			}
			yield return null;
			GameManager.instance.SetWorldGenerationStep(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScaleTwice(450), "Generating boss statues");
			for (int len = 0; len < WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScaleTwice(60); len = num + 1)
			{
				if (len % 5 == 0)
				{
					yield return null;
				}
				this.SpawnBossStatue();
				GameManager.instance.levelGenerationProgress++;
				num = len;
			}
			yield return null;
			this.GenerateCave();
			yield return null;
			GameManager.instance.ExitToMainMenu(true);
		}
		else
		{
			if (GameManager.instance.worldGameVersion < 13)
			{
				this.GenerateCave();
			}
			yield return WorldManager.instance.LoadLevelAsync();
			this.SpawnPlayer(new Vector2(GameManager.instance.respawnPos.x, GameManager.instance.respawnPos.y));
		}
		GameManager.instance.mainMenuSubmenu = 2;
		yield break;
	}

	// Token: 0x0600033F RID: 831 RVA: 0x00018259 File Offset: 0x00016459
	private void GenerateCave()
	{
		base.StartCoroutine("GenerateCaveAsync");
	}

	// Token: 0x06000340 RID: 832 RVA: 0x00018267 File Offset: 0x00016467
	private IEnumerator GenerateCaveAsync()
	{
		WorldManager.instance.SpawnBlock("mines_2", new Vector2(563.5f, 170f));
		GameManager.instance.SetWorldGenerationStep(40, "Generating big stone clusters");
		for (int i = 0; i < 5; i++)
		{
			Vector2 vector = new Vector2((float)this.rng.Next(560, 660), (float)this.rng.Next(120, 220));
			if (Vector2.Distance(vector, this.minesEntrancePosition) > 4f)
			{
				this.SpawnStructure(GameManager.instance.structureRegistryReference.FindStructureByInternalIdentifier("bigstonecluster"), vector);
			}
		}
		yield return null;
		GameManager.instance.SetWorldGenerationStep(40, "Generating cave chests");
		for (int j = 0; j < 40; j++)
		{
			this.SpawnCaveChest();
		}
		yield return null;
		GameManager.instance.SetWorldGenerationStep(150, "Generating rock trees");
		for (int k = 0; k < 150; k++)
		{
			this.PlantRockTree();
		}
		yield return null;
		GameManager.instance.SetWorldGenerationStep(400, "Generating cave ores");
		for (int l = 0; l < 400; l++)
		{
			this.SpawnCaveOre();
		}
		yield return null;
		GameManager.instance.SetWorldGenerationStep(150, "Generating emerald mushrooms");
		for (int m = 0; m < 150; m++)
		{
			this.PlantEmeraldMushroom();
		}
		yield return null;
		GameManager.instance.SetWorldGenerationStep(1, "Finalizing");
		yield break;
	}

	// Token: 0x06000341 RID: 833 RVA: 0x00018278 File Offset: 0x00016478
	private void Update()
	{
		if (!GameManager.instance.generatingWorld)
		{
			if (Random.Range(1, 1001) == 1 && !GameManager.instance.IsNight() && !GameManager.instance.nostalgiaSeed && !GameManager.instance.sniperSkillsSeed)
			{
				switch (Random.Range(1, 4))
				{
				case 1:
					this.PlantMushroom();
					break;
				case 2:
					this.PlantGrass();
					break;
				case 3:
					this.SpawnPlant();
					break;
				case 4:
					this.PlantBush();
					break;
				case 5:
					this.PlantEmeraldMushroom();
					break;
				}
			}
			if (Random.Range(1, 1901) == 1 && GameManager.instance.IsNight())
			{
				this.PlantLifeFlower();
			}
		}
	}

	// Token: 0x06000342 RID: 834 RVA: 0x00018334 File Offset: 0x00016534
	public void SpawnPlayer(Vector2 position)
	{
		this.player = Resources.Load<GameObject>("Player");
		GameObject gameObject = Object.Instantiate<GameObject>(this.player);
		gameObject.transform.position = position;
		GameManager.instance.playerMovement = gameObject.GetComponent<PlayerMovement>();
		GameManager.instance.generatingWorld = false;
	}

	// Token: 0x06000343 RID: 835 RVA: 0x0001838C File Offset: 0x0001658C
	private void RandSpawnStructure(Structure structure)
	{
		Vector2 vector = new Vector2((float)this.rng.Next(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-80), WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(81)), (float)this.rng.Next(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-80), WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(81)));
		if (Vector2.Distance(vector, this.minesEntrancePosition) > 4f)
		{
			this.SpawnStructure(structure, vector);
		}
	}

	// Token: 0x06000344 RID: 836 RVA: 0x00018418 File Offset: 0x00016618
	private void SpawnChest()
	{
		Vector2 vector = new Vector2((float)this.rng.Next(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-80), WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(81)), (float)this.rng.Next(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-80), WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(81))) + Vector2.right * 0.5f;
		bool flag = this.rng.Next(1, 4) == 1;
		if (!GameManager.instance.blockPositions.Contains(vector) && !GameManager.instance.floorPositions.Contains(vector) && !GameManager.instance.treePositions.Contains(vector) && Vector2.Distance(vector, this.minesEntrancePosition) > 4f)
		{
			GameObject gameObject = WorldManager.instance.SpawnBlock("chest", vector);
			if (gameObject != null)
			{
				Chest component = gameObject.GetComponent<Chest>();
				PlacedBlock component2 = gameObject.GetComponent<PlacedBlock>();
				component2.blockMetadata.Clear();
				component2.blockMetadata.Add("");
				component2.blockMetadata.Add("0");
				component2.blockMetadata.Add(flag ? "locked" : string.Empty);
				int num = this.rng.Next(3, 12);
				for (int i = 0; i < num; i++)
				{
					int num2 = this.rng.Next(0, this.chestItems.Length);
					component.chestInventory[this.rng.Next(0, 15)] = new InventorySlot
					{
						ITEM_ID = this.chestItems[num2],
						ITEM_STACK = this.ReturnStack(this.chestItems[num2])
					};
				}
				if (flag)
				{
					int num3 = this.rng.Next(1, 4);
					for (int j = 0; j < num3; j++)
					{
						int num4 = this.rng.Next(0, this.rareChestItems.Length);
						component.chestInventory[this.rng.Next(0, 15)] = new InventorySlot
						{
							ITEM_ID = this.rareChestItems[num4],
							ITEM_STACK = this.ReturnStack(this.rareChestItems[num4])
						};
					}
				}
			}
		}
	}

	// Token: 0x06000345 RID: 837 RVA: 0x00018674 File Offset: 0x00016874
	private void SpawnCaveChest()
	{
		Vector2 vector = new Vector2((float)this.rng.Next(560, 660), (float)this.rng.Next(120, 220)) + Vector2.right * 0.5f;
		if (!GameManager.instance.blockPositions.Contains(vector) && !GameManager.instance.floorPositions.Contains(vector) && !GameManager.instance.floorPositions.Contains(vector) && !GameManager.instance.treePositions.Contains(vector) && Vector2.Distance(vector, this.minesEntrancePosition) > 4f)
		{
			GameObject gameObject = WorldManager.instance.SpawnBlock("chest", vector);
			if (gameObject != null)
			{
				Chest component = gameObject.GetComponent<Chest>();
				PlacedBlock component2 = gameObject.GetComponent<PlacedBlock>();
				component2.blockMetadata.Clear();
				component2.blockMetadata.Add("");
				component2.blockMetadata.Add("0");
				component2.blockMetadata.Add("locked");
				int num = this.rng.Next(6, 15);
				for (int i = 0; i < num; i++)
				{
					if (this.rng.Next(1, 25) == 1)
					{
						int num2 = this.rng.Next(0, this.caveChestItems.Length);
						component.chestInventory[this.rng.Next(0, 15)] = new InventorySlot
						{
							ITEM_ID = this.caveChestItems[num2],
							ITEM_STACK = this.ReturnStack(this.caveChestItems[num2])
						};
					}
					else
					{
						int num3 = this.rng.Next(0, this.chestItems.Length);
						component.chestInventory[this.rng.Next(0, 15)] = new InventorySlot
						{
							ITEM_ID = this.chestItems[num3],
							ITEM_STACK = this.ReturnStack(this.chestItems[num3])
						};
					}
				}
			}
		}
	}

	// Token: 0x06000346 RID: 838 RVA: 0x00018888 File Offset: 0x00016A88
	private int ReturnStack(string i_id)
	{
		return Mathf.Clamp(this.rng.Next(1, 11), 1, GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(i_id).stackLimit);
	}

	// Token: 0x06000347 RID: 839 RVA: 0x000188B4 File Offset: 0x00016AB4
	private void SpawnMinesEntrance()
	{
		Vector2 pos = new Vector2((float)this.rng.Next(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-80), WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(81)), (float)this.rng.Next(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-80), WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(81)));
		this.SpawnStructure(GameManager.instance.structureRegistryReference.FindStructureByInternalIdentifier("mines_entrance"), pos);
		this.minesEntrancePosition = pos;
	}

	// Token: 0x06000348 RID: 840 RVA: 0x00018948 File Offset: 0x00016B48
	private void SpawnCrate()
	{
		Vector2 vector = new Vector2((float)this.rng.Next(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-160), WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(161)), (float)this.rng.Next(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-140), WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(141))) + Vector2.right * 0.5f;
		if (!GameManager.instance.blockPositions.Contains(vector) && !GameManager.instance.floorPositions.Contains(vector) && !GameManager.instance.treePositions.Contains(vector) && Vector2.Distance(vector, this.minesEntrancePosition) > 4f)
		{
			WorldManager.instance.SpawnBlock("crate", vector);
		}
	}

	// Token: 0x06000349 RID: 841 RVA: 0x00018A34 File Offset: 0x00016C34
	private void SpawnSquariumAltar()
	{
		Vector2 vector = new Vector2(0f, 5f) + Vector2.right * 0.5f;
		if (!GameManager.instance.blockPositions.Contains(vector) && !GameManager.instance.floorPositions.Contains(vector) && !GameManager.instance.treePositions.Contains(vector))
		{
			WorldManager.instance.SpawnBlock("squariumaltar", vector);
		}
	}

	// Token: 0x0600034A RID: 842 RVA: 0x00018AAC File Offset: 0x00016CAC
	private void SpawnOre()
	{
		Vector2 vector = new Vector2((float)this.rng.Next(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-160), WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(161)), (float)this.rng.Next(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-140), WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(141))) + Vector2.right * 0.5f;
		if (!GameManager.instance.blockPositions.Contains(vector) && !GameManager.instance.floorPositions.Contains(vector) && !GameManager.instance.treePositions.Contains(vector) && Vector2.Distance(vector, this.minesEntrancePosition) > 4f)
		{
			WorldManager.instance.SpawnBlock(this.ReturnRandomOre(), vector);
		}
	}

	// Token: 0x0600034B RID: 843 RVA: 0x00018B98 File Offset: 0x00016D98
	private void SpawnCaveOre()
	{
		Vector2 vector = new Vector2((float)this.rng.Next(560, 660), (float)this.rng.Next(120, 220)) + Vector2.right * 0.5f;
		if (!GameManager.instance.blockPositions.Contains(vector) && !GameManager.instance.floorPositions.Contains(vector) && !GameManager.instance.treePositions.Contains(vector) && Vector2.Distance(vector, this.minesEntrancePosition) > 4f)
		{
			WorldManager.instance.SpawnBlock(this.ReturnRandomCaveOre(), vector);
		}
	}

	// Token: 0x0600034C RID: 844 RVA: 0x00018C44 File Offset: 0x00016E44
	private void PlantTree()
	{
		Vector2 vector = new Vector2((float)this.rng.Next(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-160), WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(161)), (float)this.rng.Next(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-140), WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(141))) + Vector2.right * 0.5f;
		if (!GameManager.instance.treePositions.Contains(vector) && !GameManager.instance.blockPositions.Contains(vector) && !GameManager.instance.floorPositions.Contains(vector) && Vector2.Distance(vector, this.minesEntrancePosition) > 4f)
		{
			WorldManager.instance.SpawnTree(this.ReturnTree(), true, vector);
		}
	}

	// Token: 0x0600034D RID: 845 RVA: 0x00018D30 File Offset: 0x00016F30
	private void PlantRockTree()
	{
		Vector2 vector = new Vector2((float)this.rng.Next(560, 660), (float)this.rng.Next(120, 220)) + Vector2.right * 0.5f;
		if (!GameManager.instance.treePositions.Contains(vector) && !GameManager.instance.blockPositions.Contains(vector) && !GameManager.instance.floorPositions.Contains(vector) && Vector2.Distance(vector, this.minesEntrancePosition) > 4f)
		{
			WorldManager.instance.SpawnTree("rocktree", true, vector);
		}
	}

	// Token: 0x0600034E RID: 846 RVA: 0x00018DDC File Offset: 0x00016FDC
	private void PlantGrass()
	{
		Vector2 vector = new Vector2((float)this.rng.Next(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-160), WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(161)), (float)this.rng.Next(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-140), WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(141))) + Vector2.right * 0.5f;
		if (!GameManager.instance.blockPositions.Contains(vector) && !GameManager.instance.floorPositions.Contains(vector) && !GameManager.instance.treePositions.Contains(vector))
		{
			WorldManager.instance.SpawnBlock("tallgrass", vector);
		}
	}

	// Token: 0x0600034F RID: 847 RVA: 0x00018EB4 File Offset: 0x000170B4
	private void PlantMushroom()
	{
		Vector2 vector = new Vector2((float)this.rng.Next(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-160), WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(161)), (float)this.rng.Next(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-140), WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(141))) + Vector2.right * 0.5f;
		if (!GameManager.instance.blockPositions.Contains(vector) && !GameManager.instance.floorPositions.Contains(vector) && !GameManager.instance.treePositions.Contains(vector))
		{
			WorldManager.instance.SpawnBlock("mushroom", vector);
		}
	}

	// Token: 0x06000350 RID: 848 RVA: 0x00018F8C File Offset: 0x0001718C
	private void PlantEmeraldMushroom()
	{
		Vector2 vector = new Vector2((float)this.rng.Next(560, 660), (float)this.rng.Next(120, 220)) + Vector2.right * 0.5f;
		if (!GameManager.instance.blockPositions.Contains(vector) && !GameManager.instance.floorPositions.Contains(vector) && !GameManager.instance.treePositions.Contains(vector))
		{
			WorldManager.instance.SpawnBlock("emeraldmushroom", vector);
		}
	}

	// Token: 0x06000351 RID: 849 RVA: 0x00019024 File Offset: 0x00017224
	private void PlantLifeFlower()
	{
		Vector2 vector = new Vector2((float)this.rng.Next(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-80), WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(81)), (float)this.rng.Next(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-80), WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(81))) + Vector2.right * 0.5f;
		if (!GameManager.instance.blockPositions.Contains(vector) && !GameManager.instance.floorPositions.Contains(vector) && !GameManager.instance.treePositions.Contains(vector))
		{
			WorldManager.instance.SpawnBlock("lifeflower", vector);
		}
	}

	// Token: 0x06000352 RID: 850 RVA: 0x000190F0 File Offset: 0x000172F0
	private void SpawnPlant()
	{
		Vector2 vector = new Vector2((float)this.rng.Next(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-160), WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(161)), (float)this.rng.Next(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-140), WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(141))) + Vector2.right * 0.5f;
		if (!GameManager.instance.blockPositions.Contains(vector) && !GameManager.instance.floorPositions.Contains(vector) && !GameManager.instance.treePositions.Contains(vector))
		{
			WorldManager.instance.SpawnBlock(this.ReturnFlower(), vector);
		}
	}

	// Token: 0x06000353 RID: 851 RVA: 0x000191C8 File Offset: 0x000173C8
	private void PlantBush()
	{
		Vector2 vector = new Vector2((float)this.rng.Next(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-160), WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(161)), (float)this.rng.Next(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-140), WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(141))) + Vector2.right * 0.5f;
		if (!GameManager.instance.blockPositions.Contains(vector) && !GameManager.instance.floorPositions.Contains(vector) && !GameManager.instance.treePositions.Contains(vector))
		{
			WorldManager.instance.SpawnBlock("bush", vector);
		}
	}

	// Token: 0x06000354 RID: 852 RVA: 0x000192A0 File Offset: 0x000174A0
	private void SpawnBossStatue()
	{
		Vector2 vector = new Vector2((float)this.rng.Next(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-80), WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(81)), (float)this.rng.Next(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-80), WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(81))) + Vector2.right * 0.5f;
		if (!GameManager.instance.blockPositions.Contains(vector) && !GameManager.instance.floorPositions.Contains(vector) && !GameManager.instance.treePositions.Contains(vector) && Vector2.Distance(vector, this.minesEntrancePosition) > 10f)
		{
			WorldManager.instance.SpawnBlock(this.ReturnStatue(), vector);
		}
	}

	// Token: 0x06000355 RID: 853 RVA: 0x00019380 File Offset: 0x00017580
	public void SpawnTombstone()
	{
		Vector2 vector = Vector2Int.RoundToInt(GameManager.instance.playerPos) + Vector2.right * 0.5f;
		if (!GameManager.instance.blockPositions.Contains(vector) && !GameManager.instance.treePositions.Contains(vector))
		{
			WorldManager.instance.SpawnBlock("tombstone", vector);
		}
	}

	// Token: 0x06000356 RID: 854 RVA: 0x000193EC File Offset: 0x000175EC
	public void SpawnStructure(Structure structure, Vector2 pos)
	{
		foreach (StructureBlock structureBlock in structure.structureBlocks)
		{
			switch (structureBlock.blockType)
			{
			case StructureBlockType.Block:
			{
				GameObject gameObject = WorldManager.instance.SpawnBlock(this.ReturnStructureBlockKey(structureBlock.key), pos + structureBlock.pos + Vector2.right * 0.5f);
				if (gameObject != null)
				{
					PlacedBlock component = gameObject.GetComponent<PlacedBlock>();
					for (int i = 0; i < structureBlock.blockMetadata.Length; i++)
					{
						component.blockMetadata.Add(structureBlock.blockMetadata[i]);
					}
				}
				break;
			}
			case StructureBlockType.Floor:
				WorldManager.instance.SpawnFloor(structureBlock.key, pos + structureBlock.pos + Vector2.right * 0.5f);
				break;
			case StructureBlockType.Tree:
				WorldManager.instance.SpawnTree(structureBlock.key, true, pos + structureBlock.pos + Vector2.right * 0.5f);
				break;
			}
		}
	}

	// Token: 0x06000357 RID: 855 RVA: 0x00019548 File Offset: 0x00017748
	private int NegativePositiveOne(Random rng)
	{
		if (rng.Next(1, 3) == 1)
		{
			return -1;
		}
		return 1;
	}

	// Token: 0x06000358 RID: 856 RVA: 0x00019558 File Offset: 0x00017758
	private string ReturnStructureBlockKey(string key)
	{
		if (key != null)
		{
			if (key == "randOre")
			{
				return this.ReturnRandomOre();
			}
			if (key == "randSolidOre")
			{
				return this.ReturnRandomSolidOre();
			}
		}
		return key;
	}

	// Token: 0x06000359 RID: 857 RVA: 0x00019588 File Offset: 0x00017788
	private string ReturnRandomOre()
	{
		if (this.rng.Next(1, 41) == 1)
		{
			return "goldore";
		}
		if (this.rng.Next(1, 26) == 1)
		{
			return "silverore";
		}
		if (this.rng.Next(1, 16) == 1)
		{
			return "ironore";
		}
		if (this.rng.Next(1, 6) == 1)
		{
			return "copperore";
		}
		return "stoneore";
	}

	// Token: 0x0600035A RID: 858 RVA: 0x000195F8 File Offset: 0x000177F8
	private string ReturnRandomSolidOre()
	{
		if (this.rng.Next(1, 41) == 1)
		{
			return "solidgoldore";
		}
		if (this.rng.Next(1, 26) == 1)
		{
			return "solidsilverore";
		}
		if (this.rng.Next(1, 16) == 1)
		{
			return "solidironore";
		}
		if (this.rng.Next(1, 6) == 1)
		{
			return "solidcopperore";
		}
		return "stone";
	}

	// Token: 0x0600035B RID: 859 RVA: 0x00019668 File Offset: 0x00017868
	private string ReturnRandomCaveOre()
	{
		if (this.rng.Next(1, 21) == 1)
		{
			return "rubyore";
		}
		if (this.rng.Next(1, 21) == 1)
		{
			return "chromiumore";
		}
		if (this.rng.Next(1, 21) == 1)
		{
			return "titaniumore";
		}
		if (this.rng.Next(1, 21) == 1)
		{
			return "pyriumore";
		}
		if (this.rng.Next(1, 17) == 1)
		{
			return "goldore";
		}
		if (this.rng.Next(1, 12) == 1)
		{
			return "silverore";
		}
		if (this.rng.Next(1, 8) == 1)
		{
			return "ironore";
		}
		if (this.rng.Next(1, 5) == 1)
		{
			return "copperore";
		}
		return "stoneore";
	}

	// Token: 0x0600035C RID: 860 RVA: 0x00019730 File Offset: 0x00017930
	private string ReturnFlower()
	{
		if (this.rng.Next(1, 11) == 1)
		{
			return "sunflower";
		}
		switch (this.rng.Next(1, 9))
		{
		case 1:
			return "flower1";
		case 2:
			return "flower2";
		case 3:
			return "flower3";
		case 4:
			return "flower4";
		case 5:
			return "flower5";
		case 6:
			return "flower6";
		case 7:
			return "deadbush";
		case 8:
			return "deco1";
		default:
			return "mushroom";
		}
	}

	// Token: 0x0600035D RID: 861 RVA: 0x000197C4 File Offset: 0x000179C4
	private string ReturnStatue()
	{
		int num = this.rng.Next(1, 3);
		if (num == 1)
		{
			return "statue_ss2event";
		}
		if (num != 2)
		{
			return "statueboss";
		}
		return "statueboss";
	}

	// Token: 0x0600035E RID: 862 RVA: 0x000197FC File Offset: 0x000179FC
	private string ReturnTree()
	{
		if (GameManager.instance.nostalgiaSeed)
		{
			return "nostalgiatree";
		}
		if (this.rng.Next(1, 4) == 1)
		{
			return this.trees[this.rng.Next(0, this.trees.Length)];
		}
		return this.trees[0];
	}

	// Token: 0x04000419 RID: 1049
	[SerializeField]
	private string[] trees;

	// Token: 0x0400041A RID: 1050
	private GameObject player;

	// Token: 0x0400041B RID: 1051
	public string[] chestItems;

	// Token: 0x0400041C RID: 1052
	public string[] rareChestItems;

	// Token: 0x0400041D RID: 1053
	public string[] caveChestItems;

	// Token: 0x0400041E RID: 1054
	public Random rng;

	// Token: 0x0400041F RID: 1055
	[HideInInspector]
	public Vector2 minesEntrancePosition;

	// Token: 0x04000420 RID: 1056
	public Chest dimensionalCrate;
}
