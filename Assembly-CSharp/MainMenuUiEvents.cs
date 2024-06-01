using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameJolt.API;
using GameJolt.API.Core;
using GameJolt.API.Objects;
using GameJolt.UI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200009F RID: 159
public class MainMenuUiEvents : MonoBehaviour
{
	// Token: 0x060002D0 RID: 720 RVA: 0x00016885 File Offset: 0x00014A85
	private void Start()
	{
		this.slots = Object.FindObjectsOfType<MapSlot>(true);
		this.mapsSortDropdown.onValueChanged.AddListener(delegate(int <p0>)
		{
			this.ReloadMapsAfterSort(this.mapsSortDropdown);
		});
	}

	// Token: 0x060002D1 RID: 721 RVA: 0x000168AF File Offset: 0x00014AAF
	public void OpenSignIn()
	{
		if (MonoSingleton<GameJoltAPI>.Instance.CurrentUser == null)
		{
			GameManager.instance.mainMenuSubmenu = 26;
			return;
		}
		GameManager.instance.mainMenuSubmenu = 23;
	}

	// Token: 0x060002D2 RID: 722 RVA: 0x000168D8 File Offset: 0x00014AD8
	public void LinkAccount()
	{
		GameManager.instance.accountName = MonoSingleton<GameJoltAPI>.Instance.CurrentUser.Name;
		GameManager.instance.gameToken = MonoSingleton<GameJoltAPI>.Instance.CurrentUser.Token;
		GameManager.instance.isLinked = true;
		GameManager.instance.SaveSettings();
		GameManager.instance.mainMenuSubmenu = 24;
	}

	// Token: 0x060002D3 RID: 723 RVA: 0x00016938 File Offset: 0x00014B38
	public void SignOut()
	{
		GameManager.instance.accountName = "";
		GameManager.instance.gameToken = "";
		GameManager.instance.isLinked = false;
		GameManager.instance.SaveSettings();
		MonoSingleton<GameJoltAPI>.Instance.CurrentUser.SignOut();
		GameManager.instance.mainMenuSubmenu = 0;
	}

	// Token: 0x060002D4 RID: 724 RVA: 0x00016992 File Offset: 0x00014B92
	public void OpenTrophies()
	{
		if (MonoSingleton<GameJoltAPI>.Instance.CurrentUser != null)
		{
			MonoSingleton<GameJoltUI>.Instance.ShowTrophies();
			return;
		}
		GameManager.instance.mainMenuSubmenu = 26;
	}

	// Token: 0x060002D5 RID: 725 RVA: 0x000169B7 File Offset: 0x00014BB7
	public void News()
	{
		GameManager.instance.DownloadNews();
	}

	// Token: 0x060002D6 RID: 726 RVA: 0x000169C4 File Offset: 0x00014BC4
	public void NewsLink()
	{
		if (GameManager.instance.GetNews(GameManager.instance.newsIndex).link != string.Empty)
		{
			Application.OpenURL(GameManager.instance.GetNews(GameManager.instance.newsIndex).link);
		}
	}

	// Token: 0x060002D7 RID: 727 RVA: 0x00016A14 File Offset: 0x00014C14
	public void ReloadMapsAfterSort(Dropdown change)
	{
		this.Maps();
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x00016A1C File Offset: 0x00014C1C
	public void Maps()
	{
		GameManager.instance.mapsPage = 0;
		GameManager.instance.loadingMaps = true;
		this.LoadMaps();
	}

	// Token: 0x060002D9 RID: 729 RVA: 0x00016A3A File Offset: 0x00014C3A
	public void OpenStructureBuilder()
	{
		GameManager.instance.OpenStructureBuilder();
	}

	// Token: 0x060002DA RID: 730 RVA: 0x00016A48 File Offset: 0x00014C48
	private void LoadMaps()
	{
		MapSlot[] array = this.slots;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].ClearWorld();
		}
		base.StopCoroutine("DownloadMapData");
		GameManager.instance.loadedMaps.Clear();
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			GameManager.instance.mainMenuSubmenu = 18;
			return;
		}
		DataStore.GetKeys(true, delegate(string[] c)
		{
			List<string> list = c.ToList<string>();
			if (this.mapsSortDropdown.value == 0)
			{
				list.Reverse();
			}
			GameManager.instance.mapsAmount = c.Length;
			for (int j = Mathf.Clamp(GameManager.instance.mapsPage * 20, 0, list.Count); j < Mathf.Clamp(20 + GameManager.instance.mapsPage * 20, 0, list.Count); j++)
			{
				GameManager.instance.loadedMaps.Add(list[j]);
			}
			GameManager.instance.loadedMaps.Remove("BR1");
			GameManager.instance.loadedMaps.Remove("BR2");
			base.StartCoroutine("DownloadMapData");
		});
	}

	// Token: 0x060002DB RID: 731 RVA: 0x00016AB2 File Offset: 0x00014CB2
	private IEnumerator DownloadMapData()
	{
		GameManager.instance.loadingMaps = true;
		MapSlot[] array = this.slots;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].UpdateLevelData();
		}
		while (Array.Find<MapSlot>(this.slots, (MapSlot s) => this.CheckLoadingConditions(s)) != null)
		{
			yield return null;
		}
		GameManager.instance.loadingMaps = false;
		yield break;
	}

	// Token: 0x060002DC RID: 732 RVA: 0x00016AC1 File Offset: 0x00014CC1
	private bool CheckLoadingConditions(MapSlot slot)
	{
		return slot.IsWorldNull() && slot.IsSlotOccupied();
	}

	// Token: 0x060002DD RID: 733 RVA: 0x00016AD3 File Offset: 0x00014CD3
	public void NewsPage(int pageIncrement)
	{
		GameManager.instance.newsIndex = Mathf.Clamp(GameManager.instance.newsIndex + pageIncrement, 0, GameManager.instance.GetDownloadedNewsCount() - 1);
	}

	// Token: 0x060002DE RID: 734 RVA: 0x00016B00 File Offset: 0x00014D00
	public void MapPage(int pageIncrement)
	{
		GameManager.instance.loadingMaps = true;
		GameManager.instance.mapsPage += pageIncrement;
		GameManager.instance.mapsPage = Mathf.Clamp(GameManager.instance.mapsPage, 0, Mathf.FloorToInt((float)(GameManager.instance.mapsAmount - 1) / 20f));
		this.LoadMaps();
	}

	// Token: 0x060002DF RID: 735 RVA: 0x00016B64 File Offset: 0x00014D64
	public void WorldPage(int pageIncrement)
	{
		if (GameManager.instance.availableLevels.Count > 0)
		{
			GameManager.instance.worldsPage += pageIncrement;
			GameManager.instance.worldsPage = Mathf.Clamp(GameManager.instance.worldsPage, 0, Mathf.FloorToInt((float)(GameManager.instance.availableLevels.Count - 1) / 20f));
		}
	}

	// Token: 0x060002E0 RID: 736 RVA: 0x00016BCC File Offset: 0x00014DCC
	public void ImportWorlds()
	{
		GameManager.instance.LoadExportedWorldDirectiories();
		GameManager.instance.ImportExportedWorlds();
	}

	// Token: 0x060002E1 RID: 737 RVA: 0x00016BE2 File Offset: 0x00014DE2
	public void SetSubmenu(int submenu)
	{
		GameManager.instance.mainMenuSubmenu = submenu;
	}

	// Token: 0x060002E2 RID: 738 RVA: 0x00016BEF File Offset: 0x00014DEF
	public void ExitGame()
	{
		Application.Quit();
	}

	// Token: 0x060002E3 RID: 739 RVA: 0x00016BF6 File Offset: 0x00014DF6
	public void OpenLink(string url)
	{
		Application.OpenURL(url);
	}

	// Token: 0x060002E4 RID: 740 RVA: 0x00016BFE File Offset: 0x00014DFE
	public void SignIn()
	{
		GameManager.instance.mainMenuSubmenu = 25;
		new User(this.username.text, this.token.text).SignIn(delegate(bool signInSuccess)
		{
			Debug.Log(string.Format("Sign-in {0}", signInSuccess ? "successful" : "failed"));
			if (signInSuccess)
			{
				GameManager.instance.mainMenuSubmenu = 0;
				if (this.rememberMeToggle.isOn)
				{
					GameManager.instance.accountName = MonoSingleton<GameJoltAPI>.Instance.CurrentUser.Name;
					GameManager.instance.gameToken = MonoSingleton<GameJoltAPI>.Instance.CurrentUser.Token;
					GameManager.instance.isLinked = true;
					GameManager.instance.SaveSettings();
				}
				this.username.text = string.Empty;
				this.token.text = string.Empty;
				return;
			}
			GameManager.instance.mainMenuSubmenu = 27;
		}, null, false);
	}

	// Token: 0x040003B9 RID: 953
	[SerializeField]
	private InputField username;

	// Token: 0x040003BA RID: 954
	[SerializeField]
	private InputField token;

	// Token: 0x040003BB RID: 955
	[SerializeField]
	private Toggle rememberMeToggle;

	// Token: 0x040003BC RID: 956
	[SerializeField]
	private Dropdown mapsSortDropdown;

	// Token: 0x040003BD RID: 957
	private MapSlot[] slots;
}
