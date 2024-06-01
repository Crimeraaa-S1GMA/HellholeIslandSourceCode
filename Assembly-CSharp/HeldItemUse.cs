using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000050 RID: 80
public class HeldItemUse : MonoBehaviour
{
	// Token: 0x06000197 RID: 407 RVA: 0x0000D952 File Offset: 0x0000BB52
	private bool ReturnUsing()
	{
		if (GameManager.instance.ReturnSelectedItem().autoUse)
		{
			return Input.GetMouseButton(0);
		}
		return Input.GetMouseButtonDown(0);
	}

	// Token: 0x06000198 RID: 408 RVA: 0x0000D974 File Offset: 0x0000BB74
	private void Update()
	{
		if (this.ReturnUsing() && GameManager.instance.tooltipIndex == -1 && GameManager.instance.sign == null && GameManager.instance.ReturnSelectedSlot().ITEM_ID != "null" && GameManager.instance.itemCooldown <= 0f && GameManager.instance.selectedTeleporter == null && !GameManager.instance.hoveringButtonInv)
		{
			this.UseItem(GameManager.instance.ReturnSelectedItem().canUse, GameManager.instance.ReturnSelectedItem(), GameManager.instance.ReturnSelectedSlotId(), GameManager.instance.ReturnSelectedSlot());
		}
	}

	// Token: 0x06000199 RID: 409 RVA: 0x0000DA2C File Offset: 0x0000BC2C
	public void UseItem(bool canUse, Item selectedItem, int selectedSlotId, InventorySlot selectedSlot)
	{
		bool flag = canUse;
		if (GameManager.instance.EquippedAccesory("speed_glove") && !selectedItem.autoUse)
		{
			GameManager.instance.itemCooldown = selectedItem.useCooldown / 2f;
		}
		else
		{
			GameManager.instance.itemCooldown = selectedItem.useCooldown;
		}
		if (flag && selectedItem.ammoId != "null")
		{
			if (selectedItem.ammoId == "wood")
			{
				flag = (GameManager.instance.FirstItemIdInInventory(new List<string>
				{
					"wood_standard",
					"wood_birch",
					"wood_evergreen"
				}) != "null");
			}
			else
			{
				flag = (GameManager.instance.ItemCount(selectedItem.ammoId, false) > 0);
			}
		}
		if (flag && selectedItem.heal > 0)
		{
			flag = (GameManager.instance.health < GameManager.instance.ReturnMaxHealth() && !GameManager.instance.HasStatusEffect("full"));
			if (selectedItem.lifeFlower > 0)
			{
				flag = true;
			}
		}
		if (flag && selectedItem.statusEffectId != string.Empty)
		{
			if (selectedItem.statusEffectId == "return")
			{
				flag = (GameManager.instance.playerMovement != null && Vector2.Distance(GameManager.instance.respawnPos, GameManager.instance.playerPos) > 7f);
			}
			else
			{
				flag = !GameManager.instance.HasStatusEffect(selectedItem.statusEffectId);
			}
		}
		if (flag && !selectedItem.usableInMines)
		{
			flag = !GameManager.instance.isInMines;
		}
		if (flag && selectedItem.specialUsage == "MinesTeleporter")
		{
			flag = (GameManager.instance.boss == null);
		}
		if (flag && selectedItem.specialUsage == "PainVoid")
		{
			flag = !WorldPolicy.hellholeMode;
		}
		string specialUsage = selectedItem.specialUsage;
		if (specialUsage != null)
		{
			if (!(specialUsage == "Seed"))
			{
				if (!(specialUsage == "CropGrowth"))
				{
					if (!(specialUsage == "Hoe"))
					{
						if (!(specialUsage == "Pickaxe"))
						{
							if (!(specialUsage == "Hammer"))
							{
								if (specialUsage == "Axe")
								{
									PlacedTree placedTree = GameManager.instance.placedTrees.Find((PlacedTree w) => w.transform.position == GameManager.instance.selectionSquare);
									if (placedTree != null)
									{
										AudioManager.instance.Play("ChopTree");
										if (WorldPolicy.creativeMode)
										{
											placedTree.actualTreeHealth = 0f;
										}
										else
										{
											placedTree.actualTreeHealth -= selectedItem.toolPower;
										}
										placedTree.CheckForDeletion();
									}
								}
							}
							else
							{
								PlacedFloor placedFloor = GameManager.instance.placedFloors.Find((PlacedFloor w) => w.transform.position == GameManager.instance.selectionSquare);
								if (placedFloor != null)
								{
									AudioManager.instance.Play("BreakBlock");
									if (WorldPolicy.creativeMode)
									{
										placedFloor.actualWallHealth = 0f;
									}
									else
									{
										placedFloor.actualWallHealth -= selectedItem.toolPower;
									}
									placedFloor.CheckForDeletion();
								}
							}
						}
						else
						{
							PlacedBlock placedBlock = GameManager.instance.placedBlocks.Find((PlacedBlock b) => b.transform.position == GameManager.instance.selectionSquare);
							if (placedBlock != null)
							{
								AudioManager.instance.Play("BreakBlock");
								if (placedBlock.ReturnBlock().minimalToolPower <= (WorldPolicy.creativeMode ? 1.9f : selectedItem.toolPower))
								{
									placedBlock.TakeBlockDamage(selectedItem.toolPower);
								}
							}
						}
					}
					else if (flag)
					{
						flag = (!GameManager.instance.blockPositions.Contains(GameManager.instance.selectionSquare) && !GameManager.instance.floorPositions.Contains(GameManager.instance.selectionSquare) && !GameManager.instance.treePositions.Contains(GameManager.instance.selectionSquare) && !GameManager.instance.isInMines && !GameManager.instance.sniperSkillsSeed);
						if (flag)
						{
							flag = !GameManager.instance.bedHalfPositions.Contains(GameManager.instance.selectionSquare);
						}
						if (flag)
						{
							AudioManager.instance.Play("PlaceBlock");
							WorldManager.instance.SpawnBlock("farmland", GameManager.instance.selectionSquare);
						}
					}
				}
				else if (flag)
				{
					Farmland farmland = GameManager.instance.farmlands.Find((Farmland f) => f.transform.position == GameManager.instance.selectionSquare);
					flag = (farmland != null);
					if (flag)
					{
						flag = (farmland.ReturnCrop() != 0 && farmland.ReturnCropStage() < 3);
						if (flag)
						{
							AudioManager.instance.Play("PlaceBlock");
							farmland.Fertilize();
						}
					}
				}
			}
			else if (flag)
			{
				Farmland farmland2 = GameManager.instance.farmlands.Find((Farmland f) => f.transform.position == GameManager.instance.selectionSquare);
				flag = (farmland2 != null);
				if (flag)
				{
					flag = (farmland2.ReturnCrop() == 0 || farmland2.ReturnCropStage() == 0);
					if (flag)
					{
						AudioManager.instance.Play("PlaceBlock");
						string item_ID = selectedSlot.ITEM_ID;
						if (item_ID != null)
						{
							if (!(item_ID == "wheat_seeds"))
							{
								if (!(item_ID == "tomato_seeds"))
								{
									if (item_ID == "carrot_seeds")
									{
										farmland2.SetUpCrop(3, 0);
									}
								}
								else
								{
									farmland2.SetUpCrop(2, 0);
								}
							}
							else
							{
								farmland2.SetUpCrop(1, 0);
							}
						}
					}
				}
			}
		}
		if (flag && selectedItem.blockId != string.Empty)
		{
			switch (selectedItem.blockType)
			{
			case BlockType.Solid:
				flag = (!GameManager.instance.blockPositions.Contains(GameManager.instance.selectionSquare) && !GameManager.instance.treePositions.Contains(GameManager.instance.selectionSquare));
				if (flag && selectedItem.specialUsage == "Bed")
				{
					flag = (!GameManager.instance.blockPositions.Contains(GameManager.instance.selectionSquare + Vector2.right) && !GameManager.instance.treePositions.Contains(GameManager.instance.selectionSquare + Vector2.right) && !GameManager.instance.bedHalfPositions.Contains(GameManager.instance.selectionSquare + Vector2.right));
				}
				if (flag)
				{
					flag = !GameManager.instance.bedHalfPositions.Contains(GameManager.instance.selectionSquare);
				}
				break;
			case BlockType.Floor:
			{
				Farmland x = GameManager.instance.farmlands.Find((Farmland f) => f.transform.position == GameManager.instance.selectionSquare);
				flag = (!GameManager.instance.floorPositions.Contains(GameManager.instance.selectionSquare) && !GameManager.instance.treePositions.Contains(GameManager.instance.selectionSquare) && !GameManager.instance.waterPositions.Contains(GameManager.instance.selectionSquare) && x == null);
				break;
			}
			case BlockType.Tree:
				flag = (!GameManager.instance.treePositions.Contains(GameManager.instance.selectionSquare) && !GameManager.instance.blockPositions.Contains(GameManager.instance.selectionSquare) && !GameManager.instance.floorPositions.Contains(GameManager.instance.selectionSquare) && ((!GameManager.instance.isInMines && selectedItem.specialUsage != "RockSapling" && !GameManager.instance.sniperSkillsSeed) || (GameManager.instance.isInMines && selectedItem.specialUsage == "RockSapling")));
				if (flag)
				{
					flag = !GameManager.instance.bedHalfPositions.Contains(GameManager.instance.selectionSquare);
				}
				break;
			}
			if (flag && selectedItem.blockType == BlockType.Solid)
			{
				flag = !GameManager.instance.enemyBlockingBuild;
			}
			if (flag)
			{
				switch (selectedItem.blockType)
				{
				case BlockType.Solid:
					WorldManager.instance.SpawnBlock(selectedItem.blockId, GameManager.instance.selectionSquare);
					break;
				case BlockType.Floor:
					WorldManager.instance.SpawnFloor(selectedItem.blockId, GameManager.instance.selectionSquare);
					break;
				case BlockType.Tree:
					WorldManager.instance.SpawnTree(selectedItem.blockId, false, GameManager.instance.selectionSquare);
					break;
				}
				AudioManager.instance.Play("PlaceBlock");
			}
		}
		if (flag)
		{
			this.visual.Animation();
			if (selectedItem.isWeapon)
			{
				if (selectedItem.ammoId != "null" && !WorldPolicy.creativeMode)
				{
					if (GameManager.instance.EquippedAccesory("ammo_pouch"))
					{
						if (Random.Range(1, 3) == 1)
						{
							if (selectedItem.ammoId == "wood")
							{
								if (GameManager.instance.ItemCount("wood_standard", false) > 0)
								{
									GameManager.instance.RemoveItem("wood_standard", 1, false);
									return;
								}
								if (GameManager.instance.ItemCount("wood_birch", false) > 0)
								{
									GameManager.instance.RemoveItem("wood_birch", 1, false);
									return;
								}
								if (GameManager.instance.ItemCount("wood_evergreen", false) > 0)
								{
									GameManager.instance.RemoveItem("wood_evergreen", 1, false);
									return;
								}
							}
							else
							{
								GameManager.instance.RemoveItem(selectedItem.ammoId, 1, false);
							}
						}
					}
					else if (selectedItem.ammoId == "wood")
					{
						string text = GameManager.instance.FirstItemIdInInventory(new List<string>
						{
							"wood_standard",
							"wood_birch",
							"wood_evergreen"
						});
						if (GameManager.instance.ItemCount(text, false) > 0 && text != "null")
						{
							GameManager.instance.RemoveItem(text, 1, false);
						}
					}
					else
					{
						GameManager.instance.RemoveItem(selectedItem.ammoId, 1, false);
					}
				}
				if (selectedItem.specialUsage == "Flamethrower")
				{
					this.visual.flamethrowerTime = 0.3f;
				}
				if (selectedItem.projectile != null)
				{
					foreach (float num in selectedItem.projectileAngles)
					{
						GameObject gameObject = Object.Instantiate<GameObject>(selectedItem.projectile);
						gameObject.transform.position = base.transform.position;
						Vector2 vector = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - base.transform.position).normalized;
						float num2 = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
						num2 += num;
						vector = new Vector2(Mathf.Cos(num2 * 0.0174532924f), Mathf.Sin(num2 * 0.0174532924f));
						gameObject.GetComponent<Projectile>().direction = vector;
						gameObject.transform.Translate(vector * selectedItem.projectileOffset);
					}
				}
			}
			AudioManager.instance.Play(selectedItem.useSound);
			if (selectedItem.heal > 0)
			{
				if (selectedItem.lifeFlower <= 0)
				{
					GameManager.instance.AddStatusEffect("full");
				}
				GameManager.instance.health += selectedItem.heal;
			}
			GameManager.instance.extraHealth += selectedItem.lifeFlower;
			if (selectedItem.statusEffectId != string.Empty)
			{
				if (selectedItem.statusEffectId == "return")
				{
					if (GameManager.instance.playerMovement != null)
					{
						GameManager.instance.isInMines = false;
						GameManager.instance.playerMovement.transform.position = GameManager.instance.respawnPos;
					}
				}
				else
				{
					GameManager.instance.AddStatusEffect(selectedItem.statusEffectId);
				}
			}
			if (selectedItem.achievement)
			{
				AchievementManager.instance.AddAchievement(selectedItem.achievementId);
			}
			if (selectedItem.specialUsage == "PainVoid")
			{
				WorldPolicy.hellholeMode = true;
				WorldPolicy.creativeMode = false;
				WorldPolicy.spawnMobs = true;
				WorldPolicy.keepInventory = false;
				WorldPolicy.advancedTooltips = false;
				WorldPolicy.dayNightCycle = true;
				WorldPolicy.infiniteBuildRange = false;
				WorldPolicy.allowSwitchingGamemodes = false;
			}
			if (selectedItem.specialUsage == "MinesTeleporter")
			{
				WorldManager.instance.MinesTeleportation();
			}
			if (selectedItem.specialUsage == "Crate")
			{
				foreach (LootDrop lootDrop in selectedItem.crateDrops)
				{
					if (UtilityMath.RandomChance(1f / (lootDrop.DROP_CHANCE - 1f)) && lootDrop.DROP_ID != "null" && lootDrop.DROP_STACK_MIN != 0 && lootDrop.DROP_STACK_MAX != 0)
					{
						GameManager.instance.InitializePickupItem(GameManager.instance.playerPos, lootDrop.DROP_ID, Random.Range(lootDrop.DROP_STACK_MIN, lootDrop.DROP_STACK_MAX), true);
					}
				}
			}
			if (selectedItem.oneUse)
			{
				if (selectedItem.specialUsage == "Drink" && !WorldPolicy.creativeMode)
				{
					GameManager.instance.InitializePickupItem(GameManager.instance.playerPos, "bottle", 1, true);
				}
				GameManager.instance.RemoveCurrentHeldItemOne();
			}
		}
	}

	// Token: 0x0600019A RID: 410 RVA: 0x0000E820 File Offset: 0x0000CA20
	public void IterateEnemies(List<EnemyBase> enemies)
	{
		foreach (EnemyBase enemyBase in enemies)
		{
			if (enemyBase != null && enemyBase.IsCollidingWithMeleeWeapon() && GameManager.instance.ReturnSelectedItem().meleeHit)
			{
				enemyBase.TakeDamage(GameManager.instance.ReturnSelectedItem().damage + GameManager.instance.extraDamage, true, false, (base.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition)).normalized, (GameManager.instance.ReturnSelectedItem().specialUsage == "GlowSting") ? DamageSource.PlayerMeleeWeaponGlowSting : DamageSource.PlayerMeleeWeapon);
				enemyBase.RemoveMeleeCollision();
			}
		}
	}

	// Token: 0x04000219 RID: 537
	public HeldItemVisual visual;
}
