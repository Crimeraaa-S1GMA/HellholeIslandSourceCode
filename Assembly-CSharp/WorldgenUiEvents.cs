using System;
using System.IO;
using GameJolt.API;
using GameJolt.API.Core;
using GameJolt.UI;
using TrashTake.BuildInfo;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000A1 RID: 161
public class WorldgenUiEvents : MonoBehaviour
{
	// Token: 0x06000300 RID: 768 RVA: 0x000171DC File Offset: 0x000153DC
	private void Update()
	{
		if (this.hardcoreToggle.isOn)
		{
			this.showHideAdvancedOptions.interactable = false;
			GameManager.instance.showAdvancedWorldgen = false;
		}
		else
		{
			this.showHideAdvancedOptions.interactable = true;
		}
		this.worldIcon.sprite = GameManager.instance.worldIcons[Mathf.Clamp(this.worldIconId, 0, GameManager.instance.worldIcons.Length - 1)];
		this.renameWorldIcon.sprite = GameManager.instance.worldIcons[Mathf.Clamp(this.worldIconId, 0, GameManager.instance.worldIcons.Length - 1)];
	}

	// Token: 0x06000301 RID: 769 RVA: 0x0001727C File Offset: 0x0001547C
	public void PrepareWorldCreationMenu()
	{
		this.RandomizeName();
		this.RandomizeSeed();
		this.survivalToggle.isOn = true;
		this.creativeToggle.isOn = false;
		this.hardcoreToggle.isOn = false;
		this.enemySpawnToggle.isOn = true;
		this.keepInventoryToggle.isOn = true;
		this.advancedTooltipsToggle.isOn = false;
		this.dayNightToggle.isOn = true;
		this.infiniteRangeToggle.isOn = false;
		this.allowSwitchGamemodeToggle.isOn = false;
		this.experimentalFeatureToggle.isOn = false;
		this.smallWorldToggle.isOn = false;
		this.mediumWorldToggle.isOn = true;
		this.largeWorldToggle.isOn = false;
		this.worldIconId = 0;
		GameManager.instance.showAdvancedWorldgen = false;
	}

	// Token: 0x06000302 RID: 770 RVA: 0x00017343 File Offset: 0x00015543
	public void RandomizeName()
	{
		this.worldName.text = WorldNameGenerator.instance.ReturnRandomWorldName();
	}

	// Token: 0x06000303 RID: 771 RVA: 0x0001735C File Offset: 0x0001555C
	public void RandomizeSeed()
	{
		this.worldSeed.text = GameManager.instance.ReturnRandomSeed().ToString();
	}

	// Token: 0x06000304 RID: 772 RVA: 0x00017386 File Offset: 0x00015586
	public void NextIcon()
	{
		this.worldIconId = (this.worldIconId + 1) % GameManager.instance.worldIcons.Length;
	}

	// Token: 0x06000305 RID: 773 RVA: 0x000173A4 File Offset: 0x000155A4
	public void CreateWorld()
	{
		string name = this.worldName.text.Trim();
		string newLevelGuid = GameManager.instance.GetNewLevelGuid();
		string seed = this.worldSeed.text.Trim();
		GameManager.instance.CreateWorld(name, newLevelGuid, seed, this.ReturnWorldSizeFromToggles(), this.creativeToggle.isOn, this.hardcoreToggle.isOn, this.enemySpawnToggle.isOn, this.keepInventoryToggle.isOn, this.advancedTooltipsToggle.isOn, this.dayNightToggle.isOn, this.infiniteRangeToggle.isOn, this.allowSwitchGamemodeToggle.isOn, this.experimentalFeatureToggle.isOn, this.worldIconId);
	}

	// Token: 0x06000306 RID: 774 RVA: 0x0001745C File Offset: 0x0001565C
	public void LoadWorld(string worldName)
	{
		if (GameManager.instance.gameState == GameState.MainMenu)
		{
			try
			{
				WorldManager.instance.isNewWorld = false;
				WorldManager.instance.loadedWorld = JsonUtility.FromJson<World>(PlayerPrefs.GetString(worldName));
				GameManager.instance.worldGameVersion = JsonUtility.FromJson<World>(PlayerPrefs.GetString(worldName)).versionId;
				if (JsonUtility.FromJson<World>(PlayerPrefs.GetString(worldName)).versionId > TrashTakeBuildInfo.GAME_VERSION_ID)
				{
					GameManager.instance.mainMenuSubmenu = 19;
				}
				else if (WorldManager.instance.loadedWorld.versionId < TrashTakeBuildInfo.GAME_VERSION_ID)
				{
					GameManager.instance.mainMenuSubmenu = 30;
				}
				else
				{
					this.StartWorldLoad();
				}
			}
			catch
			{
				GameManager.instance.mainMenuSubmenu = 3;
			}
		}
	}

	// Token: 0x06000307 RID: 775 RVA: 0x00017524 File Offset: 0x00015724
	public void StartWorldLoad()
	{
		if (WorldManager.instance.loadedWorld != null)
		{
			GameManager.instance.StartWorldLoad();
		}
	}

	// Token: 0x06000308 RID: 776 RVA: 0x0001753C File Offset: 0x0001573C
	public void LoadWorldSlot(int slotIndex)
	{
		if (slotIndex < GameManager.instance.availableLevels.Count)
		{
			this.LoadWorld(GameManager.instance.availableLevels[slotIndex]);
		}
	}

	// Token: 0x06000309 RID: 777 RVA: 0x00017568 File Offset: 0x00015768
	public void DeleteWorldScreen(int worldIndex)
	{
		if (PlayerPrefs.HasKey(GameManager.instance.availableLevels[worldIndex]))
		{
			WorldManager.instance.worldIdToDelete = worldIndex;
			WorldManager.instance.worldToDelete = JsonUtility.FromJson<World>(PlayerPrefs.GetString(GameManager.instance.availableLevels[worldIndex]));
			GameManager.instance.mainMenuSubmenu = 6;
		}
	}

	// Token: 0x0600030A RID: 778 RVA: 0x000175C8 File Offset: 0x000157C8
	public void DeleteWorld()
	{
		GameManager.instance.uncopiableLevels.Remove(GameManager.instance.availableLevels[WorldManager.instance.worldIdToDelete]);
		PlayerPrefs.DeleteKey(GameManager.instance.availableLevels[WorldManager.instance.worldIdToDelete]);
		GameManager.instance.availableLevels.RemoveAt(WorldManager.instance.worldIdToDelete);
		GameManager.instance.SaveAvailableLevels();
	}

	// Token: 0x0600030B RID: 779 RVA: 0x00017640 File Offset: 0x00015840
	public void ExportWorld()
	{
		if (PlayerPrefs.HasKey(GameManager.instance.availableLevels[WorldManager.instance.worldIdToDelete]))
		{
			string @string = PlayerPrefs.GetString(GameManager.instance.availableLevels[WorldManager.instance.worldIdToDelete]);
			if (Directory.Exists(Application.persistentDataPath + "/ExportedWorlds/"))
			{
				File.WriteAllText(Application.persistentDataPath + "/ExportedWorlds/" + GameManager.instance.availableLevels[WorldManager.instance.worldIdToDelete] + ".ttwld", @string);
				return;
			}
			Directory.CreateDirectory(Application.persistentDataPath + "/ExportedWorlds/");
			File.WriteAllText(Application.persistentDataPath + "/ExportedWorlds/" + GameManager.instance.availableLevels[WorldManager.instance.worldIdToDelete] + ".ttwld", @string);
		}
	}

	// Token: 0x0600030C RID: 780 RVA: 0x00017724 File Offset: 0x00015924
	public void UploadWorld()
	{
		if (MonoSingleton<GameJoltAPI>.Instance.CurrentUser == null)
		{
			GameManager.instance.mainMenuSubmenu = 26;
			return;
		}
		if (PlayerPrefs.HasKey(GameManager.instance.availableLevels[WorldManager.instance.worldIdToDelete]) && !GameManager.instance.uncopiableLevels.Contains(GameManager.instance.availableLevels[WorldManager.instance.worldIdToDelete]))
		{
			GameManager.instance.mapToPublish = PlayerPrefs.GetString(GameManager.instance.availableLevels[WorldManager.instance.worldIdToDelete]);
			GameManager.instance.mainMenuSubmenu = 14;
			return;
		}
		GameManager.instance.mainMenuSubmenu = 16;
	}

	// Token: 0x0600030D RID: 781 RVA: 0x000177D8 File Offset: 0x000159D8
	public void UploadWorldToServer()
	{
		if (MonoSingleton<GameJoltAPI>.Instance.CurrentUser != null)
		{
			string id = "@" + UtilityMath.GetEpochTime().ToString() + this.ReturnUniqueLevelId(20);
			World world = JsonUtility.FromJson<World>(GameManager.instance.mapToPublish);
			world.mapDescription = this.mapDesc.text.Trim();
			world.mapAuthor = MonoSingleton<GameJoltAPI>.Instance.CurrentUser.Name;
			string map = JsonUtility.ToJson(world);
			GameManager.instance.mainMenuSubmenu = 17;
			Action<bool> <>9__1;
			DataStore.Get(id, true, delegate(string c)
			{
				if (c == null)
				{
					string id = id;
					string map = map;
					bool global = true;
					Action<bool> callback;
					if ((callback = <>9__1) == null)
					{
						callback = (<>9__1 = delegate(bool c)
						{
							if (c)
							{
								this.mapDesc.text = string.Empty;
								AchievementManager.instance.AddAchievement(156199);
								GameManager.instance.mainMenuSubmenu = 10;
								return;
							}
							Debug.LogWarning("Upload Failed");
							DataStore.Delete(id, true, null);
							GameManager.instance.mainMenuSubmenu = 16;
						});
					}
					DataStore.SetSegmented(id, map, global, callback, null, 1048565);
					return;
				}
				Debug.LogWarning("Upload Failed");
				GameManager.instance.mainMenuSubmenu = 16;
			});
			return;
		}
		MonoSingleton<GameJoltUI>.Instance.ShowSignIn();
	}

	// Token: 0x0600030E RID: 782 RVA: 0x0001789C File Offset: 0x00015A9C
	private string ReturnUniqueLevelId(int length)
	{
		string text = "";
		for (int i = 0; i < length; i++)
		{
			text += Random.Range(0, 10).ToString();
		}
		return text;
	}

	// Token: 0x0600030F RID: 783 RVA: 0x000178D4 File Offset: 0x00015AD4
	public void DownloadMap()
	{
		World worldToDownload = GameManager.instance.worldToDownload;
		if (worldToDownload != null && !GameManager.instance.ReturnAvailableLevelNames().Contains(worldToDownload.name) && !GameManager.instance.availableLevels.Contains(worldToDownload.storageName) && worldToDownload.name != "AvailableLevels" && worldToDownload.name != "SETTINGS")
		{
			if (worldToDownload.storageName != null)
			{
				PlayerPrefs.SetString(worldToDownload.storageName, JsonUtility.ToJson(worldToDownload));
				GameManager.instance.availableLevels.Add(worldToDownload.storageName);
				GameManager.instance.uncopiableLevels.Add(worldToDownload.storageName);
			}
			else
			{
				PlayerPrefs.SetString(worldToDownload.name, JsonUtility.ToJson(worldToDownload));
				GameManager.instance.availableLevels.Add(worldToDownload.name);
				GameManager.instance.uncopiableLevels.Add(worldToDownload.name);
			}
			GameManager.instance.SaveAvailableLevels();
			GameManager.instance.mainMenuSubmenu = 12;
			return;
		}
		GameManager.instance.mainMenuSubmenu = 13;
	}

	// Token: 0x06000310 RID: 784 RVA: 0x000179F3 File Offset: 0x00015BF3
	public void SwitchAdvancedWorldgen()
	{
		GameManager.instance.showAdvancedWorldgen = !GameManager.instance.showAdvancedWorldgen;
	}

	// Token: 0x06000311 RID: 785 RVA: 0x00017A0C File Offset: 0x00015C0C
	public void OpenRenameWorld()
	{
		this.worldIconId = WorldManager.instance.worldToDelete.worldIconId;
		this.renameWorld.text = WorldManager.instance.worldToDelete.name;
		GameManager.instance.mainMenuSubmenu = 29;
	}

	// Token: 0x06000312 RID: 786 RVA: 0x00017A4C File Offset: 0x00015C4C
	public void RenameWorld()
	{
		if (this.renameWorld.text.Length > 0)
		{
			if (WorldManager.instance.worldToDelete.storageName == null)
			{
				WorldManager.instance.worldToDelete.storageName = WorldManager.instance.worldToDelete.name;
			}
			WorldManager.instance.worldToDelete.name = this.renameWorld.text;
			WorldManager.instance.worldToDelete.worldIconId = this.worldIconId;
			PlayerPrefs.SetString(WorldManager.instance.worldToDelete.storageName, JsonUtility.ToJson(WorldManager.instance.worldToDelete));
			GameManager.instance.mainMenuSubmenu = 2;
		}
	}

	// Token: 0x06000313 RID: 787 RVA: 0x00017AFB File Offset: 0x00015CFB
	private float ReturnWorldSizeFromToggles()
	{
		if (this.largeWorldToggle.isOn)
		{
			return 1f;
		}
		if (this.mediumWorldToggle.isOn)
		{
			return 0.7f;
		}
		return 0.5f;
	}

	// Token: 0x040003C1 RID: 961
	[SerializeField]
	private InputField worldName;

	// Token: 0x040003C2 RID: 962
	[SerializeField]
	private Image worldIcon;

	// Token: 0x040003C3 RID: 963
	[SerializeField]
	private InputField worldSeed;

	// Token: 0x040003C4 RID: 964
	[SerializeField]
	private Toggle survivalToggle;

	// Token: 0x040003C5 RID: 965
	[SerializeField]
	private Toggle creativeToggle;

	// Token: 0x040003C6 RID: 966
	[SerializeField]
	private Toggle hardcoreToggle;

	// Token: 0x040003C7 RID: 967
	[SerializeField]
	private Toggle enemySpawnToggle;

	// Token: 0x040003C8 RID: 968
	[SerializeField]
	private Toggle keepInventoryToggle;

	// Token: 0x040003C9 RID: 969
	[SerializeField]
	private Toggle advancedTooltipsToggle;

	// Token: 0x040003CA RID: 970
	[SerializeField]
	private Toggle infiniteRangeToggle;

	// Token: 0x040003CB RID: 971
	[SerializeField]
	private Toggle allowSwitchGamemodeToggle;

	// Token: 0x040003CC RID: 972
	[SerializeField]
	private Toggle experimentalFeatureToggle;

	// Token: 0x040003CD RID: 973
	[SerializeField]
	private Toggle dayNightToggle;

	// Token: 0x040003CE RID: 974
	[SerializeField]
	private Toggle smallWorldToggle;

	// Token: 0x040003CF RID: 975
	[SerializeField]
	private Toggle mediumWorldToggle;

	// Token: 0x040003D0 RID: 976
	[SerializeField]
	private Toggle largeWorldToggle;

	// Token: 0x040003D1 RID: 977
	[SerializeField]
	private InputField renameWorld;

	// Token: 0x040003D2 RID: 978
	[SerializeField]
	private Image renameWorldIcon;

	// Token: 0x040003D3 RID: 979
	[SerializeField]
	private InputField mapDesc;

	// Token: 0x040003D4 RID: 980
	[SerializeField]
	private Button showHideAdvancedOptions;

	// Token: 0x040003D5 RID: 981
	private int worldIconId;
}
