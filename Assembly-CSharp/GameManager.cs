using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using GameJolt.API;
using GameJolt.API.Core;
using GameJolt.API.Objects;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

// Token: 0x02000047 RID: 71
public class GameManager : MonoBehaviour
{
	// Token: 0x17000002 RID: 2
	// (get) Token: 0x06000121 RID: 289 RVA: 0x00007F44 File Offset: 0x00006144
	public Material Lit
	{
		get
		{
			return this.lit;
		}
	}

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x06000122 RID: 290 RVA: 0x00007F4C File Offset: 0x0000614C
	public Material Unlit
	{
		get
		{
			return this.unlit;
		}
	}

	// Token: 0x06000123 RID: 291 RVA: 0x00007F54 File Offset: 0x00006154
	private void Awake()
	{
		if (GameManager.instance == null)
		{
			GameManager.instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
		Resolution[] array = Screen.resolutions;
		this.resolutions.Clear();
		foreach (Resolution item in array)
		{
			this.resolutions.Add(item);
		}
		this.playerMovement = Object.FindObjectOfType<PlayerMovement>();
		this.enemyBar = Resources.Load<GameObject>("EnemyBar");
		Application.targetFrameRate = 60;
		this.LoadSettings();
		this.ReloadCustomSkin();
	}

	// Token: 0x06000124 RID: 292 RVA: 0x00007FF0 File Offset: 0x000061F0
	public void SaveSettings()
	{
		string value = JsonUtility.ToJson(new TrashTakeSettings
		{
			music = this.music,
			sound = this.sound,
			ambience = this.ambience,
			fullscreen = this.fullscreen,
			guiScale = this.guiScale,
			isLinked = this.isLinked,
			accountName = this.accountName,
			gameToken = this.gameToken,
			zoom = this.zoom,
			postProcessing = this.postProcessing,
			versionLastOpenedIn = Application.version,
			blockAnimations = this.blockAnimations,
			enemyCorpses = this.enemyCorpses,
			treeTransparency = this.treeTransparency,
			autosave = this.autosave,
			interactionTooltips = this.interactionTooltips,
			enemyHealthBars = this.enemyHealthBars,
			bossHealthBars = this.bossHealthBars
		});
		PlayerPrefs.SetString("SETTINGS", value);
	}

	// Token: 0x06000125 RID: 293 RVA: 0x000080EC File Offset: 0x000062EC
	private void LoadSettings()
	{
		if (PlayerPrefs.HasKey("SETTINGS"))
		{
			TrashTakeSettings trashTakeSettings = JsonUtility.FromJson<TrashTakeSettings>(PlayerPrefs.GetString("SETTINGS"));
			this.music = trashTakeSettings.music;
			this.sound = trashTakeSettings.sound;
			this.ambience = trashTakeSettings.ambience;
			this.fullscreen = trashTakeSettings.fullscreen;
			this.guiScale = trashTakeSettings.guiScale;
			Screen.fullScreen = this.fullscreen;
			this.isLinked = trashTakeSettings.isLinked;
			this.accountName = trashTakeSettings.accountName;
			this.gameToken = trashTakeSettings.gameToken;
			this.zoom = trashTakeSettings.zoom;
			this.postProcessing = trashTakeSettings.postProcessing;
			this.versionLastOpenedIn = trashTakeSettings.versionLastOpenedIn;
			this.blockAnimations = trashTakeSettings.blockAnimations;
			this.enemyCorpses = trashTakeSettings.enemyCorpses;
			this.treeTransparency = trashTakeSettings.treeTransparency;
			this.autosave = trashTakeSettings.autosave;
			this.interactionTooltips = trashTakeSettings.interactionTooltips;
			this.enemyHealthBars = trashTakeSettings.enemyHealthBars;
			this.bossHealthBars = trashTakeSettings.bossHealthBars;
			if (this.isLinked && MonoSingleton<GameJoltAPI>.Instance.CurrentUser == null && Application.internetReachability != NetworkReachability.NotReachable)
			{
				this.mainMenuSubmenu = 25;
				new User(this.accountName, this.gameToken).SignIn(delegate(bool c)
				{
					if (c)
					{
						this.mainMenuSubmenu = 0;
					}
					else
					{
						this.mainMenuSubmenu = 0;
					}
					if (this.versionLastOpenedIn != Application.version)
					{
						this.DownloadNews();
					}
				}, null, false);
				return;
			}
		}
		else
		{
			Screen.fullScreen = true;
		}
	}

	// Token: 0x06000126 RID: 294 RVA: 0x00008249 File Offset: 0x00006449
	private IEnumerator RegenHealth()
	{
		for (;;)
		{
			yield return new WaitForSeconds(5f);
			if (this.gameState == GameState.Playing)
			{
				this.health += (this.EquippedAccesory("red_necklace") ? 5 : 1);
			}
		}
		yield break;
	}

	// Token: 0x06000127 RID: 295 RVA: 0x00008258 File Offset: 0x00006458
	private IEnumerator StatusEffectCoroutine()
	{
		for (;;)
		{
			yield return new WaitForSeconds(1f);
			this.StatusEffectControl(1f);
		}
		yield break;
	}

	// Token: 0x06000128 RID: 296 RVA: 0x00008268 File Offset: 0x00006468
	private void Start()
	{
		this.LoadAvailableLevels();
		base.InvokeRepeating("SaveSettings", 5f, 5f);
		base.StartCoroutine("RegenHealth");
		base.StartCoroutine("StatusEffectCoroutine");
		this.LoadExportedWorldDirectiories();
		this.ForceOpenCreativeModeTab(CreativeMenuTab.All);
	}

	// Token: 0x06000129 RID: 297 RVA: 0x000082B5 File Offset: 0x000064B5
	public void DownloadNews()
	{
		base.StartCoroutine("DownloadNewsAsync");
	}

	// Token: 0x0600012A RID: 298 RVA: 0x000082C3 File Offset: 0x000064C3
	private IEnumerator DownloadNewsAsync()
	{
		this.mainMenuSubmenu = 22;
		this.newsIndex = 0;
		UnityWebRequest request = UnityWebRequest.Get("http://crimeraaa-survivalium.ct8.pl/news.php");
		yield return request.SendWebRequest();
		if (request.result == UnityWebRequest.Result.Success)
		{
			this.downloadedNews = JsonUtility.FromJson<BulletinCollection>(request.downloadHandler.text);
			this.mainMenuSubmenu = 21;
		}
		else
		{
			this.mainMenuSubmenu = 18;
		}
		yield break;
	}

	// Token: 0x0600012B RID: 299 RVA: 0x000082D4 File Offset: 0x000064D4
	public void LoadExportedWorldDirectiories()
	{
		this.exportedWorldsList.Clear();
		if (Directory.Exists(Application.persistentDataPath + "/ExportedWorlds/"))
		{
			foreach (string item in Directory.GetFiles(Application.persistentDataPath + "/ExportedWorlds/"))
			{
				this.exportedWorldsList.Add(item);
			}
		}
	}

	// Token: 0x0600012C RID: 300 RVA: 0x00008338 File Offset: 0x00006538
	public void ImportExportedWorlds()
	{
		foreach (string path in this.exportedWorldsList)
		{
			string text = File.ReadAllText(path);
			World world = JsonUtility.FromJson<World>(text);
			if (world != null && !this.availableLevels.Contains(world.name) && world.name != "AvailableLevels" && world.name != "SETTINGS")
			{
				if (world.storageName == string.Empty)
				{
					PlayerPrefs.SetString(world.name, text);
					this.availableLevels.Add(world.name);
				}
				else
				{
					PlayerPrefs.SetString(world.storageName, text);
					this.availableLevels.Add(world.storageName);
				}
				this.SaveAvailableLevels();
			}
			this.mainMenuSubmenu = 20;
		}
	}

	// Token: 0x0600012D RID: 301 RVA: 0x00008430 File Offset: 0x00006630
	public bool CanCraftItem(int selRecipe)
	{
		bool result = true;
		if (selRecipe != -1)
		{
			foreach (RecipeComponent recipeComponent in this.currentRecipes[selRecipe].ingredients)
			{
				if (recipeComponent.COMP_STACK > this.ItemCount(recipeComponent.COMP_ID, true))
				{
					result = false;
				}
			}
			return result;
		}
		return false;
	}

	// Token: 0x0600012E RID: 302 RVA: 0x00008484 File Offset: 0x00006684
	public void CraftItem(int selRecipe)
	{
		if (selRecipe != -1)
		{
			RecipeComponent[] ingredients = this.currentRecipes[selRecipe].ingredients;
			if (this.CanCraftItem(selRecipe))
			{
				foreach (RecipeComponent recipeComponent in ingredients)
				{
					this.RemoveItem(recipeComponent.COMP_ID, recipeComponent.COMP_STACK, true);
				}
				if (this.currentRecipes[selRecipe].isAchievement)
				{
					AchievementManager.instance.AddAchievement(this.currentRecipes[selRecipe].achievementId);
				}
				this.InitializePickupItem(this.playerPos, this.currentRecipes[selRecipe].result.COMP_ID, this.currentRecipes[selRecipe].result.COMP_STACK, true);
			}
		}
	}

	// Token: 0x0600012F RID: 303 RVA: 0x00008548 File Offset: 0x00006748
	public void OpenCloseInventory()
	{
		if (!(this.chair == null) || !(this.bed == null))
		{
			this.bed = null;
			this.chair = null;
			this.playerMovement.transform.position = this.playerBeforeBed;
			return;
		}
		if (this.selectedTeleporter == null)
		{
			if (this.pickingOutItems && (!this.creativePickingOut || (this.creativePickingOut && this.pickedSlot != 61)))
			{
				if (this.inventory[this.pickedSlot].ITEM_ID == "null" || (this.inventory[this.pickedSlot].ITEM_ID == this.inventory[61].ITEM_ID && this.inventory[this.pickedSlot].ITEM_STACK + this.inventory[61].ITEM_STACK <= this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[61].ITEM_ID).stackLimit))
				{
					this.inventory[this.pickedSlot] = new InventorySlot
					{
						ITEM_ID = this.inventory[61].ITEM_ID,
						ITEM_STACK = this.inventory[this.pickedSlot].ITEM_STACK + this.inventory[61].ITEM_STACK
					};
				}
				else
				{
					this.InitializePickupItem(GameManager.instance.playerPos, this.inventory[61].ITEM_ID, this.inventory[61].ITEM_STACK, false);
				}
				this.pickingOutItems = false;
				this.creativePickingOut = false;
			}
			this.inventory[61] = InventorySlot.EmptySlot();
			this.station = CraftingStation.ByHand;
			this.ReloadCraftingMenu(this.station);
			this.fullInventoryOpen = !this.fullInventoryOpen;
			this.tooltipIndex = -1;
			this.draggingSlot = false;
			this.draggedSlot = 0;
			this.npcShop = null;
			this.dragStart = 0;
			this.dragEnd = 0;
			this.tooltipCustom = "";
			this.SaveChestSlots();
			this.openedChest = null;
			this.sign = null;
			this.bed = null;
			this.chair = null;
			this.selectedRecipe = -1;
			this.hoveringButtonInv = false;
			this.selectingButtonInv = false;
			return;
		}
		this.selectedTeleporter = null;
		this.temporaryTeleporterList.Clear();
		this.teleporterSelectedId = 0;
	}

	// Token: 0x06000130 RID: 304 RVA: 0x000087E1 File Offset: 0x000069E1
	public void ExitToMainMenu(bool save)
	{
		base.StartCoroutine(this.ExitToMenuAsync(save));
	}

	// Token: 0x06000131 RID: 305 RVA: 0x000087F1 File Offset: 0x000069F1
	private IEnumerator ExitToMenuAsync(bool save)
	{
		yield return null;
		this.hoveringButtonInv = false;
		this.selectingButtonInv = false;
		this.SaveChestSlots();
		if (save)
		{
			yield return WorldManager.instance.SaveWorld();
		}
		this.selectedTeleporter = null;
		this.temporaryTeleporterList.Clear();
		this.teleporterSelectedId = 0;
		this.npcShop = null;
		this.fullInventoryOpen = false;
		this.tooltipIndex = -1;
		this.draggingSlot = false;
		this.draggedSlot = 0;
		this.dragStart = 0;
		this.dragEnd = 0;
		this.openedChest = null;
		this.sign = null;
		this.bed = null;
		this.chair = null;
		SceneManager.LoadSceneAsync("GameLoad");
		yield break;
	}

	// Token: 0x06000132 RID: 306 RVA: 0x00008808 File Offset: 0x00006A08
	private void SetGameState()
	{
		this.gameState = GameState.Playing;
		if (SceneManager.GetActiveScene().name == "GameLoad")
		{
			this.gameState = GameState.MainMenu;
		}
	}

	// Token: 0x06000133 RID: 307 RVA: 0x0000883C File Offset: 0x00006A3C
	private void Update()
	{
		this.SetGameState();
		if (this.gameState == GameState.Playing)
		{
			this.SwitchSlot();
			if (Input.GetKeyDown(KeyCode.Escape) && this.playerMovement != null)
			{
				this.OpenCloseInventory();
			}
			if (this.playerMovement == null && this.fullInventoryOpen)
			{
				this.fullInventoryOpen = false;
				this.CloseAllMenus();
			}
		}
		if (this.playerMovement != null && this.bed == null && this.chair == null && this.selectedTeleporter == null && Input.GetKeyDown(KeyCode.Q) && !this.generatingWorld && this.ReturnSelectedSlot().ITEM_ID != "null" && this.ReturnSelectedSlot().ITEM_STACK > 0)
		{
			if (Input.GetKey(KeyCode.LeftShift))
			{
				this.InitializePickupItem(this.playerPos, this.ReturnSelectedSlot().ITEM_ID, this.ReturnSelectedSlot().ITEM_STACK, false);
				this.inventory[this.ReturnSelectedSlotId()] = InventorySlot.EmptySlot();
			}
			else
			{
				this.InitializePickupItem(this.playerPos, this.ReturnSelectedSlot().ITEM_ID, 1, false);
				this.inventory[this.ReturnSelectedSlotId()] = new InventorySlot
				{
					ITEM_ID = this.ReturnSelectedSlot().ITEM_ID,
					ITEM_STACK = this.ReturnSelectedSlot().ITEM_STACK - 1
				};
				if (this.inventory[this.ReturnSelectedSlotId()].ITEM_STACK <= 0)
				{
					this.inventory[this.ReturnSelectedSlotId()] = InventorySlot.EmptySlot();
					if (this.draggingSlot)
					{
						this.draggingSlot = false;
						this.pickingOutItems = false;
						this.creativePickingOut = false;
						this.draggedSlot = 0;
					}
				}
			}
		}
		if (this.playerMovement != null && this.selectedTeleporter == null && Input.GetKeyDown(KeyCode.F) && !this.generatingWorld && !this.fullInventoryOpen && this.health < this.ReturnMaxHealth() && this.itemCooldown <= 0f && !this.HasStatusEffect("full"))
		{
			this.FoodShortcut();
		}
		GameState gameState = this.tempState;
		this.invasionEnemiesLeft = Mathf.Max(this.invasionEnemiesLeft, 0);
		if (this.gameState == GameState.Playing)
		{
			if (this.invasionEnemiesLeft <= 0)
			{
				if (this.invasionEvent == Invasion.CubicChaos)
				{
					AchievementManager.instance.AddAchievement(179819);
				}
				this.invasionEvent = Invasion.None;
			}
		}
		else
		{
			this.invasionEnemiesLeft = 0;
			this.invasionEvent = Invasion.None;
		}
		if (this.gameState != GameState.Playing)
		{
			this.emoteIndex = 0;
			this.isInMines = false;
		}
		if (this.gameState == GameState.MainMenu && this.tempState != GameState.MainMenu)
		{
			WorldManager.instance.loadedWorld = null;
		}
		if (this.selectedTeleporter != null && this.playerMovement != null && this.gameState == GameState.Playing)
		{
			this.playerMovement.transform.position = this.selectedTeleporter.transform.position;
			this.isInMines = (this.selectedTeleporter.transform.position.x > 500f);
		}
		if (this.lake != null)
		{
			this.fishingCooldown = 0.5f;
			if (Mathf.Abs(Mathf.Sin(this.fishingHitTime)) < 0.1f)
			{
				this.fishingHitTime += Time.deltaTime * 0.25f;
			}
			else
			{
				this.fishingHitTime += Time.deltaTime * this.ReturnSelectedItem().fishingSpeed * (float)(this.EquippedAccesory("bait_bag") ? 2 : 1);
			}
			if (this.ReturnSelectedItem().specialUsage != "Fishing")
			{
				this.lake = null;
			}
			if (Input.GetMouseButtonDown(0))
			{
				if (Mathf.Abs(Mathf.Sin(this.fishingHitTime)) < 0.1f)
				{
					LootDrop lootDrop = this.ReturnFishingItemId();
					if (lootDrop != null)
					{
						this.InitializePickupItem(this.playerPos, lootDrop.DROP_ID, Random.Range(lootDrop.DROP_STACK_MIN, lootDrop.DROP_STACK_MAX), true);
					}
					else
					{
						AchievementManager.instance.AddAchievement(158319);
						int num = Random.Range(1, 3);
						if (num != 1)
						{
							if (num == 2)
							{
								this.InitializePickupItem(this.playerPos, "raw_salmon", 1, true);
							}
						}
						else
						{
							this.InitializePickupItem(this.playerPos, "raw_fish", 1, true);
						}
					}
				}
				this.fishingHitTime = -2f;
				this.lake = null;
			}
		}
		else
		{
			this.fishingCooldown = Mathf.Max(this.fishingCooldown - Time.deltaTime, 0f);
			this.fishingHitTime = -2f;
		}
		if (this.minute != this.ReturnHour() && this.ReturnHour() == 5f && this.gameState == GameState.Playing && this.tempState == this.gameState)
		{
			if (Random.Range(1, 6) == 1 && !this.rain)
			{
				this.rain = true;
			}
			else
			{
				this.rain = false;
			}
			if (this.ReturnDay() > 10)
			{
				AchievementManager.instance.AddAchievement(159733);
			}
			AchievementManager.instance.AddAchievement(156195);
			GuiManager guiManager = Object.FindObjectOfType<GuiManager>();
			guiManager.StopCoroutine("PlayDayDisplayAnimation");
			guiManager.StartCoroutine("PlayDayDisplayAnimation");
		}
		this.minute = this.ReturnHour();
		if (this.gameState == GameState.MainMenu)
		{
			this.fullTime += Time.deltaTime * (float)((this.mainMenuSubmenu == 9) ? 0 : 300);
		}
		else
		{
			this.fullTime += Time.deltaTime * (float)((this.bed != null && (this.IsNight() || this.rain)) ? 160 : ((WorldPolicy.dayNightCycle && !this.generatingWorld) ? (this.isInMines ? 8 : 4) : 0));
		}
		this.time = this.fullTime % 1440f;
		this.money += this.ItemCount("money", false);
		this.RemoveItem("money", this.ItemCount("money", false), false);
		this.itemCooldown -= Time.deltaTime;
		if (this.itemCooldown < 0f)
		{
			this.itemCooldown = 0f;
		}
		this.UpdateDefense();
		this.UpdateExtraMovementSpeed();
		this.UpdateExtraDamage();
		this.UpdateExtraCritChance();
		this.UpdateExtraPickupRange();
		this.UpdateExtraDashLength();
		this.UpdateExtraBuildRange();
		this.DetectAchievements();
		this.health = Mathf.Clamp(this.health, 0, this.ReturnMaxHealth());
		if (this.HasStatusEffect("poison"))
		{
			this.DealDamage(1, DamageSource.Poison);
		}
		if (WorldPolicy.creativeMode)
		{
			this.invinciblityFrame = 1f;
			this.health = this.ReturnMaxHealth();
		}
		else
		{
			this.invinciblityFrame = Mathf.Clamp(this.invinciblityFrame - Time.deltaTime, 0f, 2f);
		}
		if (this.openedChest == null)
		{
			this.WipeChestSlots();
		}
		if (this.playerMovement != null)
		{
			this.playerPos = this.playerMovement.transform.position;
			if (!this.hoveringButtonInv)
			{
				this.selectionSquare = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.up * 0.5f;
			}
			if (WorldPolicy.infiniteBuildRange)
			{
				this.selectionSquare.x = Mathf.Round(this.selectionSquare.x) + 0.5f;
				this.selectionSquare.y = Mathf.Max(Mathf.Round(this.selectionSquare.y), (float)(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-148) - 4));
			}
			else
			{
				this.selectionSquare.x = Mathf.Round(Mathf.Clamp(this.selectionSquare.x, this.playerMovement.transform.position.x - (4f + this.extraBuildRange), this.playerMovement.transform.position.x + (3f + this.extraBuildRange))) + 0.5f;
				this.selectionSquare.y = Mathf.Round(Mathf.Clamp(this.selectionSquare.y, this.playerMovement.transform.position.y - (3f + this.extraBuildRange), this.playerMovement.transform.position.y + (4f + this.extraBuildRange)));
			}
		}
		else if (this.gameState == GameState.Playing && !this.tempDeath)
		{
			this.tooltipCustom = string.Empty;
			if (this.EquippedAccesory("ukrainium_guard"))
			{
				this.invinciblityFrame = 4f;
				AudioManager.instance.Play("KillEnemy");
				this.health = this.ReturnMaxHealth();
				WorldGeneration worldGeneration = Object.FindObjectOfType<WorldGeneration>();
				if (worldGeneration != null)
				{
					worldGeneration.SpawnPlayer(this.playerPos);
				}
				this.DropUkrainiumGuards();
				UkrainiumGuardAnimation.PlayAnimation();
				AchievementManager.instance.AddAchievement(179818);
			}
			else
			{
				AudioManager.instance.Play("KillEnemy");
				WorldGeneration worldGeneration2 = Object.FindObjectOfType<WorldGeneration>();
				if (worldGeneration2 != null)
				{
					worldGeneration2.SpawnTombstone();
				}
				this.statusEffects.Clear();
				this.DropItems();
				if (WorldPolicy.hellholeMode)
				{
					this.money = Mathf.Max((int)((float)this.money * 0.8f), 0);
				}
				AchievementManager.instance.AddAchievement(179822);
			}
		}
		this.inventory[36] = new InventorySlot
		{
			ITEM_ID = "null",
			ITEM_STACK = 0
		};
		if (this.inventory[60].ITEM_ID != "null")
		{
			this.money += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[60].ITEM_ID).price * this.inventory[60].ITEM_STACK;
			this.inventory[60] = new InventorySlot
			{
				ITEM_ID = "null",
				ITEM_STACK = 0
			};
		}
		if (this.tooltipIndex != -1 && Input.GetMouseButtonDown(1) && this.fullInventoryOpen && (this.inventory[61].ITEM_ID == "null" || this.inventory[61].ITEM_ID == this.inventory[this.tooltipIndex].ITEM_ID) && this.inventory[this.tooltipIndex].ITEM_ID != "null" && !this.IsAnySpecialSlot(this.tooltipIndex))
		{
			this.inventory[this.tooltipIndex] = new InventorySlot
			{
				ITEM_ID = this.inventory[this.tooltipIndex].ITEM_ID,
				ITEM_STACK = this.inventory[this.tooltipIndex].ITEM_STACK - 1
			};
			this.inventory[61] = new InventorySlot
			{
				ITEM_ID = this.inventory[this.tooltipIndex].ITEM_ID,
				ITEM_STACK = this.inventory[61].ITEM_STACK + 1
			};
			this.draggingSlot = true;
			this.pickingOutItems = true;
			this.pickedSlot = this.tooltipIndex;
			this.draggedSlot = 61;
			this.dragStart = 61;
			if (this.inventory[this.tooltipIndex].ITEM_STACK < 1)
			{
				this.inventory[this.tooltipIndex] = new InventorySlot
				{
					ITEM_ID = "null",
					ITEM_STACK = 0
				};
			}
		}
		if (this.gameState == GameState.Playing && !this.generatingWorld)
		{
			if (this.sign == null && this.openedChest == null && (!this.hoveringButtonInv || !this.selectingButtonInv) && this.playerMovement != null)
			{
				if (Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.Underscore))
				{
					this.zoom += 5f * Time.deltaTime;
				}
				if (Input.GetKey(KeyCode.Plus) || Input.GetKey(KeyCode.Equals))
				{
					this.zoom -= 5f * Time.deltaTime;
				}
			}
			this.zoom = Mathf.Clamp(this.zoom, 6f, 12f);
			Camera.main.orthographicSize = this.zoom;
		}
		else
		{
			this.mainMenuCameraZoom = Mathf.SmoothDamp(this.mainMenuCameraZoom, 7f, ref this.mainMenuZoomOutVel, 2f);
			Camera.main.orthographicSize = this.mainMenuCameraZoom;
		}
		this.sunlight = Mathf.SmoothDamp(this.sunlight, this.ReturnLightLevelFromMinute(), ref this.currentSunlightVel, 0.2f);
		this.lightLevel = this.sunlight;
		this.tempState = this.gameState;
		this.tempDeath = (this.playerMovement == null);
	}

	// Token: 0x06000134 RID: 308 RVA: 0x00009578 File Offset: 0x00007778
	public int ReturnDay()
	{
		return Mathf.FloorToInt(this.fullTime / 1440f) + 1;
	}

	// Token: 0x06000135 RID: 309 RVA: 0x00009590 File Offset: 0x00007790
	private void DetectAchievements()
	{
		if (this.gameState == GameState.Playing && !this.generatingWorld)
		{
			if (this.ItemCount("pistol", false) > 0 || this.ItemCount("machine_gun", false) > 0 || this.ItemCount("rocket_launcher", false) > 0 || this.ItemCount("shotgun", false) > 0 || this.ItemCount("sniper", false) > 0 || this.ItemCount("super_shooter", false) > 0)
			{
				AchievementManager.instance.AddAchievement(156198);
			}
			if (this.ItemCount("firesword", false) > 0)
			{
				AchievementManager.instance.AddAchievement(156202);
			}
			if (this.ItemCount("money_card", false) > 0)
			{
				AchievementManager.instance.AddAchievement(179823);
			}
			if (this.ItemCount("toast", false) > 0)
			{
				AchievementManager.instance.AddAchievement(179824);
			}
			if (this.inventory[37].ITEM_ID != "null" || this.inventory[38].ITEM_ID != "null" || this.inventory[39].ITEM_ID != "null")
			{
				AchievementManager.instance.AddAchievement(156197);
			}
			if (this.inventory[37].ITEM_ID == "monke_mask")
			{
				AchievementManager.instance.AddAchievement(175243);
			}
		}
	}

	// Token: 0x06000136 RID: 310 RVA: 0x00009710 File Offset: 0x00007910
	private void DropItems()
	{
		for (int i = 0; i < 44; i++)
		{
			if (!WorldPolicy.keepInventory && this.inventory[i].ITEM_ID != "axe_stone" && this.inventory[i].ITEM_ID != "pickaxe_stone" && this.inventory[i].ITEM_ID != "sword_stone" && this.inventory[i].ITEM_ID != "hammer_stone" && this.inventory[i].ITEM_ID != "pain_void")
			{
				if (this.inventory[i].ITEM_ID != "ukrainium_guard")
				{
					this.InitializePickupItem(this.playerPos + new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f)), this.inventory[i].ITEM_ID, this.inventory[i].ITEM_STACK, true);
				}
				this.inventory[i] = new InventorySlot
				{
					ITEM_ID = "null",
					ITEM_STACK = 0
				};
			}
		}
	}

	// Token: 0x06000137 RID: 311 RVA: 0x00009878 File Offset: 0x00007A78
	private void DropUkrainiumGuards()
	{
		for (int i = 37; i < 44; i++)
		{
			if (this.inventory[i].ITEM_ID == "ukrainium_guard")
			{
				this.inventory[i] = new InventorySlot
				{
					ITEM_ID = "null",
					ITEM_STACK = 0
				};
			}
		}
	}

	// Token: 0x06000138 RID: 312 RVA: 0x000098D9 File Offset: 0x00007AD9
	public int ReturnMaxHealth()
	{
		return 20 + this.extraHealth;
	}

	// Token: 0x06000139 RID: 313 RVA: 0x000098E4 File Offset: 0x00007AE4
	public void DealDamage(int damage, DamageSource damageSource)
	{
		if (damage > 0 && this.selectedTeleporter == null && this.invinciblityFrame <= 0f && this.playerMovement != null)
		{
			AudioManager.instance.Play("HitEnemy");
			this.lastPlayerDamageSource = damageSource;
			this.health -= Mathf.Clamp(damage - this.defense / (WorldPolicy.hellholeMode ? 5 : 3), 1, 10000);
			this.invinciblityFrame = 0.3f;
		}
	}

	// Token: 0x0600013A RID: 314 RVA: 0x0000996B File Offset: 0x00007B6B
	public void ResetDamageSource()
	{
		this.lastPlayerDamageSource = DamageSource.PlaceholderForImmunity;
	}

	// Token: 0x0600013B RID: 315 RVA: 0x00009978 File Offset: 0x00007B78
	public List<string> ReturnAvailableLevelNames()
	{
		List<string> list = new List<string>();
		foreach (string key in this.availableLevels)
		{
			World world = JsonUtility.FromJson<World>(PlayerPrefs.GetString(key));
			if (world != null)
			{
				list.Add(world.name);
			}
		}
		return list;
	}

	// Token: 0x0600013C RID: 316 RVA: 0x000099E4 File Offset: 0x00007BE4
	private void LoadAvailableLevels()
	{
		if (PlayerPrefs.HasKey("AvailableLevels"))
		{
			WorldListJson worldListJson = JsonUtility.FromJson<WorldListJson>(PlayerPrefs.GetString("AvailableLevels"));
			foreach (string text in worldListJson.availableLevels)
			{
				if (PlayerPrefs.HasKey(text))
				{
					this.availableLevels.Add(text);
				}
			}
			foreach (string text2 in worldListJson.uncopiableLevels)
			{
				if (PlayerPrefs.HasKey(text2))
				{
					this.uncopiableLevels.Add(text2);
				}
			}
		}
	}

	// Token: 0x0600013D RID: 317 RVA: 0x00009AB4 File Offset: 0x00007CB4
	public void SaveAvailableLevels()
	{
		string[] array = this.availableLevels.ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			if (PlayerPrefs.HasKey(this.availableLevels[i]))
			{
				array[i] = JsonUtility.FromJson<World>(PlayerPrefs.GetString(this.availableLevels[i])).name;
			}
		}
		string[] array2 = this.availableLevels.ToArray();
		Array.Sort<string, string>(array, array2);
		this.availableLevels.Clear();
		foreach (string item in array2)
		{
			this.availableLevels.Add(item);
		}
		PlayerPrefs.SetString("AvailableLevels", JsonUtility.ToJson(new WorldListJson
		{
			availableLevels = this.availableLevels,
			uncopiableLevels = this.uncopiableLevels
		}));
	}

	// Token: 0x0600013E RID: 318 RVA: 0x00009B80 File Offset: 0x00007D80
	public void ReloadCraftingMenu(CraftingStation station)
	{
		this.recipeTab = 0;
		this.currentRecipes.Clear();
		foreach (Recipe recipe in this.recipeRegistryReference.recipes)
		{
			if ((recipe.craftingStation == station || recipe.craftingStation == CraftingStation.ByHand || (station == CraftingStation.ChromiumAnvil && recipe.craftingStation == CraftingStation.Anvil) || station == CraftingStation.DebugCrafter) && (!recipe.experimental || WorldPolicy.experimentalFeatures) && (recipe.result.COMP_ID != "pain_void" || !WorldPolicy.hellholeMode))
			{
				this.currentRecipes.Add(recipe);
			}
		}
	}

	// Token: 0x0600013F RID: 319 RVA: 0x00009C18 File Offset: 0x00007E18
	public void LoadChestSlots()
	{
		if (this.openedChest != null)
		{
			this.inventory[44] = this.openedChest.chestInventory[0];
			this.inventory[45] = this.openedChest.chestInventory[1];
			this.inventory[46] = this.openedChest.chestInventory[2];
			this.inventory[47] = this.openedChest.chestInventory[3];
			this.inventory[48] = this.openedChest.chestInventory[4];
			this.inventory[49] = this.openedChest.chestInventory[5];
			this.inventory[50] = this.openedChest.chestInventory[6];
			this.inventory[51] = this.openedChest.chestInventory[7];
			this.inventory[52] = this.openedChest.chestInventory[8];
			this.inventory[53] = this.openedChest.chestInventory[9];
			this.inventory[54] = this.openedChest.chestInventory[10];
			this.inventory[55] = this.openedChest.chestInventory[11];
			this.inventory[56] = this.openedChest.chestInventory[12];
			this.inventory[57] = this.openedChest.chestInventory[13];
			this.inventory[58] = this.openedChest.chestInventory[14];
			this.inventory[59] = this.openedChest.chestInventory[15];
		}
	}

	// Token: 0x06000140 RID: 320 RVA: 0x00009E20 File Offset: 0x00008020
	public void WipeChestSlots()
	{
		this.inventory[44] = InventorySlot.EmptySlot();
		this.inventory[45] = InventorySlot.EmptySlot();
		this.inventory[46] = InventorySlot.EmptySlot();
		this.inventory[47] = InventorySlot.EmptySlot();
		this.inventory[48] = InventorySlot.EmptySlot();
		this.inventory[49] = InventorySlot.EmptySlot();
		this.inventory[50] = InventorySlot.EmptySlot();
		this.inventory[51] = InventorySlot.EmptySlot();
		this.inventory[52] = InventorySlot.EmptySlot();
		this.inventory[53] = InventorySlot.EmptySlot();
		this.inventory[54] = InventorySlot.EmptySlot();
		this.inventory[55] = InventorySlot.EmptySlot();
		this.inventory[56] = InventorySlot.EmptySlot();
		this.inventory[57] = InventorySlot.EmptySlot();
		this.inventory[58] = InventorySlot.EmptySlot();
		this.inventory[59] = InventorySlot.EmptySlot();
	}

	// Token: 0x06000141 RID: 321 RVA: 0x00009F50 File Offset: 0x00008150
	public void SaveChestSlots()
	{
		if (this.openedChest != null)
		{
			Object.FindObjectOfType<InventoryUiEvents>().SaveChestName();
			this.openedChest.chestInventory[0] = this.inventory[44];
			this.openedChest.chestInventory[1] = this.inventory[45];
			this.openedChest.chestInventory[2] = this.inventory[46];
			this.openedChest.chestInventory[3] = this.inventory[47];
			this.openedChest.chestInventory[4] = this.inventory[48];
			this.openedChest.chestInventory[5] = this.inventory[49];
			this.openedChest.chestInventory[6] = this.inventory[50];
			this.openedChest.chestInventory[7] = this.inventory[51];
			this.openedChest.chestInventory[8] = this.inventory[52];
			this.openedChest.chestInventory[9] = this.inventory[53];
			this.openedChest.chestInventory[10] = this.inventory[54];
			this.openedChest.chestInventory[11] = this.inventory[55];
			this.openedChest.chestInventory[12] = this.inventory[56];
			this.openedChest.chestInventory[13] = this.inventory[57];
			this.openedChest.chestInventory[14] = this.inventory[58];
			this.openedChest.chestInventory[15] = this.inventory[59];
			this.inventory[44] = InventorySlot.EmptySlot();
			this.inventory[45] = InventorySlot.EmptySlot();
			this.inventory[46] = InventorySlot.EmptySlot();
			this.inventory[47] = InventorySlot.EmptySlot();
			this.inventory[48] = InventorySlot.EmptySlot();
			this.inventory[49] = InventorySlot.EmptySlot();
			this.inventory[50] = InventorySlot.EmptySlot();
			this.inventory[51] = InventorySlot.EmptySlot();
			this.inventory[52] = InventorySlot.EmptySlot();
			this.inventory[53] = InventorySlot.EmptySlot();
			this.inventory[54] = InventorySlot.EmptySlot();
			this.inventory[55] = InventorySlot.EmptySlot();
			this.inventory[56] = InventorySlot.EmptySlot();
			this.inventory[57] = InventorySlot.EmptySlot();
			this.inventory[58] = InventorySlot.EmptySlot();
			this.inventory[59] = InventorySlot.EmptySlot();
		}
	}

	// Token: 0x06000142 RID: 322 RVA: 0x0000A27F File Offset: 0x0000847F
	public bool IsNight()
	{
		return this.ReturnHour() < 5f || 21f <= this.ReturnHour();
	}

	// Token: 0x06000143 RID: 323 RVA: 0x0000A29E File Offset: 0x0000849E
	public float ReturnHour()
	{
		return Mathf.Floor(Mathf.Floor(this.time) / 60f);
	}

	// Token: 0x06000144 RID: 324 RVA: 0x0000A2B8 File Offset: 0x000084B8
	public string ReturnFormattedGameTimeAmPm(bool includeSec)
	{
		if (Mathf.Floor(Mathf.Floor(this.time) / 60f) > 12f)
		{
			string text = (Mathf.Floor(this.time) % 60f).ToString();
			if (text.Length < 2)
			{
				text = "0" + text;
			}
			if (includeSec)
			{
				return string.Format("{0}:{1} PM", Mathf.Floor(Mathf.Floor(this.time) / 60f) - 12f, text);
			}
			return string.Format("{0} PM", Mathf.Floor(Mathf.Floor(this.time) / 60f) - 12f);
		}
		else
		{
			string text2 = (Mathf.Floor(this.time) % 60f).ToString();
			if (text2.Length < 2)
			{
				text2 = "0" + text2;
			}
			if (includeSec)
			{
				return string.Format("{0}:{1} AM", Mathf.Floor(Mathf.Floor(this.time) / 60f), text2);
			}
			return string.Format("{0} AM", Mathf.Floor(Mathf.Floor(this.time) / 60f));
		}
	}

	// Token: 0x06000145 RID: 325 RVA: 0x0000A3F0 File Offset: 0x000085F0
	public string ReturnFormattedGameTime()
	{
		string text = (Mathf.Floor(this.time) % 60f).ToString();
		if (text.Length < 2)
		{
			text = "0" + text;
		}
		return string.Format("{0}:{1}", Mathf.Floor(Mathf.Floor(this.time) / 60f), text);
	}

	// Token: 0x06000146 RID: 326 RVA: 0x0000A454 File Offset: 0x00008654
	public void CloseAllMenus()
	{
		if (this.pickingOutItems && (!this.creativePickingOut || (this.creativePickingOut && this.pickedSlot != 61)))
		{
			if (this.inventory[this.pickedSlot].ITEM_ID == "null" || (this.inventory[this.pickedSlot].ITEM_ID == this.inventory[61].ITEM_ID && this.inventory[this.pickedSlot].ITEM_STACK + this.inventory[61].ITEM_STACK <= this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[61].ITEM_ID).stackLimit))
			{
				this.inventory[this.pickedSlot] = new InventorySlot
				{
					ITEM_ID = this.inventory[61].ITEM_ID,
					ITEM_STACK = this.inventory[this.pickedSlot].ITEM_STACK + this.inventory[61].ITEM_STACK
				};
			}
			else
			{
				this.InitializePickupItem(GameManager.instance.playerPos, this.inventory[61].ITEM_ID, this.inventory[61].ITEM_STACK, false);
			}
			this.pickingOutItems = false;
			this.creativePickingOut = false;
		}
		this.inventory[61] = InventorySlot.EmptySlot();
		this.ReloadCraftingMenu(CraftingStation.ByHand);
		if (this.openedChest != null)
		{
			this.SaveChestSlots();
		}
		this.tooltipIndex = -1;
		this.draggingSlot = false;
		this.npcShop = null;
		this.draggedSlot = 0;
		this.dragStart = 0;
		this.dragEnd = 0;
		this.tooltipCustom = "";
		this.sign = null;
		this.bed = null;
		this.chair = null;
		this.openedStationBlock = null;
		this.selectedRecipe = -1;
		this.hoveringButtonInv = false;
		this.selectingButtonInv = false;
		this.station = CraftingStation.ByHand;
	}

	// Token: 0x06000147 RID: 327 RVA: 0x0000A670 File Offset: 0x00008870
	public InventorySlot ReturnSelectedSlot()
	{
		return this.inventory[this.ReturnSelectedSlotId()];
	}

	// Token: 0x06000148 RID: 328 RVA: 0x0000A683 File Offset: 0x00008883
	public bool CheckIfSlotDragged(int slotId)
	{
		return this.draggedSlot == slotId;
	}

	// Token: 0x06000149 RID: 329 RVA: 0x0000A68E File Offset: 0x0000888E
	public Item ReturnSelectedItem()
	{
		return this.itemRegistryReference.FindItemByInternalIdentifier(this.ReturnSelectedSlot().ITEM_ID);
	}

	// Token: 0x0600014A RID: 330 RVA: 0x0000A6A6 File Offset: 0x000088A6
	public int ReturnSelectedSlotId()
	{
		if (this.draggingSlot)
		{
			return this.draggedSlot;
		}
		return this.selectedItem;
	}

	// Token: 0x0600014B RID: 331 RVA: 0x0000A6C0 File Offset: 0x000088C0
	public float ReturnLightLevelFromMinute()
	{
		float num = this.ReturnHour();
		if (num == 0f)
		{
			return 0.26f;
		}
		if (num == 1f)
		{
			return 0.3f;
		}
		if (num == 2f)
		{
			return 0.4f;
		}
		if (num == 3f)
		{
			return 0.45f;
		}
		if (num == 4f)
		{
			return 0.48f;
		}
		if (num == 5f)
		{
			return 0.5f;
		}
		if (num == 6f)
		{
			return 0.6f;
		}
		if (num == 7f)
		{
			return 0.66f;
		}
		if (num == 8f)
		{
			return 0.72f;
		}
		if (num == 9f)
		{
			return 0.8f;
		}
		if (num == 10f)
		{
			return 0.85f;
		}
		if (num == 11f)
		{
			return 1f;
		}
		if (num == 12f)
		{
			return 1f;
		}
		if (num == 13f)
		{
			return 1f;
		}
		if (num == 14f)
		{
			return 1f;
		}
		if (num == 15f)
		{
			return 1f;
		}
		if (num == 16f)
		{
			return 1f;
		}
		if (num == 17f)
		{
			return 1f;
		}
		if (num == 18f)
		{
			return 0.85f;
		}
		if (num == 19f)
		{
			return 0.75f;
		}
		if (num == 20f)
		{
			return 0.5f;
		}
		if (num == 21f)
		{
			return 0.4f;
		}
		if (num == 22f)
		{
			return 0.3f;
		}
		if (num == 23f)
		{
			return 0.25f;
		}
		if (num != 24f)
		{
			return 1f;
		}
		return 0.21f;
	}

	// Token: 0x0600014C RID: 332 RVA: 0x0000A888 File Offset: 0x00008A88
	private void SwitchSlot()
	{
		if (!this.generatingWorld && this.playerMovement != null)
		{
			if (this.sign == null && this.openedChest == null && (!this.hoveringButtonInv || !this.selectingButtonInv))
			{
				if (Input.GetKeyDown(KeyCode.Alpha1))
				{
					this.selectedItem = 0;
				}
				if (Input.GetKeyDown(KeyCode.Alpha2))
				{
					this.selectedItem = 1;
				}
				if (Input.GetKeyDown(KeyCode.Alpha3))
				{
					this.selectedItem = 2;
				}
				if (Input.GetKeyDown(KeyCode.Alpha4))
				{
					this.selectedItem = 3;
				}
				if (Input.GetKeyDown(KeyCode.Alpha5))
				{
					this.selectedItem = 4;
				}
				if (Input.GetKeyDown(KeyCode.Alpha6))
				{
					this.selectedItem = 5;
				}
				if (Input.GetKeyDown(KeyCode.Alpha7))
				{
					this.selectedItem = 6;
				}
				if (Input.GetKeyDown(KeyCode.Alpha8))
				{
					this.selectedItem = 7;
				}
				if (Input.GetKeyDown(KeyCode.Alpha9))
				{
					this.selectedItem = 8;
				}
			}
			if (this.npcShop == null)
			{
				if (Input.GetKey(KeyCode.Z) && this.sign == null && this.bed == null && this.chair == null && this.openedChest == null && !this.generatingWorld && this.playerMovement.vampire == null)
				{
					int num = (this.emoteIndex + (int)Input.mouseScrollDelta.y) % this.emoteRegistryReference.emotes.Length;
					if (num < 0)
					{
						this.emoteIndex = this.emoteRegistryReference.emotes.Length - 1;
					}
					else
					{
						this.emoteIndex = num;
					}
				}
				else
				{
					this.selectedItem += (int)Input.mouseScrollDelta.y;
				}
			}
		}
		if (this.selectedItem < 0)
		{
			this.selectedItem = 8;
		}
		if (this.selectedItem > 8)
		{
			this.selectedItem = 0;
		}
	}

	// Token: 0x0600014D RID: 333 RVA: 0x0000AA68 File Offset: 0x00008C68
	public int GetSlotForPickup(string itemId)
	{
		int num = -1;
		if (num == -1)
		{
			int num2 = this.inventory.IndexOf(this.inventory.Find((InventorySlot i) => i.ITEM_ID == itemId && i.ITEM_STACK < this.itemRegistryReference.FindItemByInternalIdentifier(i.ITEM_ID).stackLimit));
			if (num2 <= 35)
			{
				num = num2;
			}
		}
		if (num == -1)
		{
			int num3 = this.inventory.IndexOf(this.inventory.Find((InventorySlot i) => i.ITEM_ID == "null" && i.ITEM_STACK < this.itemRegistryReference.FindItemByInternalIdentifier(i.ITEM_ID).stackLimit));
			if (num3 <= 35)
			{
				num = num3;
			}
		}
		return num;
	}

	// Token: 0x0600014E RID: 334 RVA: 0x0000AAE8 File Offset: 0x00008CE8
	public int ItemCount(string i_id, bool crafting)
	{
		if (i_id == "money" && crafting)
		{
			return this.money;
		}
		int num = 0;
		for (int i = 0; i <= 35; i++)
		{
			if (this.inventory[i].ITEM_ID == i_id)
			{
				num += this.inventory[i].ITEM_STACK;
			}
		}
		return num;
	}

	// Token: 0x0600014F RID: 335 RVA: 0x0000AB48 File Offset: 0x00008D48
	public int RemoveItem(string removedId, int quantity, bool crafting)
	{
		if (removedId == "money" && crafting)
		{
			this.money -= quantity;
			return quantity;
		}
		int num = quantity;
		for (int i = 0; i < quantity; i++)
		{
			if (!this.RemoveItemBase(removedId, 1))
			{
				num--;
			}
		}
		return num;
	}

	// Token: 0x06000150 RID: 336 RVA: 0x0000AB94 File Offset: 0x00008D94
	private bool RemoveItemBase(string removeItemId, int quantity)
	{
		int quantity2 = quantity;
		bool result = false;
		Predicate<InventorySlot> <>9__0;
		for (int j = 0; j < quantity; j++)
		{
			List<InventorySlot> list = this.inventory;
			Predicate<InventorySlot> match;
			if ((match = <>9__0) == null)
			{
				match = (<>9__0 = ((InventorySlot i) => i.ITEM_ID == removeItemId && i.ITEM_STACK >= quantity));
			}
			int num = list.FindIndex(match);
			if (num != -1 && num <= 35 && this.inventory[num].ITEM_STACK > 0)
			{
				this.inventory[num] = new InventorySlot
				{
					ITEM_ID = this.inventory[num].ITEM_ID,
					ITEM_STACK = this.inventory[num].ITEM_STACK - 1
				};
				if (this.inventory[num].ITEM_STACK <= 0)
				{
					this.inventory[num] = InventorySlot.EmptySlot();
				}
			}
			else
			{
				result = true;
			}
		}
		return result;
	}

	// Token: 0x06000151 RID: 337 RVA: 0x0000AC9C File Offset: 0x00008E9C
	private int AddItemBase(string addItemId, int quantity)
	{
		int slotForPickup = this.GetSlotForPickup(addItemId);
		int value = 0;
		if (slotForPickup != -1 && slotForPickup <= 35)
		{
			if (this.inventory[slotForPickup].ITEM_ID == "null")
			{
				this.quantityAdded += Mathf.Clamp(quantity, 0, this.itemRegistryReference.FindItemByInternalIdentifier(addItemId).stackLimit);
				value = quantity - this.itemRegistryReference.FindItemByInternalIdentifier(addItemId).stackLimit;
				this.inventory[slotForPickup] = new InventorySlot
				{
					ITEM_ID = addItemId,
					ITEM_STACK = Mathf.Clamp(quantity, 0, this.itemRegistryReference.FindItemByInternalIdentifier(addItemId).stackLimit)
				};
			}
			else
			{
				int item_STACK = this.inventory[slotForPickup].ITEM_STACK;
				this.quantityAdded += Mathf.Clamp(item_STACK + quantity, 0, this.itemRegistryReference.FindItemByInternalIdentifier(addItemId).stackLimit) - item_STACK;
				value = item_STACK + quantity - this.itemRegistryReference.FindItemByInternalIdentifier(addItemId).stackLimit;
				this.inventory[slotForPickup] = new InventorySlot
				{
					ITEM_ID = addItemId,
					ITEM_STACK = Mathf.Clamp(item_STACK + quantity, 0, this.itemRegistryReference.FindItemByInternalIdentifier(addItemId).stackLimit)
				};
			}
		}
		return Mathf.Clamp(value, 0, int.MaxValue);
	}

	// Token: 0x06000152 RID: 338 RVA: 0x0000ADF4 File Offset: 0x00008FF4
	public int AddItem(string addedId, int addedQuantity, bool crafting)
	{
		if (addedId == "money" && crafting)
		{
			this.money += addedQuantity;
			return addedQuantity;
		}
		int num = addedQuantity;
		do
		{
			num = this.AddItemBase(addedId, num);
		}
		while (num > 0);
		int result = this.quantityAdded;
		this.quantityAdded = 0;
		return result;
	}

	// Token: 0x06000153 RID: 339 RVA: 0x0000AE3C File Offset: 0x0000903C
	public string FirstItemIdInInventory(List<string> itemIds)
	{
		for (int i = 0; i <= 35; i++)
		{
			if (itemIds.Contains(this.inventory[i].ITEM_ID))
			{
				return this.inventory[i].ITEM_ID;
			}
		}
		return "null";
	}

	// Token: 0x06000154 RID: 340 RVA: 0x0000AE88 File Offset: 0x00009088
	public void RemoveCurrentHeldItemOne()
	{
		if (!WorldPolicy.creativeMode)
		{
			this.inventory[this.ReturnSelectedSlotId()] = new InventorySlot
			{
				ITEM_ID = this.ReturnSelectedSlot().ITEM_ID,
				ITEM_STACK = this.ReturnSelectedSlot().ITEM_STACK - 1
			};
		}
		if (this.ReturnSelectedSlot().ITEM_STACK <= 0)
		{
			this.inventory[this.ReturnSelectedSlotId()] = InventorySlot.EmptySlot();
			if (this.draggingSlot)
			{
				this.draggingSlot = false;
				this.pickingOutItems = false;
				this.creativePickingOut = false;
				this.draggedSlot = 0;
			}
		}
	}

	// Token: 0x06000155 RID: 341 RVA: 0x0000AF24 File Offset: 0x00009124
	public void ResetInventory(bool includeTools)
	{
		this.inventory.Clear();
		foreach (InventorySlot inventorySlot in this.newWorldInventory)
		{
			if (includeTools)
			{
				if (inventorySlot.ITEM_ID != "pain_void" || (!WorldPolicy.hellholeMode && !WorldPolicy.trainwreckMode))
				{
					this.inventory.Add(inventorySlot);
				}
				else
				{
					this.inventory.Add(InventorySlot.EmptySlot());
				}
			}
			else
			{
				this.inventory.Add(InventorySlot.EmptySlot());
			}
		}
	}

	// Token: 0x06000156 RID: 342 RVA: 0x0000AFD0 File Offset: 0x000091D0
	public PickupItem InitializePickupItem(Vector2 pos, string i_id, int i_quan, bool pickUpInstant)
	{
		GameObject gameObject = Object.Instantiate<GameObject>(this.pickupItem);
		PickupItem component = gameObject.GetComponent<PickupItem>();
		component.InitializePickupItem(i_id, i_quan);
		if (pickUpInstant)
		{
			component.EnablePickingUp();
		}
		gameObject.transform.position = pos;
		return component;
	}

	// Token: 0x06000157 RID: 343 RVA: 0x0000B014 File Offset: 0x00009214
	public void LootChest()
	{
		for (int i = 44; i <= 59; i++)
		{
			this.InitializePickupItem(this.playerPos, this.inventory[i].ITEM_ID, this.inventory[i].ITEM_STACK, true);
			this.inventory[i] = InventorySlot.EmptySlot();
		}
	}

	// Token: 0x06000158 RID: 344 RVA: 0x0000B070 File Offset: 0x00009270
	public string GetNewLevelGuid()
	{
		string text;
		do
		{
			text = Guid.NewGuid().ToString();
		}
		while (this.availableLevels.Contains(text));
		return text;
	}

	// Token: 0x06000159 RID: 345 RVA: 0x0000B0A0 File Offset: 0x000092A0
	public bool IsAccesorySlot(int slot)
	{
		return slot == 40 || slot == 41 || slot == 42 || slot == 43;
	}

	// Token: 0x0600015A RID: 346 RVA: 0x0000B0BF File Offset: 0x000092BF
	public bool IsArmorSlot(int slot)
	{
		return slot == 37 || slot == 38 || slot == 39;
	}

	// Token: 0x0600015B RID: 347 RVA: 0x0000B0D7 File Offset: 0x000092D7
	public bool IsAnySpecialSlot(int slot)
	{
		return slot == 37 || slot == 38 || slot == 39 || slot == 40 || slot == 41 || slot == 42 || slot == 43;
	}

	// Token: 0x0600015C RID: 348 RVA: 0x0000B10C File Offset: 0x0000930C
	private void UpdateExtraCritChance()
	{
		this.extraCritChance = 0;
		if (!this.CheckIfSlotDragged(37))
		{
			this.extraCritChance += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[37].ITEM_ID).accesoryBoosts.critChance;
		}
		if (!this.CheckIfSlotDragged(38))
		{
			this.extraCritChance += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[38].ITEM_ID).accesoryBoosts.critChance;
		}
		if (!this.CheckIfSlotDragged(39))
		{
			this.extraCritChance += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[39].ITEM_ID).accesoryBoosts.critChance;
		}
		if (!this.CheckIfSlotDragged(40))
		{
			this.extraCritChance += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[40].ITEM_ID).accesoryBoosts.critChance;
		}
		if (!this.CheckIfSlotDragged(41))
		{
			this.extraCritChance += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[41].ITEM_ID).accesoryBoosts.critChance;
		}
		if (!this.CheckIfSlotDragged(42))
		{
			this.extraCritChance += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[42].ITEM_ID).accesoryBoosts.critChance;
		}
		if (!this.CheckIfSlotDragged(43))
		{
			this.extraCritChance += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[43].ITEM_ID).accesoryBoosts.critChance;
		}
		if (this.HasStatusEffect("crit"))
		{
			this.extraCritChance += 4;
		}
	}

	// Token: 0x0600015D RID: 349 RVA: 0x0000B2F0 File Offset: 0x000094F0
	private void UpdateExtraPickupRange()
	{
		this.extraPickupRange = 0f;
		if (!this.CheckIfSlotDragged(37))
		{
			this.extraPickupRange += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[37].ITEM_ID).accesoryBoosts.pickupRange;
		}
		if (!this.CheckIfSlotDragged(38))
		{
			this.extraPickupRange += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[38].ITEM_ID).accesoryBoosts.pickupRange;
		}
		if (!this.CheckIfSlotDragged(39))
		{
			this.extraPickupRange += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[39].ITEM_ID).accesoryBoosts.pickupRange;
		}
		if (!this.CheckIfSlotDragged(40))
		{
			this.extraPickupRange += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[40].ITEM_ID).accesoryBoosts.pickupRange;
		}
		if (!this.CheckIfSlotDragged(41))
		{
			this.extraPickupRange += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[41].ITEM_ID).accesoryBoosts.pickupRange;
		}
		if (!this.CheckIfSlotDragged(42))
		{
			this.extraPickupRange += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[42].ITEM_ID).accesoryBoosts.pickupRange;
		}
		if (!this.CheckIfSlotDragged(43))
		{
			this.extraPickupRange += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[43].ITEM_ID).accesoryBoosts.pickupRange;
		}
	}

	// Token: 0x0600015E RID: 350 RVA: 0x0000B4BC File Offset: 0x000096BC
	private void UpdateDefense()
	{
		this.defense = 0;
		if (!this.CheckIfSlotDragged(37))
		{
			this.defense += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[37].ITEM_ID).accesoryBoosts.defenseBoost;
		}
		if (!this.CheckIfSlotDragged(38))
		{
			this.defense += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[38].ITEM_ID).accesoryBoosts.defenseBoost;
		}
		if (!this.CheckIfSlotDragged(39))
		{
			this.defense += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[39].ITEM_ID).accesoryBoosts.defenseBoost;
		}
		if (!this.CheckIfSlotDragged(40))
		{
			this.defense += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[40].ITEM_ID).accesoryBoosts.defenseBoost;
		}
		if (!this.CheckIfSlotDragged(41))
		{
			this.defense += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[41].ITEM_ID).accesoryBoosts.defenseBoost;
		}
		if (!this.CheckIfSlotDragged(42))
		{
			this.defense += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[42].ITEM_ID).accesoryBoosts.defenseBoost;
		}
		if (!this.CheckIfSlotDragged(43))
		{
			this.defense += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[43].ITEM_ID).accesoryBoosts.defenseBoost;
		}
		if (this.HasStatusEffect("protection"))
		{
			this.defense += 8;
		}
	}

	// Token: 0x0600015F RID: 351 RVA: 0x0000B6A0 File Offset: 0x000098A0
	private void UpdateExtraMovementSpeed()
	{
		this.extraMovementSpeed = 0f;
		if (!this.CheckIfSlotDragged(37))
		{
			this.extraMovementSpeed += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[37].ITEM_ID).accesoryBoosts.movementSpeed;
		}
		if (!this.CheckIfSlotDragged(38))
		{
			this.extraMovementSpeed += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[38].ITEM_ID).accesoryBoosts.movementSpeed;
		}
		if (!this.CheckIfSlotDragged(39))
		{
			this.extraMovementSpeed += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[39].ITEM_ID).accesoryBoosts.movementSpeed;
		}
		if (!this.CheckIfSlotDragged(40))
		{
			this.extraMovementSpeed += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[40].ITEM_ID).accesoryBoosts.movementSpeed;
		}
		if (!this.CheckIfSlotDragged(41))
		{
			this.extraMovementSpeed += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[41].ITEM_ID).accesoryBoosts.movementSpeed;
		}
		if (!this.CheckIfSlotDragged(42))
		{
			this.extraMovementSpeed += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[42].ITEM_ID).accesoryBoosts.movementSpeed;
		}
		if (!this.CheckIfSlotDragged(43))
		{
			this.extraMovementSpeed += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[43].ITEM_ID).accesoryBoosts.movementSpeed;
		}
		if (this.HasStatusEffect("speed"))
		{
			this.extraMovementSpeed += 1.8f;
		}
		if (this.rotativeCameraFeature)
		{
			this.extraMovementSpeed += 5f;
		}
	}

	// Token: 0x06000160 RID: 352 RVA: 0x0000B8A4 File Offset: 0x00009AA4
	private void UpdateExtraDamage()
	{
		this.extraDamage = 0f;
		if (!this.CheckIfSlotDragged(37))
		{
			this.extraDamage += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[37].ITEM_ID).accesoryBoosts.damageBoost;
		}
		if (!this.CheckIfSlotDragged(38))
		{
			this.extraDamage += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[38].ITEM_ID).accesoryBoosts.damageBoost;
		}
		if (!this.CheckIfSlotDragged(39))
		{
			this.extraDamage += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[39].ITEM_ID).accesoryBoosts.damageBoost;
		}
		if (!this.CheckIfSlotDragged(40))
		{
			this.extraDamage += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[40].ITEM_ID).accesoryBoosts.damageBoost;
		}
		if (!this.CheckIfSlotDragged(41))
		{
			this.extraDamage += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[41].ITEM_ID).accesoryBoosts.damageBoost;
		}
		if (!this.CheckIfSlotDragged(42))
		{
			this.extraDamage += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[42].ITEM_ID).accesoryBoosts.damageBoost;
		}
		if (!this.CheckIfSlotDragged(43))
		{
			this.extraDamage += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[43].ITEM_ID).accesoryBoosts.damageBoost;
		}
		if (this.sunflowerPositions.Find((PlacedBlock s) => Vector2.Distance(this.playerPos, s.transform.position) < 15f) != null)
		{
			this.extraDamage += 6f;
		}
		if (this.HasStatusEffect("strength"))
		{
			this.extraDamage += 4f;
		}
	}

	// Token: 0x06000161 RID: 353 RVA: 0x0000BAC0 File Offset: 0x00009CC0
	private void UpdateExtraDashLength()
	{
		this.extraDashLength = 0f;
		if (!this.CheckIfSlotDragged(37))
		{
			this.extraDashLength += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[37].ITEM_ID).accesoryBoosts.dashBoost;
		}
		if (!this.CheckIfSlotDragged(38))
		{
			this.extraDashLength += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[38].ITEM_ID).accesoryBoosts.dashBoost;
		}
		if (!this.CheckIfSlotDragged(39))
		{
			this.extraDashLength += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[39].ITEM_ID).accesoryBoosts.dashBoost;
		}
		if (!this.CheckIfSlotDragged(40))
		{
			this.extraDashLength += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[40].ITEM_ID).accesoryBoosts.dashBoost;
		}
		if (!this.CheckIfSlotDragged(41))
		{
			this.extraDashLength += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[41].ITEM_ID).accesoryBoosts.dashBoost;
		}
		if (!this.CheckIfSlotDragged(42))
		{
			this.extraDashLength += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[42].ITEM_ID).accesoryBoosts.dashBoost;
		}
		if (!this.CheckIfSlotDragged(43))
		{
			this.extraDashLength += this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[43].ITEM_ID).accesoryBoosts.dashBoost;
		}
	}

	// Token: 0x06000162 RID: 354 RVA: 0x0000BC8C File Offset: 0x00009E8C
	private void UpdateExtraBuildRange()
	{
		this.extraBuildRange = 0f;
		if (!this.CheckIfSlotDragged(37))
		{
			this.extraBuildRange += (float)this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[37].ITEM_ID).accesoryBoosts.buildingRange;
		}
		if (!this.CheckIfSlotDragged(38))
		{
			this.extraBuildRange += (float)this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[38].ITEM_ID).accesoryBoosts.buildingRange;
		}
		if (!this.CheckIfSlotDragged(39))
		{
			this.extraBuildRange += (float)this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[39].ITEM_ID).accesoryBoosts.buildingRange;
		}
		if (!this.CheckIfSlotDragged(40))
		{
			this.extraBuildRange += (float)this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[40].ITEM_ID).accesoryBoosts.buildingRange;
		}
		if (!this.CheckIfSlotDragged(41))
		{
			this.extraBuildRange += (float)this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[41].ITEM_ID).accesoryBoosts.buildingRange;
		}
		if (!this.CheckIfSlotDragged(42))
		{
			this.extraBuildRange += (float)this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[42].ITEM_ID).accesoryBoosts.buildingRange;
		}
		if (!this.CheckIfSlotDragged(43))
		{
			this.extraBuildRange += (float)this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[43].ITEM_ID).accesoryBoosts.buildingRange;
		}
		if (this.HasStatusEffect("range"))
		{
			this.extraBuildRange += 1f;
		}
	}

	// Token: 0x06000163 RID: 355 RVA: 0x0000BE7C File Offset: 0x0000A07C
	public bool EquippedAccesory(string accesory_id)
	{
		bool result = false;
		if (this.inventory[40].ITEM_ID == accesory_id && !this.CheckIfSlotDragged(40))
		{
			result = true;
		}
		if (this.inventory[41].ITEM_ID == accesory_id && !this.CheckIfSlotDragged(41))
		{
			result = true;
		}
		if (this.inventory[42].ITEM_ID == accesory_id && !this.CheckIfSlotDragged(42))
		{
			result = true;
		}
		if (this.inventory[43].ITEM_ID == accesory_id && !this.CheckIfSlotDragged(43))
		{
			result = true;
		}
		return result;
	}

	// Token: 0x06000164 RID: 356 RVA: 0x0000BF24 File Offset: 0x0000A124
	public bool EquippedShield()
	{
		bool result = false;
		if (this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[40].ITEM_ID).specialUsage == "Shield" && !this.CheckIfSlotDragged(40))
		{
			result = true;
		}
		if (this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[41].ITEM_ID).specialUsage == "Shield" && !this.CheckIfSlotDragged(41))
		{
			result = true;
		}
		if (this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[42].ITEM_ID).specialUsage == "Shield" && !this.CheckIfSlotDragged(42))
		{
			result = true;
		}
		if (this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[43].ITEM_ID).specialUsage == "Shield" && !this.CheckIfSlotDragged(43))
		{
			result = true;
		}
		return result;
	}

	// Token: 0x06000165 RID: 357 RVA: 0x0000C01C File Offset: 0x0000A21C
	public void FoodShortcut()
	{
		int i = 0;
		while (i <= 35)
		{
			Item item = this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[i].ITEM_ID);
			if (item.heal > 0 && item.lifeFlower <= 0)
			{
				AudioManager.instance.Play(item.useSound);
				this.AddStatusEffect("full");
				this.health += item.heal;
				if (this.EquippedAccesory("speed_glove"))
				{
					this.itemCooldown = item.useCooldown / 2f;
				}
				else
				{
					this.itemCooldown = item.useCooldown;
				}
				if (!WorldPolicy.creativeMode)
				{
					this.inventory[i] = new InventorySlot
					{
						ITEM_ID = this.inventory[i].ITEM_ID,
						ITEM_STACK = this.inventory[i].ITEM_STACK - 1
					};
				}
				if (item.specialUsage == "Drink" && !WorldPolicy.creativeMode)
				{
					this.InitializePickupItem(GameManager.instance.playerPos, "bottle", 1, true);
				}
				if (this.inventory[i].ITEM_STACK <= 0)
				{
					this.inventory[i] = InventorySlot.EmptySlot();
					return;
				}
				break;
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x06000166 RID: 358 RVA: 0x0000C174 File Offset: 0x0000A374
	private LootDrop ReturnFishingItemId()
	{
		LootDrop result = null;
		foreach (LootDrop lootDrop in this.fishingLootTable.drops)
		{
			if (UtilityMath.RandomChance(1f / (lootDrop.DROP_CHANCE - 1f)) && lootDrop.DROP_ID != "null" && lootDrop.DROP_STACK_MIN != 0 && lootDrop.DROP_STACK_MAX != 0)
			{
				result = lootDrop;
			}
		}
		return result;
	}

	// Token: 0x06000167 RID: 359 RVA: 0x0000C208 File Offset: 0x0000A408
	public void InvasionEvent(int enemies, Invasion invasion)
	{
		this.invasionEnemiesLeft = enemies;
		this.invasionEvent = invasion;
	}

	// Token: 0x06000168 RID: 360 RVA: 0x0000C218 File Offset: 0x0000A418
	public string InvasionName(Invasion invasion)
	{
		if (invasion == Invasion.CubicChaos)
		{
			return "Cubic Chaos";
		}
		return string.Empty;
	}

	// Token: 0x06000169 RID: 361 RVA: 0x0000C229 File Offset: 0x0000A429
	public int InvasionEnemyAmount(Invasion invasion)
	{
		if (invasion == Invasion.CubicChaos)
		{
			return 250;
		}
		return 1;
	}

	// Token: 0x0600016A RID: 362 RVA: 0x0000C236 File Offset: 0x0000A436
	public void SetWorldGenerationStep(int stepsNeeded, string stepString)
	{
		this.levelGenerationStepsNeeded = stepsNeeded;
		this.levelGenerationStep = (WorldPolicy.trainwreckMode ? UtilityMath.ReverseString(stepString) : stepString);
		this.levelGenerationProgress = 0;
	}

	// Token: 0x0600016B RID: 363 RVA: 0x0000C25C File Offset: 0x0000A45C
	public int ReturnRandomSeed()
	{
		return Random.Range(-2140000000, 2140000001);
	}

	// Token: 0x0600016C RID: 364 RVA: 0x0000C26D File Offset: 0x0000A46D
	public void OpenStructureBuilder()
	{
		SceneManager.LoadScene("StructureBuilderScene");
	}

	// Token: 0x0600016D RID: 365 RVA: 0x0000C279 File Offset: 0x0000A479
	public void OpenCreativeModeTab(CreativeMenuTab tab, bool refresh)
	{
		if (this.creativeMenuTab != tab || refresh)
		{
			this.ForceOpenCreativeModeTab(tab);
		}
	}

	// Token: 0x0600016E RID: 366 RVA: 0x0000C294 File Offset: 0x0000A494
	private void ForceOpenCreativeModeTab(CreativeMenuTab tab)
	{
		this.creativeItemIds.Clear();
		foreach (string text in this.allCreativeItemIds)
		{
			if (text == "null" || ((tab == CreativeMenuTab.All || this.itemRegistryReference.FindItemByInternalIdentifier(text).tab == tab) && (this.itemRegistryReference.FindItemByInternalIdentifier(text).name.ToLower().Trim().Contains(this.creativeModeSearch.ToLower().Trim()) || this.itemRegistryReference.FindItemByInternalIdentifier(text).description1.ToLower().Trim().Contains(this.creativeModeSearch.ToLower().Trim()) || this.itemRegistryReference.FindItemByInternalIdentifier(text).description2.ToLower().Trim().Contains(this.creativeModeSearch.ToLower().Trim()))))
			{
				this.creativeItemIds.Add(text);
			}
		}
		this.creativeTab = 0;
		this.creativeMenuTab = tab;
	}

	// Token: 0x0600016F RID: 367 RVA: 0x0000C3A6 File Offset: 0x0000A5A6
	public int ReturnMaxCreativePage()
	{
		if (this.creativeItemIds.Count < 2)
		{
			return 0;
		}
		return Mathf.FloorToInt((float)(this.creativeItemIds.Count - 2) / 24f);
	}

	// Token: 0x06000170 RID: 368 RVA: 0x0000C3D1 File Offset: 0x0000A5D1
	public int ReturnMaxRecipePage()
	{
		if (this.currentRecipes.Count < 1)
		{
			return 0;
		}
		return Mathf.FloorToInt((float)(this.currentRecipes.Count - 1) / 18f);
	}

	// Token: 0x06000171 RID: 369 RVA: 0x0000C3FC File Offset: 0x0000A5FC
	public Sprite ReturnPlayerPartSprite(int index)
	{
		Sprite sprite = this.FindArmor(index);
		if (sprite != null)
		{
			return sprite;
		}
		return this.playerSkin[index];
	}

	// Token: 0x06000172 RID: 370 RVA: 0x0000C424 File Offset: 0x0000A624
	public Sprite FindArmor(int index)
	{
		if (!this.CheckIfSlotDragged(37) && this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[37].ITEM_ID).armorSkinSprites[index] != null)
		{
			return this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[37].ITEM_ID).armorSkinSprites[index];
		}
		if (!this.CheckIfSlotDragged(38) && this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[38].ITEM_ID).armorSkinSprites[index] != null)
		{
			return this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[38].ITEM_ID).armorSkinSprites[index];
		}
		if (!this.CheckIfSlotDragged(39) && this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[39].ITEM_ID).armorSkinSprites[index] != null)
		{
			return this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[39].ITEM_ID).armorSkinSprites[index];
		}
		return null;
	}

	// Token: 0x06000173 RID: 371 RVA: 0x0000C544 File Offset: 0x0000A744
	public void ReloadCustomSkin()
	{
		this.ReloadCustomSkinPart(0, "preview.png");
		this.ReloadCustomSkinPart(1, "head.png");
		this.ReloadCustomSkinPart(2, "eyes_open.png");
		this.ReloadCustomSkinPart(3, "eyes_closed.png");
		this.ReloadCustomSkinPart(4, "neck.png");
		this.ReloadCustomSkinPart(5, "torso.png");
		this.ReloadCustomSkinPart(6, "left_hand_top.png");
		this.ReloadCustomSkinPart(7, "right_hand_top.png");
		this.ReloadCustomSkinPart(8, "left_hand_bottom.png");
		this.ReloadCustomSkinPart(9, "right_hand_bottom.png");
		this.ReloadCustomSkinPart(10, "pants.png");
		this.ReloadCustomSkinPart(11, "left_leg_top.png");
		this.ReloadCustomSkinPart(12, "right_leg_top.png");
		this.ReloadCustomSkinPart(13, "left_leg_bottom.png");
		this.ReloadCustomSkinPart(14, "right_leg_bottom.png");
	}

	// Token: 0x06000174 RID: 372 RVA: 0x0000C60C File Offset: 0x0000A80C
	private void ReloadCustomSkinPart(int index, string fileName)
	{
		Sprite customSkinTexture = CustomSkinUtility.GetCustomSkinTexture(fileName);
		if (customSkinTexture != null)
		{
			this.playerSkin[index] = customSkinTexture;
			return;
		}
		this.playerSkin[index] = this.defaultSkin[index];
	}

	// Token: 0x06000175 RID: 373 RVA: 0x0000C643 File Offset: 0x0000A843
	public bool IsArmorGlowmask(int inventoryIndex)
	{
		return !this.CheckIfSlotDragged(inventoryIndex) && this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[inventoryIndex].ITEM_ID).isArmorGlowmask;
	}

	// Token: 0x06000176 RID: 374 RVA: 0x0000C671 File Offset: 0x0000A871
	public Sprite ReturnHeadAccesory()
	{
		if (!this.CheckIfSlotDragged(37))
		{
			return this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[37].ITEM_ID).headAccesorySprite;
		}
		return null;
	}

	// Token: 0x06000177 RID: 375 RVA: 0x0000C6A1 File Offset: 0x0000A8A1
	public bool IsMiningHelmet()
	{
		return !this.CheckIfSlotDragged(37) && this.itemRegistryReference.FindItemByInternalIdentifier(this.inventory[37].ITEM_ID).isMiningHelmet;
	}

	// Token: 0x06000178 RID: 376 RVA: 0x0000C6D4 File Offset: 0x0000A8D4
	public void Respawn()
	{
		if (this.playerMovement == null && !this.generatingWorld && !this.EquippedAccesory("ukrainium_guard"))
		{
			this.tooltipCustom = string.Empty;
			if (WorldPolicy.hardcoreMode)
			{
				PlayerPrefs.DeleteKey(WorldManager.instance.loadedWorld.storageName);
				this.availableLevels.Remove(WorldManager.instance.loadedWorld.storageName);
				this.SaveAvailableLevels();
				this.ExitToMainMenu(false);
				return;
			}
			this.isInMines = false;
			this.health = this.ReturnMaxHealth();
			WorldGeneration worldGeneration = Object.FindObjectOfType<WorldGeneration>();
			if (worldGeneration == null)
			{
				return;
			}
			worldGeneration.SpawnPlayer(new Vector2(this.respawnPos.x, this.respawnPos.y));
		}
	}

	// Token: 0x06000179 RID: 377 RVA: 0x0000C799 File Offset: 0x0000A999
	public void AddBossFlag(string bossFlag)
	{
		if (!this.defeatBosses.Contains(bossFlag))
		{
			this.defeatBosses.Add(bossFlag);
		}
	}

	// Token: 0x0600017A RID: 378 RVA: 0x0000C7B5 File Offset: 0x0000A9B5
	public ActiveStatusEffect StatusEffectIndex(int index)
	{
		if (index >= this.statusEffects.Count)
		{
			return null;
		}
		if (this.statusEffectRegistryReference.FindStatusEffectByInternalIdentifier(this.statusEffects[index].statusId) != null)
		{
			return this.statusEffects[index];
		}
		return null;
	}

	// Token: 0x0600017B RID: 379 RVA: 0x0000C7F4 File Offset: 0x0000A9F4
	public bool HasStatusEffect(string idName)
	{
		return this.statusEffects.Find((ActiveStatusEffect e) => e.statusId == idName) != null;
	}

	// Token: 0x0600017C RID: 380 RVA: 0x0000C828 File Offset: 0x0000AA28
	public void AddStatusEffect(string idName)
	{
		if (!this.HasStatusEffect(idName) && this.statusEffectRegistryReference.FindStatusEffectByInternalIdentifier(idName) != null)
		{
			this.statusEffects.Add(new ActiveStatusEffect
			{
				statusId = idName,
				statusTimeLeft = this.statusEffectRegistryReference.FindStatusEffectByInternalIdentifier(idName).defaultDurationSeconds
			});
		}
	}

	// Token: 0x0600017D RID: 381 RVA: 0x0000C87C File Offset: 0x0000AA7C
	private void StatusEffectControl(float delta)
	{
		List<ActiveStatusEffect> list = new List<ActiveStatusEffect>();
		foreach (ActiveStatusEffect activeStatusEffect in this.statusEffects)
		{
			activeStatusEffect.statusTimeLeft -= delta;
			if (activeStatusEffect.statusTimeLeft <= 0f || this.statusEffectRegistryReference.FindStatusEffectByInternalIdentifier(activeStatusEffect.statusId) == null)
			{
				list.Add(activeStatusEffect);
			}
		}
		foreach (ActiveStatusEffect item in list)
		{
			this.statusEffects.Remove(item);
		}
	}

	// Token: 0x0600017E RID: 382 RVA: 0x0000C948 File Offset: 0x0000AB48
	public void CreateWorld(string name, string guid, string seed, float size, bool creative, bool hardcore, bool enemySpawning, bool keepInventory, bool advancedTooltips, bool dayNightCycle, bool infiniteRange, bool allowSwitchingGamemode, bool experimentalFeatures, int worldIconId)
	{
		if (this.gameState == GameState.MainMenu)
		{
			if (this.ReturnAvailableLevelNames().Contains(name) || this.availableLevels.Contains(guid) || name.Length <= 0)
			{
				this.mainMenuSubmenu = 4;
				return;
			}
			this.showAdvancedWorldgen = false;
			WorldManager.instance.isNewWorld = true;
			WorldManager.instance.loadedWorld = new World();
			WorldManager.instance.loadedWorld.name = name;
			WorldManager.instance.loadedWorld.storageName = guid;
			WorldManager.instance.loadedWorld.worldScale = size;
			WorldManager.instance.loadedWorld.worldIconId = worldIconId;
			bool flag = seed.ToLower() == "easy mode";
			bool flag2 = seed.ToLower() == "game too hard";
			bool flag3 = seed.ToLower().Contains("3d");
			bool flag4 = seed.ToLower() == "nostalgia";
			bool flag5 = seed.ToLower() == "sniper skills" || seed.ToLower() == "ss2" || seed.ToLower() == "square shooter" || seed.ToLower() == "square shooter 2";
			bool flag6 = seed.ToLower() == "kyiv" || seed.ToLower() == "kiev";
			if (seed == string.Empty || flag || flag3 || flag4 || flag5 || flag2 || flag6)
			{
				WorldPolicy.worldSeed = this.ReturnRandomSeed();
			}
			else
			{
				if (UtilityMath.IsNumeric(seed))
				{
					try
					{
						WorldPolicy.worldSeed = Convert.ToInt32(seed);
						goto IL_1A7;
					}
					catch
					{
						WorldPolicy.worldSeed = this.ReturnRandomSeed();
						goto IL_1A7;
					}
				}
				WorldPolicy.worldSeed = seed.GetHashCode();
			}
			IL_1A7:
			WorldManager.instance.loadedWorld.playerPosX = 0f;
			if (flag4 || flag5)
			{
				WorldManager.instance.loadedWorld.playerPosY = 0f;
			}
			else
			{
				WorldManager.instance.loadedWorld.playerPosY = (float)(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-148) + 5);
			}
			this.respawnPos = new Vector2(WorldManager.instance.loadedWorld.playerPosX, WorldManager.instance.loadedWorld.playerPosY);
			this.selectedTeleporter = null;
			WorldPolicy.creativeMode = (creative && !flag && !flag2 && !hardcore);
			WorldPolicy.hardcoreMode = hardcore;
			WorldPolicy.hellholeMode = (flag || flag2);
			WorldPolicy.trainwreckMode = flag2;
			if (hardcore || flag || flag2)
			{
				WorldPolicy.spawnMobs = true;
				WorldPolicy.keepInventory = false;
				WorldPolicy.advancedTooltips = false;
				WorldPolicy.dayNightCycle = true;
				WorldPolicy.infiniteBuildRange = false;
				WorldPolicy.allowSwitchingGamemodes = false;
				WorldPolicy.experimentalFeatures = experimentalFeatures;
			}
			else
			{
				WorldPolicy.spawnMobs = enemySpawning;
				WorldPolicy.keepInventory = keepInventory;
				WorldPolicy.advancedTooltips = advancedTooltips;
				WorldPolicy.dayNightCycle = dayNightCycle;
				WorldPolicy.infiniteBuildRange = infiniteRange;
				WorldPolicy.allowSwitchingGamemodes = allowSwitchingGamemode;
				WorldPolicy.experimentalFeatures = experimentalFeatures;
			}
			this.fullTime = 300f;
			this.rain = false;
			this.health = 20;
			this.extraHealth = 0;
			this.money = 0;
			this.rotativeCameraFeature = flag3;
			this.nostalgiaSeed = flag4;
			this.sniperSkillsSeed = flag5;
			this.kyivSeed = flag6;
			this.defeatBosses.Clear();
			this.statusEffects.Clear();
			this.highlightableBlocks.Clear();
			this.availableLevels.Add(WorldManager.instance.loadedWorld.storageName);
			this.SaveAvailableLevels();
			this.ResetInventory(true);
			this.mainMenuSubmenu = 9;
			AchievementManager.instance.AddAchievement(156192);
			SceneManager.LoadSceneAsync("CoreGame");
		}
	}

	// Token: 0x0600017F RID: 383 RVA: 0x0000CCD4 File Offset: 0x0000AED4
	public void StartWorldLoad()
	{
		if (this.gameState == GameState.MainMenu)
		{
			if (WorldManager.instance.loadedWorld.versionId < 100)
			{
				WorldManager.instance.loadedWorld.storageName = WorldManager.instance.loadedWorld.name;
			}
			WorldPolicy.worldSeed = WorldManager.instance.loadedWorld.seed;
			WorldPolicy.creativeMode = (WorldManager.instance.loadedWorld.creative && !WorldManager.instance.loadedWorld.hardcoreMode && !WorldManager.instance.loadedWorld.hellholeMode && !WorldManager.instance.loadedWorld.trainwreckMode);
			WorldPolicy.spawnMobs = WorldManager.instance.loadedWorld.spawning;
			WorldPolicy.keepInventory = WorldManager.instance.loadedWorld.keepInventory;
			WorldPolicy.advancedTooltips = WorldManager.instance.loadedWorld.advancedTooltips;
			WorldPolicy.dayNightCycle = WorldManager.instance.loadedWorld.dayNightCycle;
			WorldPolicy.hardcoreMode = WorldManager.instance.loadedWorld.hardcoreMode;
			WorldPolicy.infiniteBuildRange = WorldManager.instance.loadedWorld.infiniteBuildRange;
			WorldPolicy.allowSwitchingGamemodes = WorldManager.instance.loadedWorld.allowSwitchingGamemodes;
			WorldPolicy.hellholeMode = WorldManager.instance.loadedWorld.hellholeMode;
			WorldPolicy.trainwreckMode = WorldManager.instance.loadedWorld.trainwreckMode;
			WorldPolicy.experimentalFeatures = WorldManager.instance.loadedWorld.experimentalFeatures;
			this.fullTime = WorldManager.instance.loadedWorld.time;
			this.rain = WorldManager.instance.loadedWorld.rain;
			this.health = WorldManager.instance.loadedWorld.health;
			this.extraHealth = WorldManager.instance.loadedWorld.extraHealth;
			this.money = WorldManager.instance.loadedWorld.money;
			this.rotativeCameraFeature = WorldManager.instance.loadedWorld.rotativeCameraFeature;
			this.nostalgiaSeed = WorldManager.instance.loadedWorld.nostalgiaSeed;
			this.sniperSkillsSeed = WorldManager.instance.loadedWorld.sniperSkillsSeed;
			this.kyivSeed = WorldManager.instance.loadedWorld.kyivSeed;
			this.respawnPos = new Vector2(WorldManager.instance.loadedWorld.playerPosX, WorldManager.instance.loadedWorld.playerPosY);
			this.mainMenuSubmenu = 9;
			SceneManager.LoadSceneAsync("CoreGame");
		}
	}

	// Token: 0x06000180 RID: 384 RVA: 0x0000CF40 File Offset: 0x0000B140
	public NewsBulletin GetNews(int index)
	{
		if (index >= this.downloadedNews.news.Count)
		{
			return new NewsBulletin
			{
				header = string.Empty,
				article = string.Empty,
				link = string.Empty
			};
		}
		if (this.downloadedNews.news[index] != null)
		{
			return this.downloadedNews.news[index];
		}
		return new NewsBulletin
		{
			header = string.Empty,
			article = string.Empty,
			link = string.Empty
		};
	}

	// Token: 0x06000181 RID: 385 RVA: 0x0000CFD2 File Offset: 0x0000B1D2
	public int GetDownloadedNewsCount()
	{
		return this.downloadedNews.news.Count;
	}

	// Token: 0x06000182 RID: 386 RVA: 0x0000CFE4 File Offset: 0x0000B1E4
	public Emote ReturnCurrentEmote()
	{
		return this.emoteRegistryReference.emotes[this.emoteIndex];
	}

	// Token: 0x06000183 RID: 387 RVA: 0x0000CFF8 File Offset: 0x0000B1F8
	public void MainMenuCameraZoomStart()
	{
		this.mainMenuCameraZoom = 0.1f;
	}

	// Token: 0x06000184 RID: 388 RVA: 0x0000D008 File Offset: 0x0000B208
	public bool AllowInteractablity()
	{
		return this.selectedTeleporter == null && this.bed == null && !this.generatingWorld && this.playerMovement != null && this.chair == null && this.sign == null;
	}

	// Token: 0x04000142 RID: 322
	public static GameManager instance;

	// Token: 0x04000143 RID: 323
	public ItemRegistry itemRegistryReference;

	// Token: 0x04000144 RID: 324
	public RecipeRegistry recipeRegistryReference;

	// Token: 0x04000145 RID: 325
	public BlockRegistry blockRegistryReference;

	// Token: 0x04000146 RID: 326
	public FloorRegistry floorRegistryReference;

	// Token: 0x04000147 RID: 327
	public TreeRegistry treeRegistryReference;

	// Token: 0x04000148 RID: 328
	public FishingLootTable fishingLootTable;

	// Token: 0x04000149 RID: 329
	public StructureRegistry structureRegistryReference;

	// Token: 0x0400014A RID: 330
	public StatusEffectRegistry statusEffectRegistryReference;

	// Token: 0x0400014B RID: 331
	public PotionRegistry potionRegistryReference;

	// Token: 0x0400014C RID: 332
	public BestiaryRegistry bestiaryRegistryReference;

	// Token: 0x0400014D RID: 333
	public EmoteRegistry emoteRegistryReference;

	// Token: 0x0400014E RID: 334
	public LayerMask enemyLineOfSightMask;

	// Token: 0x0400014F RID: 335
	public Sprite[] playerSkin;

	// Token: 0x04000150 RID: 336
	public Sprite[] defaultSkin;

	// Token: 0x04000151 RID: 337
	public Sprite[] worldIcons;

	// Token: 0x04000152 RID: 338
	[SerializeField]
	private Material lit;

	// Token: 0x04000153 RID: 339
	[SerializeField]
	private Material unlit;

	// Token: 0x04000154 RID: 340
	public Font gameFont;

	// Token: 0x04000155 RID: 341
	public Font fallbackFont;

	// Token: 0x04000156 RID: 342
	[HideInInspector]
	public List<Recipe> currentRecipes = new List<Recipe>();

	// Token: 0x04000157 RID: 343
	public GameObject pickupItem;

	// Token: 0x04000158 RID: 344
	public GameObject enemyCorpse;

	// Token: 0x04000159 RID: 345
	public GameObject enemyBar;

	// Token: 0x0400015A RID: 346
	public List<InventorySlot> inventory = new List<InventorySlot>(62);

	// Token: 0x0400015B RID: 347
	public List<InventorySlot> newWorldInventory = new List<InventorySlot>(62);

	// Token: 0x0400015C RID: 348
	[HideInInspector]
	public bool draggingSlot;

	// Token: 0x0400015D RID: 349
	[HideInInspector]
	public bool pickingOutItems;

	// Token: 0x0400015E RID: 350
	[HideInInspector]
	public bool creativePickingOut;

	// Token: 0x0400015F RID: 351
	[HideInInspector]
	public int draggedSlot;

	// Token: 0x04000160 RID: 352
	[HideInInspector]
	public int pickedSlot;

	// Token: 0x04000161 RID: 353
	[HideInInspector]
	public int dragStart;

	// Token: 0x04000162 RID: 354
	[HideInInspector]
	public int dragEnd;

	// Token: 0x04000163 RID: 355
	[HideInInspector]
	public int tooltipIndex = -1;

	// Token: 0x04000164 RID: 356
	[HideInInspector]
	public string tooltipCustom = "";

	// Token: 0x04000165 RID: 357
	[HideInInspector]
	public int selectedItem;

	// Token: 0x04000166 RID: 358
	[HideInInspector]
	public int creativeTab;

	// Token: 0x04000167 RID: 359
	[HideInInspector]
	public int recipeTab;

	// Token: 0x04000168 RID: 360
	[HideInInspector]
	public CreativeMenuTab creativeMenuTab = CreativeMenuTab.All;

	// Token: 0x04000169 RID: 361
	[HideInInspector]
	public string creativeModeSearch = string.Empty;

	// Token: 0x0400016A RID: 362
	[HideInInspector]
	public int selectedRecipe = -1;

	// Token: 0x0400016B RID: 363
	[HideInInspector]
	public bool fullInventoryOpen;

	// Token: 0x0400016C RID: 364
	[HideInInspector]
	public float lightLevel = 0.2f;

	// Token: 0x0400016D RID: 365
	public int health = 20;

	// Token: 0x0400016E RID: 366
	[HideInInspector]
	public float invinciblityFrame;

	// Token: 0x0400016F RID: 367
	[HideInInspector]
	public int extraHealth;

	// Token: 0x04000170 RID: 368
	[HideInInspector]
	public Chest openedChest;

	// Token: 0x04000171 RID: 369
	[HideInInspector]
	public CraftingStation station;

	// Token: 0x04000172 RID: 370
	[HideInInspector]
	public float time = 300f;

	// Token: 0x04000173 RID: 371
	public float fullTime = 300f;

	// Token: 0x04000174 RID: 372
	public GameState gameState;

	// Token: 0x04000175 RID: 373
	private GameState tempState;

	// Token: 0x04000176 RID: 374
	[HideInInspector]
	public List<Vector2> blockPositions = new List<Vector2>();

	// Token: 0x04000177 RID: 375
	[HideInInspector]
	public List<Vector2> floorPositions = new List<Vector2>();

	// Token: 0x04000178 RID: 376
	[HideInInspector]
	public List<Vector2> treePositions = new List<Vector2>();

	// Token: 0x04000179 RID: 377
	[HideInInspector]
	public List<Vector2> wirePositions = new List<Vector2>();

	// Token: 0x0400017A RID: 378
	[HideInInspector]
	public List<Vector2> waterPositions = new List<Vector2>();

	// Token: 0x0400017B RID: 379
	[HideInInspector]
	public List<Vector2> bedHalfPositions = new List<Vector2>();

	// Token: 0x0400017C RID: 380
	[HideInInspector]
	public List<Vector2> leafBlockPositions = new List<Vector2>();

	// Token: 0x0400017D RID: 381
	[HideInInspector]
	public List<Vector2> spawnBlockerPositions = new List<Vector2>();

	// Token: 0x0400017E RID: 382
	[HideInInspector]
	public List<PlacedBlock> sunflowerPositions = new List<PlacedBlock>();

	// Token: 0x0400017F RID: 383
	[HideInInspector]
	public List<EnemyBase> enemyList = new List<EnemyBase>();

	// Token: 0x04000180 RID: 384
	[HideInInspector]
	public List<Farmland> farmlands = new List<Farmland>();

	// Token: 0x04000181 RID: 385
	[HideInInspector]
	public List<PlacedBlock> placedBlocks = new List<PlacedBlock>();

	// Token: 0x04000182 RID: 386
	[HideInInspector]
	public List<PlacedFloor> placedFloors = new List<PlacedFloor>();

	// Token: 0x04000183 RID: 387
	[HideInInspector]
	public List<PlacedTree> placedTrees = new List<PlacedTree>();

	// Token: 0x04000184 RID: 388
	[HideInInspector]
	public Vector2 selectionSquare;

	// Token: 0x04000185 RID: 389
	[HideInInspector]
	public PlayerMovement playerMovement;

	// Token: 0x04000186 RID: 390
	[HideInInspector]
	public Vector2 playerPos;

	// Token: 0x04000187 RID: 391
	[HideInInspector]
	public int mainMenuSubmenu;

	// Token: 0x04000188 RID: 392
	public bool displayAutosave;

	// Token: 0x04000189 RID: 393
	public List<string> availableLevels = new List<string>();

	// Token: 0x0400018A RID: 394
	public List<string> uncopiableLevels = new List<string>();

	// Token: 0x0400018B RID: 395
	[HideInInspector]
	public float itemCooldown;

	// Token: 0x0400018C RID: 396
	[HideInInspector]
	public int defense;

	// Token: 0x0400018D RID: 397
	public bool music = true;

	// Token: 0x0400018E RID: 398
	public bool sound = true;

	// Token: 0x0400018F RID: 399
	public bool ambience = true;

	// Token: 0x04000190 RID: 400
	public bool fullscreen = true;

	// Token: 0x04000191 RID: 401
	public bool blockAnimations = true;

	// Token: 0x04000192 RID: 402
	public bool treeTransparency = true;

	// Token: 0x04000193 RID: 403
	public bool enemyCorpses = true;

	// Token: 0x04000194 RID: 404
	public bool autosave = true;

	// Token: 0x04000195 RID: 405
	public bool interactionTooltips = true;

	// Token: 0x04000196 RID: 406
	public bool enemyHealthBars = true;

	// Token: 0x04000197 RID: 407
	public bool bossHealthBars = true;

	// Token: 0x04000198 RID: 408
	public float guiScale = 1350f;

	// Token: 0x04000199 RID: 409
	public bool isLinked;

	// Token: 0x0400019A RID: 410
	public string accountName = "";

	// Token: 0x0400019B RID: 411
	public string gameToken = "";

	// Token: 0x0400019C RID: 412
	public float zoom = 8f;

	// Token: 0x0400019D RID: 413
	public bool postProcessing;

	// Token: 0x0400019E RID: 414
	public string versionLastOpenedIn = "v";

	// Token: 0x0400019F RID: 415
	[HideInInspector]
	public Sign sign;

	// Token: 0x040001A0 RID: 416
	public bool rain;

	// Token: 0x040001A1 RID: 417
	[HideInInspector]
	public Bed bed;

	// Token: 0x040001A2 RID: 418
	[HideInInspector]
	public Chair chair;

	// Token: 0x040001A3 RID: 419
	[HideInInspector]
	public Vector2 playerBeforeBed;

	// Token: 0x040001A4 RID: 420
	[HideInInspector]
	public List<Vector2> highlightableBlocks;

	// Token: 0x040001A5 RID: 421
	[HideInInspector]
	public NpcShop npcShop;

	// Token: 0x040001A6 RID: 422
	[HideInInspector]
	public Lake lake;

	// Token: 0x040001A7 RID: 423
	[HideInInspector]
	public float fishingHitTime;

	// Token: 0x040001A8 RID: 424
	[HideInInspector]
	public float fishingCooldown;

	// Token: 0x040001A9 RID: 425
	public int money;

	// Token: 0x040001AA RID: 426
	[HideInInspector]
	public Boss boss;

	// Token: 0x040001AB RID: 427
	[HideInInspector]
	public Teleporter selectedTeleporter;

	// Token: 0x040001AC RID: 428
	[HideInInspector]
	public List<Teleporter> temporaryTeleporterList = new List<Teleporter>();

	// Token: 0x040001AD RID: 429
	[HideInInspector]
	public int teleporterSelectedId;

	// Token: 0x040001AE RID: 430
	public bool showAdvancedWorldgen;

	// Token: 0x040001AF RID: 431
	[HideInInspector]
	public float extraMovementSpeed;

	// Token: 0x040001B0 RID: 432
	[HideInInspector]
	public float extraDamage;

	// Token: 0x040001B1 RID: 433
	[HideInInspector]
	public int extraCritChance;

	// Token: 0x040001B2 RID: 434
	[HideInInspector]
	public float extraPickupRange;

	// Token: 0x040001B3 RID: 435
	[HideInInspector]
	public float extraDashLength;

	// Token: 0x040001B4 RID: 436
	[HideInInspector]
	public float extraBuildRange;

	// Token: 0x040001B5 RID: 437
	[HideInInspector]
	public List<Resolution> resolutions = new List<Resolution>();

	// Token: 0x040001B6 RID: 438
	[HideInInspector]
	public bool hoveringButtonInv;

	// Token: 0x040001B7 RID: 439
	[HideInInspector]
	public bool selectingButtonInv;

	// Token: 0x040001B8 RID: 440
	public List<string> loadedMaps = new List<string>();

	// Token: 0x040001B9 RID: 441
	public int mapsPage;

	// Token: 0x040001BA RID: 442
	public int mapsAmount;

	// Token: 0x040001BB RID: 443
	public string mapToPublish;

	// Token: 0x040001BC RID: 444
	public World worldToDownload = new World();

	// Token: 0x040001BD RID: 445
	public string downloadedWorldId = "0";

	// Token: 0x040001BE RID: 446
	public int worldsPage;

	// Token: 0x040001BF RID: 447
	public bool loadingMaps;

	// Token: 0x040001C0 RID: 448
	public bool enemyBlockingBuild;

	// Token: 0x040001C1 RID: 449
	public Vector2 respawnPos = Vector2.zero;

	// Token: 0x040001C2 RID: 450
	public List<string> exportedWorldsList = new List<string>();

	// Token: 0x040001C3 RID: 451
	public List<string> creativeItemIds = new List<string>();

	// Token: 0x040001C4 RID: 452
	public string[] allCreativeItemIds = new string[1];

	// Token: 0x040001C5 RID: 453
	private BulletinCollection downloadedNews = new BulletinCollection();

	// Token: 0x040001C6 RID: 454
	public int newsIndex;

	// Token: 0x040001C7 RID: 455
	public bool isInMines;

	// Token: 0x040001C8 RID: 456
	public int worldGameVersion;

	// Token: 0x040001C9 RID: 457
	public int enemiesInWorld;

	// Token: 0x040001CA RID: 458
	public bool rotativeCameraFeature;

	// Token: 0x040001CB RID: 459
	public bool nostalgiaSeed;

	// Token: 0x040001CC RID: 460
	public bool sniperSkillsSeed;

	// Token: 0x040001CD RID: 461
	public bool kyivSeed;

	// Token: 0x040001CE RID: 462
	public bool generatingWorld;

	// Token: 0x040001CF RID: 463
	public int levelGenerationProgress;

	// Token: 0x040001D0 RID: 464
	public int levelGenerationStepsNeeded = 100;

	// Token: 0x040001D1 RID: 465
	public string levelGenerationStep = "";

	// Token: 0x040001D2 RID: 466
	[HideInInspector]
	public StationBlock openedStationBlock;

	// Token: 0x040001D3 RID: 467
	public int invasionEnemiesLeft;

	// Token: 0x040001D4 RID: 468
	public Invasion invasionEvent;

	// Token: 0x040001D5 RID: 469
	public List<string> defeatBosses = new List<string>();

	// Token: 0x040001D6 RID: 470
	[HideInInspector]
	public List<ActiveStatusEffect> statusEffects = new List<ActiveStatusEffect>();

	// Token: 0x040001D7 RID: 471
	private int emoteIndex;

	// Token: 0x040001D8 RID: 472
	private float minute;

	// Token: 0x040001D9 RID: 473
	private float sunlight = 1f;

	// Token: 0x040001DA RID: 474
	private float currentSunlightVel;

	// Token: 0x040001DB RID: 475
	private float mainMenuCameraZoom = 7f;

	// Token: 0x040001DC RID: 476
	private float mainMenuZoomOutVel;

	// Token: 0x040001DD RID: 477
	private bool tempDeath;

	// Token: 0x040001DE RID: 478
	private DamageSource lastPlayerDamageSource = DamageSource.PlaceholderForImmunity;

	// Token: 0x040001DF RID: 479
	private int quantityAdded;
}
