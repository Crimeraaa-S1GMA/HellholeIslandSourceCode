using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200009E RID: 158
public class InventoryUiEvents : MonoBehaviour
{
	// Token: 0x060002BA RID: 698 RVA: 0x000164EE File Offset: 0x000146EE
	private void Start()
	{
		this.UpdateCreativeSearchText(this.creativeSearch);
		this.creativeSearch.onValueChanged.AddListener(delegate(string <p0>)
		{
			this.UpdateCreativeSearchText(this.creativeSearch);
		});
	}

	// Token: 0x060002BB RID: 699 RVA: 0x00016518 File Offset: 0x00014718
	public void UpdateCreativeSearchText(InputField field)
	{
		GameManager.instance.creativeModeSearch = field.text;
		GameManager.instance.OpenCreativeModeTab(GameManager.instance.creativeMenuTab, true);
	}

	// Token: 0x060002BC RID: 700 RVA: 0x0001653F File Offset: 0x0001473F
	public void ResetCreativeSearch()
	{
		this.creativeSearch.text = string.Empty;
		this.UpdateCreativeSearchText(this.creativeSearch);
	}

	// Token: 0x060002BD RID: 701 RVA: 0x0001655D File Offset: 0x0001475D
	public void CreativeInventoryTab(int tab)
	{
		GameManager.instance.creativeTab = Mathf.Clamp(GameManager.instance.creativeTab + tab, 0, GameManager.instance.ReturnMaxCreativePage());
	}

	// Token: 0x060002BE RID: 702 RVA: 0x00016585 File Offset: 0x00014785
	public void RecipeTab(int tab)
	{
		GameManager.instance.recipeTab = Mathf.Clamp(GameManager.instance.recipeTab + tab, 0, GameManager.instance.ReturnMaxRecipePage());
	}

	// Token: 0x060002BF RID: 703 RVA: 0x000165AD File Offset: 0x000147AD
	public void Craft()
	{
		GameManager.instance.CraftItem(GameManager.instance.selectedRecipe);
	}

	// Token: 0x060002C0 RID: 704 RVA: 0x000165C3 File Offset: 0x000147C3
	public void ReturnToMenu()
	{
		GameManager.instance.ExitToMainMenu(true);
	}

	// Token: 0x060002C1 RID: 705 RVA: 0x000165D0 File Offset: 0x000147D0
	public void SaveSignContent()
	{
		GameManager.instance.sign.SaveContent(this.signContentField.text.Trim());
	}

	// Token: 0x060002C2 RID: 706 RVA: 0x000165F1 File Offset: 0x000147F1
	public void LoadSignContent()
	{
		this.signContentField.text = GameManager.instance.sign.ReturnBlockComp().blockMetadata[0].Trim();
	}

	// Token: 0x060002C3 RID: 707 RVA: 0x0001661D File Offset: 0x0001481D
	public void SaveChestName()
	{
		GameManager.instance.openedChest.placedBlock.blockMetadata[0] = this.chestNameField.text.Trim();
	}

	// Token: 0x060002C4 RID: 708 RVA: 0x00016649 File Offset: 0x00014849
	public void LoadChestName()
	{
		this.chestNameField.text = GameManager.instance.openedChest.placedBlock.blockMetadata[0].Trim();
	}

	// Token: 0x060002C5 RID: 709 RVA: 0x00016675 File Offset: 0x00014875
	public void LootAll()
	{
		GameManager.instance.LootChest();
	}

	// Token: 0x060002C6 RID: 710 RVA: 0x00016681 File Offset: 0x00014881
	public void LockChest()
	{
		GameManager.instance.openedChest.placedBlock.blockMetadata[2] = "locked";
		GameManager.instance.SaveChestSlots();
		GameManager.instance.openedChest = null;
	}

	// Token: 0x060002C7 RID: 711 RVA: 0x000166B7 File Offset: 0x000148B7
	public void UnloadSign()
	{
		GameManager.instance.sign = null;
	}

	// Token: 0x060002C8 RID: 712 RVA: 0x000166C4 File Offset: 0x000148C4
	public void OpenCreativeTab(int tab)
	{
		GameManager.instance.OpenCreativeModeTab((CreativeMenuTab)tab, false);
	}

	// Token: 0x060002C9 RID: 713 RVA: 0x000166D4 File Offset: 0x000148D4
	public void Weather(int id)
	{
		switch (id)
		{
		case 0:
			GameManager.instance.fullTime = (float)(1440 * (GameManager.instance.ReturnDay() - 1) + 800);
			return;
		case 1:
			GameManager.instance.fullTime = (float)(1440 * (GameManager.instance.ReturnDay() - 1) + 1260);
			return;
		case 2:
			GameManager.instance.rain = false;
			return;
		case 3:
			GameManager.instance.rain = true;
			return;
		default:
			return;
		}
	}

	// Token: 0x060002CA RID: 714 RVA: 0x00016757 File Offset: 0x00014957
	public void OpenCloseInv()
	{
		GameManager.instance.OpenCloseInventory();
	}

	// Token: 0x060002CB RID: 715 RVA: 0x00016763 File Offset: 0x00014963
	public void SwitchGamemode()
	{
		if (WorldPolicy.allowSwitchingGamemodes)
		{
			WorldPolicy.creativeMode = !WorldPolicy.creativeMode;
		}
	}

	// Token: 0x060002CC RID: 716 RVA: 0x0001677C File Offset: 0x0001497C
	public void TeleporterButton(int id)
	{
		switch (id)
		{
		case 0:
			GameManager.instance.selectedTeleporter = null;
			GameManager.instance.temporaryTeleporterList.Clear();
			GameManager.instance.teleporterSelectedId = 0;
			return;
		case 1:
			GameManager.instance.teleporterSelectedId = Mathf.Clamp(GameManager.instance.teleporterSelectedId - 1, 0, GameManager.instance.temporaryTeleporterList.Count - 1);
			GameManager.instance.selectedTeleporter = GameManager.instance.temporaryTeleporterList[GameManager.instance.teleporterSelectedId];
			return;
		case 2:
			GameManager.instance.teleporterSelectedId = Mathf.Clamp(GameManager.instance.teleporterSelectedId + 1, 0, GameManager.instance.temporaryTeleporterList.Count - 1);
			GameManager.instance.selectedTeleporter = GameManager.instance.temporaryTeleporterList[GameManager.instance.teleporterSelectedId];
			return;
		default:
			return;
		}
	}

	// Token: 0x060002CD RID: 717 RVA: 0x00016863 File Offset: 0x00014A63
	public void Respawn()
	{
		GameManager.instance.Respawn();
	}

	// Token: 0x040003B6 RID: 950
	[SerializeField]
	private InputField signContentField;

	// Token: 0x040003B7 RID: 951
	[SerializeField]
	private InputField chestNameField;

	// Token: 0x040003B8 RID: 952
	[SerializeField]
	private InputField creativeSearch;
}
