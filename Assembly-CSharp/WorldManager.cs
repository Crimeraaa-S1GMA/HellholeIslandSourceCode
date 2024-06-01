using System;
using System.Collections;
using System.Collections.Generic;
using TrashTake.BuildInfo;
using UnityEngine;

// Token: 0x020000B0 RID: 176
public class WorldManager : MonoBehaviour
{
	// Token: 0x06000360 RID: 864 RVA: 0x00019857 File Offset: 0x00017A57
	private void Awake()
	{
		if (WorldManager.instance == null)
		{
			WorldManager.instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
			this.loadedWorld = null;
			return;
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06000361 RID: 865 RVA: 0x0001988C File Offset: 0x00017A8C
	public void MinesTeleportation()
	{
		if (GameManager.instance.gameState == GameState.Playing)
		{
			AchievementManager.instance.AddAchievement(160462);
			GameManager.instance.bed = null;
			GameManager.instance.chair = null;
			foreach (EnemyBase enemyBase in GameManager.instance.enemyList)
			{
				Object.Destroy(enemyBase.gameObject);
			}
			if (GameManager.instance.isInMines)
			{
				GameManager.instance.isInMines = false;
				GameManager.instance.playerMovement.transform.position = Object.FindObjectOfType<WorldGeneration>().minesEntrancePosition + Vector2.down * 4f;
				Camera.main.transform.position = GameManager.instance.respawnPos;
				return;
			}
			GameManager.instance.isInMines = true;
			GameManager.instance.playerMovement.transform.position = new Vector2(566.5f, 170f);
			Camera.main.transform.position = new Vector2(610f, 170f);
		}
	}

	// Token: 0x06000362 RID: 866 RVA: 0x000199E0 File Offset: 0x00017BE0
	public IEnumerator LoadLevelAsync()
	{
		GameManager.instance.highlightableBlocks.Clear();
		GameManager.instance.wirePositions.Clear();
		GameManager.instance.waterPositions.Clear();
		GameManager.instance.bedHalfPositions.Clear();
		GameManager.instance.leafBlockPositions.Clear();
		GameManager.instance.sunflowerPositions.Clear();
		GameManager.instance.spawnBlockerPositions.Clear();
		GameManager.instance.enemyList.Clear();
		GameManager.instance.farmlands.Clear();
		GameManager.instance.placedBlocks.Clear();
		GameManager.instance.placedFloors.Clear();
		GameManager.instance.placedTrees.Clear();
		GameManager.instance.enemiesInWorld = 0;
		GameManager.instance.invasionEvent = Invasion.None;
		GameManager.instance.invasionEnemiesLeft = 0;
		GameManager.instance.ResetDamageSource();
		GameManager.instance.ResetInventory(false);
		for (int i = 0; i < this.loadedWorld.inventory.Count; i++)
		{
			if (i < this.loadedWorld.inventory.Count)
			{
				if (this.loadedWorld.versionId >= 100)
				{
					GameManager.instance.inventory[i] = this.loadedWorld.inventory[i];
				}
				else
				{
					GameManager.instance.inventory[i] = new InventorySlot
					{
						ITEM_ID = GameManager.instance.itemRegistryReference.items[int.Parse(this.loadedWorld.inventory[i].ITEM_ID)].internalIdentifier,
						ITEM_STACK = this.loadedWorld.inventory[i].ITEM_STACK
					};
				}
			}
		}
		yield return null;
		GameManager.instance.defeatBosses.Clear();
		for (int j = 0; j < this.loadedWorld.defeatBosses.Count; j++)
		{
			GameManager.instance.defeatBosses.Add(this.loadedWorld.defeatBosses[j]);
		}
		yield return null;
		GameManager.instance.statusEffects.Clear();
		for (int k = 0; k < this.loadedWorld.statusEffects.Count; k++)
		{
			GameManager.instance.statusEffects.Add(this.loadedWorld.statusEffects[k]);
		}
		yield return null;
		foreach (SaveBlock saveBlock in this.loadedWorld.blocksPlaced)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.blockPrefab);
			gameObject.transform.position = new Vector2(saveBlock.x, saveBlock.y);
			PlacedBlock component = gameObject.GetComponent<PlacedBlock>();
			component.blockMetadata.Clear();
			foreach (string item in saveBlock.blockMetadata)
			{
				component.blockMetadata.Add(item);
			}
			component.PrepareBlock(saveBlock.blockKey);
			if (saveBlock.blockKey == "mines_0")
			{
				Object.FindObjectOfType<WorldGeneration>().minesEntrancePosition = new Vector2(saveBlock.x, saveBlock.y);
			}
			GameManager.instance.blockPositions.Add(new Vector2(saveBlock.x, saveBlock.y));
		}
		yield return null;
		foreach (SaveFloor saveFloor in this.loadedWorld.floorsPlaced)
		{
			GameObject gameObject2 = Object.Instantiate<GameObject>(this.floorPrefab);
			gameObject2.transform.position = new Vector2(saveFloor.x, saveFloor.y);
			gameObject2.GetComponent<PlacedFloor>().PrepareFloor(saveFloor.floorKey);
			GameManager.instance.floorPositions.Add(new Vector2(saveFloor.x, saveFloor.y));
		}
		yield return null;
		foreach (SaveTree saveTree in this.loadedWorld.treesPlaced)
		{
			GameObject gameObject3 = Object.Instantiate<GameObject>(this.treePrefab);
			gameObject3.transform.position = new Vector2(saveTree.x, saveTree.y);
			gameObject3.GetComponent<PlacedTree>().PrepareTree(saveTree.treeKey, true);
			GameManager.instance.treePositions.Add(new Vector2(saveTree.x, saveTree.y));
		}
		yield return null;
		foreach (SaveChest saveChest in this.loadedWorld.chestsPlaced)
		{
			GameObject gameObject4 = Object.Instantiate<GameObject>(this.blockPrefab);
			gameObject4.transform.position = new Vector2(saveChest.x, saveChest.y);
			PlacedBlock component2 = gameObject4.GetComponent<PlacedBlock>();
			component2.blockMetadata.Clear();
			foreach (string item2 in saveChest.blockMetadata)
			{
				component2.blockMetadata.Add(item2);
			}
			component2.PrepareBlock("chest");
			List<InventorySlot> list = new List<InventorySlot>();
			foreach (InventorySlot inventorySlot in saveChest.inventory)
			{
				if (this.loadedWorld.versionId < 100)
				{
					list.Add(new InventorySlot
					{
						ITEM_ID = GameManager.instance.itemRegistryReference.items[int.Parse(inventorySlot.ITEM_ID)].internalIdentifier,
						ITEM_STACK = inventorySlot.ITEM_STACK
					});
				}
				else
				{
					list.Add(new InventorySlot
					{
						ITEM_ID = inventorySlot.ITEM_ID,
						ITEM_STACK = inventorySlot.ITEM_STACK
					});
				}
			}
			gameObject4.GetComponent<Chest>().chestInventory = list;
			GameManager.instance.blockPositions.Add(new Vector2(saveChest.x, saveChest.y));
		}
		yield return null;
		yield break;
	}

	// Token: 0x06000363 RID: 867 RVA: 0x000199EF File Offset: 0x00017BEF
	public IEnumerator SaveWorld()
	{
		GameManager.instance.displayAutosave = true;
		this.loadedWorld.inventory.Clear();
		for (int i = 0; i < GameManager.instance.inventory.Count; i++)
		{
			this.loadedWorld.inventory.Add(GameManager.instance.inventory[i]);
		}
		yield return null;
		this.loadedWorld.defeatBosses.Clear();
		for (int j = 0; j < GameManager.instance.defeatBosses.Count; j++)
		{
			this.loadedWorld.defeatBosses.Add(GameManager.instance.defeatBosses[j]);
		}
		yield return null;
		this.loadedWorld.statusEffects.Clear();
		for (int k = 0; k < GameManager.instance.statusEffects.Count; k++)
		{
			this.loadedWorld.statusEffects.Add(GameManager.instance.statusEffects[k]);
		}
		yield return null;
		this.loadedWorld.seed = WorldPolicy.worldSeed;
		this.loadedWorld.playerPosX = GameManager.instance.respawnPos.x;
		this.loadedWorld.playerPosY = GameManager.instance.respawnPos.y;
		this.loadedWorld.creative = WorldPolicy.creativeMode;
		this.loadedWorld.keepInventory = WorldPolicy.keepInventory;
		this.loadedWorld.spawning = WorldPolicy.spawnMobs;
		this.loadedWorld.advancedTooltips = WorldPolicy.advancedTooltips;
		this.loadedWorld.hardcoreMode = WorldPolicy.hardcoreMode;
		this.loadedWorld.infiniteBuildRange = WorldPolicy.infiniteBuildRange;
		this.loadedWorld.allowSwitchingGamemodes = WorldPolicy.allowSwitchingGamemodes;
		this.loadedWorld.hellholeMode = WorldPolicy.hellholeMode;
		this.loadedWorld.trainwreckMode = WorldPolicy.trainwreckMode;
		this.loadedWorld.time = GameManager.instance.fullTime;
		this.loadedWorld.rain = GameManager.instance.rain;
		this.loadedWorld.health = GameManager.instance.health;
		this.loadedWorld.extraHealth = GameManager.instance.extraHealth;
		this.loadedWorld.money = GameManager.instance.money;
		this.loadedWorld.versionId = TrashTakeBuildInfo.GAME_VERSION_ID;
		this.loadedWorld.dayNightCycle = WorldPolicy.dayNightCycle;
		this.loadedWorld.rotativeCameraFeature = GameManager.instance.rotativeCameraFeature;
		this.loadedWorld.nostalgiaSeed = GameManager.instance.nostalgiaSeed;
		this.loadedWorld.sniperSkillsSeed = GameManager.instance.sniperSkillsSeed;
		this.loadedWorld.kyivSeed = GameManager.instance.kyivSeed;
		this.loadedWorld.experimentalFeatures = WorldPolicy.experimentalFeatures;
		yield return null;
		this.SaveLevelBlocks();
		yield return null;
		this.SaveLevelFloors();
		yield return null;
		this.SaveLevelTrees();
		if (this.loadedWorld.storageName == null || this.loadedWorld.versionId < 100)
		{
			this.loadedWorld.storageName = this.loadedWorld.name;
		}
		PlayerPrefs.SetString(this.loadedWorld.storageName, JsonUtility.ToJson(this.loadedWorld));
		GameManager.instance.SaveAvailableLevels();
		GameManager.instance.displayAutosave = false;
		yield break;
	}

	// Token: 0x06000364 RID: 868 RVA: 0x00019A00 File Offset: 0x00017C00
	public void SaveLevelBlocks()
	{
		PlacedBlock[] array = Object.FindObjectsOfType<PlacedBlock>();
		this.loadedWorld.blocksPlaced.Clear();
		this.loadedWorld.chestsPlaced.Clear();
		foreach (PlacedBlock placedBlock in array)
		{
			if (placedBlock.blockKey == "chest")
			{
				this.loadedWorld.chestsPlaced.Add(new SaveChest
				{
					x = placedBlock.transform.position.x,
					y = placedBlock.transform.position.y,
					inventory = placedBlock.GetComponent<Chest>().chestInventory,
					blockMetadata = placedBlock.blockMetadata
				});
			}
			else
			{
				this.loadedWorld.blocksPlaced.Add(new SaveBlock
				{
					x = placedBlock.transform.position.x,
					y = placedBlock.transform.position.y,
					blockKey = placedBlock.blockKey,
					blockMetadata = placedBlock.blockMetadata
				});
			}
		}
	}

	// Token: 0x06000365 RID: 869 RVA: 0x00019B18 File Offset: 0x00017D18
	public void SaveLevelFloors()
	{
		PlacedFloor[] array = Object.FindObjectsOfType<PlacedFloor>();
		this.loadedWorld.floorsPlaced.Clear();
		foreach (PlacedFloor placedFloor in array)
		{
			this.loadedWorld.floorsPlaced.Add(new SaveFloor
			{
				x = placedFloor.transform.position.x,
				y = placedFloor.transform.position.y,
				floorKey = placedFloor.floorKey
			});
		}
	}

	// Token: 0x06000366 RID: 870 RVA: 0x00019B9C File Offset: 0x00017D9C
	public void SaveLevelTrees()
	{
		PlacedTree[] array = Object.FindObjectsOfType<PlacedTree>();
		this.loadedWorld.treesPlaced.Clear();
		foreach (PlacedTree placedTree in array)
		{
			this.loadedWorld.treesPlaced.Add(new SaveTree
			{
				x = placedTree.transform.position.x,
				y = placedTree.transform.position.y,
				treeKey = placedTree.treeKey
			});
		}
	}

	// Token: 0x06000367 RID: 871 RVA: 0x00019C20 File Offset: 0x00017E20
	public GameObject SpawnBlock(string blockId, Vector2 pos)
	{
		if (!GameManager.instance.blockPositions.Contains(pos) && !GameManager.instance.treePositions.Contains(pos))
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.blockPrefab);
			gameObject.transform.position = pos;
			GameManager.instance.blockPositions.Add(pos);
			gameObject.GetComponent<PlacedBlock>().PrepareBlock(blockId);
			return gameObject;
		}
		return null;
	}

	// Token: 0x06000368 RID: 872 RVA: 0x00019C8C File Offset: 0x00017E8C
	public GameObject SpawnFloor(string floorId, Vector2 pos)
	{
		if (!GameManager.instance.floorPositions.Contains(pos) && !GameManager.instance.treePositions.Contains(pos))
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.floorPrefab);
			gameObject.transform.position = pos;
			GameManager.instance.floorPositions.Add(pos);
			gameObject.GetComponent<PlacedFloor>().PrepareFloor(floorId);
			return gameObject;
		}
		return null;
	}

	// Token: 0x06000369 RID: 873 RVA: 0x00019CF8 File Offset: 0x00017EF8
	public GameObject SpawnTree(string treeId, bool matureTree, Vector2 pos)
	{
		if (!GameManager.instance.blockPositions.Contains(pos) && !GameManager.instance.floorPositions.Contains(pos) && !GameManager.instance.treePositions.Contains(pos))
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.treePrefab);
			gameObject.transform.position = pos;
			GameManager.instance.treePositions.Add(pos);
			if (treeId == "standardtree" && GameManager.instance.nostalgiaSeed)
			{
				gameObject.GetComponent<PlacedTree>().PrepareTree("nostalgiatree", matureTree);
			}
			else
			{
				gameObject.GetComponent<PlacedTree>().PrepareTree(treeId, matureTree);
			}
			return gameObject;
		}
		return null;
	}

	// Token: 0x04000421 RID: 1057
	public static WorldManager instance;

	// Token: 0x04000422 RID: 1058
	[HideInInspector]
	public World loadedWorld;

	// Token: 0x04000423 RID: 1059
	[HideInInspector]
	public bool isNewWorld = true;

	// Token: 0x04000424 RID: 1060
	public GameObject blockPrefab;

	// Token: 0x04000425 RID: 1061
	public GameObject floorPrefab;

	// Token: 0x04000426 RID: 1062
	public GameObject treePrefab;

	// Token: 0x04000427 RID: 1063
	public int worldIdToDelete;

	// Token: 0x04000428 RID: 1064
	[HideInInspector]
	public World worldToDelete;
}
