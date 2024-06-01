using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200004E RID: 78
public class GuiManager : MonoBehaviour
{
	// Token: 0x06000190 RID: 400 RVA: 0x0000D48C File Offset: 0x0000B68C
	private void Update()
	{
		this.exitAndSave.SetActive(GameManager.instance.fullInventoryOpen && !GameManager.instance.displayAutosave);
		GameObject[] array = this.mainSlots;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(!GameManager.instance.generatingWorld);
		}
		array = this.fullSlots;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(GameManager.instance.fullInventoryOpen && !GameManager.instance.generatingWorld);
		}
		this.statusEffects.SetActive(!GameManager.instance.fullInventoryOpen && !GameManager.instance.generatingWorld);
		this.draggedSlot.SetActive(GameManager.instance.draggingSlot);
		this.creative.SetActive(GameManager.instance.fullInventoryOpen && WorldPolicy.creativeMode && GameManager.instance.station == CraftingStation.ByHand);
		this.crafting.SetActive(GameManager.instance.fullInventoryOpen && (!WorldPolicy.creativeMode || GameManager.instance.station > CraftingStation.ByHand));
		this.chestInv.SetActive(GameManager.instance.fullInventoryOpen && GameManager.instance.openedChest != null);
		this.deathScreen.SetActive(GameManager.instance.playerMovement == null && !WorldPolicy.hardcoreMode && !GameManager.instance.generatingWorld);
		this.hardcoreDeathScreen.SetActive(GameManager.instance.playerMovement == null && WorldPolicy.hardcoreMode && !GameManager.instance.generatingWorld);
		this.healthBar.SetActive(!WorldPolicy.creativeMode && !GameManager.instance.generatingWorld);
		this.autosave.SetActive(GameManager.instance.displayAutosave);
		this.signEdition.SetActive(GameManager.instance.sign != null);
		this.npcShop.SetActive(GameManager.instance.npcShop != null);
		this.teleporterButtons.SetActive(GameManager.instance.selectedTeleporter != null);
		this.craftButton.interactable = GameManager.instance.CanCraftItem(GameManager.instance.selectedRecipe);
		this.craftButtonObject.SetActive(GameManager.instance.selectedRecipe != -1);
		this.compassCoordinates.SetActive(GameManager.instance.EquippedAccesory("compass") && GameManager.instance.fullInventoryOpen);
		this.switchGamemodeButton.SetActive(WorldPolicy.allowSwitchingGamemodes && GameManager.instance.fullInventoryOpen);
		this.creativeSearchResetButton.SetActive(GameManager.instance.creativeModeSearch != string.Empty);
		this.bossBar.SetActive(GameManager.instance.boss != null && GameManager.instance.bossHealthBars);
		this.invasionBar.SetActive(GameManager.instance.invasionEvent != Invasion.None && GameManager.instance.boss == null);
		this.generationProgress.SetActive(GameManager.instance.generatingWorld);
		if (GameManager.instance.playerMovement != null)
		{
			this.emoteName.SetActive(Input.GetKey(KeyCode.Z) && GameManager.instance.sign == null && GameManager.instance.bed == null && GameManager.instance.chair == null && GameManager.instance.openedChest == null && !GameManager.instance.generatingWorld && GameManager.instance.playerMovement.vampire == null);
		}
		else
		{
			this.emoteName.SetActive(false);
		}
		this.emoteIcon.sprite = GameManager.instance.ReturnCurrentEmote().emoteIcon;
	}

	// Token: 0x06000191 RID: 401 RVA: 0x0000D89F File Offset: 0x0000BA9F
	public IEnumerator PlayDayDisplayAnimation()
	{
		this.dayDisplay.gameObject.SetActive(true);
		this.dayDisplay.Play("In");
		yield return new WaitForSeconds(3f);
		this.dayDisplay.Play("Out");
		yield return new WaitForSeconds(2f);
		this.dayDisplay.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x040001FE RID: 510
	[SerializeField]
	private GameObject draggedSlot;

	// Token: 0x040001FF RID: 511
	[SerializeField]
	private GameObject[] fullSlots;

	// Token: 0x04000200 RID: 512
	[SerializeField]
	private GameObject[] mainSlots;

	// Token: 0x04000201 RID: 513
	[SerializeField]
	private GameObject statusEffects;

	// Token: 0x04000202 RID: 514
	[SerializeField]
	private GameObject creative;

	// Token: 0x04000203 RID: 515
	[SerializeField]
	private GameObject crafting;

	// Token: 0x04000204 RID: 516
	[SerializeField]
	private GameObject chestInv;

	// Token: 0x04000205 RID: 517
	[SerializeField]
	private GameObject deathScreen;

	// Token: 0x04000206 RID: 518
	[SerializeField]
	private GameObject hardcoreDeathScreen;

	// Token: 0x04000207 RID: 519
	[SerializeField]
	private GameObject healthBar;

	// Token: 0x04000208 RID: 520
	[SerializeField]
	private GameObject autosave;

	// Token: 0x04000209 RID: 521
	[SerializeField]
	private GameObject signEdition;

	// Token: 0x0400020A RID: 522
	[SerializeField]
	private GameObject npcShop;

	// Token: 0x0400020B RID: 523
	[SerializeField]
	private GameObject teleporterButtons;

	// Token: 0x0400020C RID: 524
	[SerializeField]
	private GameObject exitAndSave;

	// Token: 0x0400020D RID: 525
	[SerializeField]
	private Button craftButton;

	// Token: 0x0400020E RID: 526
	[SerializeField]
	private GameObject craftButtonObject;

	// Token: 0x0400020F RID: 527
	[SerializeField]
	private GameObject compassCoordinates;

	// Token: 0x04000210 RID: 528
	[SerializeField]
	private GameObject switchGamemodeButton;

	// Token: 0x04000211 RID: 529
	[SerializeField]
	private GameObject creativeSearchResetButton;

	// Token: 0x04000212 RID: 530
	[SerializeField]
	private GameObject bossBar;

	// Token: 0x04000213 RID: 531
	[SerializeField]
	private GameObject invasionBar;

	// Token: 0x04000214 RID: 532
	[SerializeField]
	private GameObject generationProgress;

	// Token: 0x04000215 RID: 533
	[SerializeField]
	private GameObject emoteName;

	// Token: 0x04000216 RID: 534
	[SerializeField]
	private Image emoteIcon;

	// Token: 0x04000217 RID: 535
	[SerializeField]
	private Animator dayDisplay;
}
