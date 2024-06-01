using System;
using System.Collections;
using GameJolt.API;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000065 RID: 101
public class MapSlot : MonoBehaviour
{
	// Token: 0x060001D3 RID: 467 RVA: 0x00010235 File Offset: 0x0000E435
	private void Start()
	{
		this.button = base.GetComponent<Button>();
	}

	// Token: 0x060001D4 RID: 468 RVA: 0x00010243 File Offset: 0x0000E443
	public void ClearWorld()
	{
		this.world = null;
	}

	// Token: 0x060001D5 RID: 469 RVA: 0x0001024C File Offset: 0x0000E44C
	private void Update()
	{
		if (this.index < GameManager.instance.loadedMaps.Count && !GameManager.instance.loadingMaps)
		{
			if (this.world != null)
			{
				this.worldNameTarget.text = string.Concat(new string[]
				{
					"<size=30>",
					this.world.name,
					"</size>\n<size=22>By ",
					this.world.mapAuthor,
					"</size>"
				});
			}
			else
			{
				this.worldNameTarget.text = "Fetching world data...";
			}
			this.button.interactable = true;
			return;
		}
		this.worldNameTarget.text = "";
		this.button.interactable = false;
	}

	// Token: 0x060001D6 RID: 470 RVA: 0x00010309 File Offset: 0x0000E509
	public bool IsSlotOccupied()
	{
		return this.index < GameManager.instance.loadedMaps.Count;
	}

	// Token: 0x060001D7 RID: 471 RVA: 0x00010322 File Offset: 0x0000E522
	public bool IsWorldNull()
	{
		return this.world == null;
	}

	// Token: 0x060001D8 RID: 472 RVA: 0x0001032D File Offset: 0x0000E52D
	public void UpdateLevelData()
	{
		base.StartCoroutine("UpdateLevelDataAsync");
	}

	// Token: 0x060001D9 RID: 473 RVA: 0x0001033B File Offset: 0x0000E53B
	private IEnumerator UpdateLevelDataAsync()
	{
		yield return null;
		if (this.index < GameManager.instance.loadedMaps.Count)
		{
			this.ClearWorld();
			this.Download();
		}
		yield break;
	}

	// Token: 0x060001DA RID: 474 RVA: 0x0001034A File Offset: 0x0000E54A
	private void Download()
	{
		base.StartCoroutine("DownloadAsync");
	}

	// Token: 0x060001DB RID: 475 RVA: 0x00010358 File Offset: 0x0000E558
	private IEnumerator DownloadAsync()
	{
		yield return null;
		DataStore.Get(GameManager.instance.loadedMaps[this.index], true, delegate(string c)
		{
			if (c != null)
			{
				this.world = JsonUtility.FromJson<World>(c);
				return;
			}
			this.Download();
		});
		yield break;
	}

	// Token: 0x060001DC RID: 476 RVA: 0x00010368 File Offset: 0x0000E568
	public void DownloadLevel()
	{
		if (this.world != null)
		{
			GameManager.instance.worldToDownload = this.world;
			GameManager.instance.downloadedWorldId = GameManager.instance.loadedMaps[this.index];
			GameManager.instance.mainMenuSubmenu = 15;
		}
	}

	// Token: 0x040002BC RID: 700
	public Text worldNameTarget;

	// Token: 0x040002BD RID: 701
	public int index;

	// Token: 0x040002BE RID: 702
	private Button button;

	// Token: 0x040002BF RID: 703
	private World world;
}
