using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000081 RID: 129
public class RecipeSlot : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x06000252 RID: 594 RVA: 0x00013628 File Offset: 0x00011828
	private void Update()
	{
		if (GameManager.instance.currentRecipes.Count > 0)
		{
			this.displayItem.sprite = GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(GameManager.instance.currentRecipes[Mathf.Clamp(this.itemIndex + GameManager.instance.recipeTab * 18, 0, GameManager.instance.currentRecipes.Count - 1)].result.COMP_ID).sprite;
			this.displayItem.enabled = (this.itemIndex + GameManager.instance.recipeTab * 18 < GameManager.instance.currentRecipes.Count);
			if (this.itemIndex + GameManager.instance.recipeTab * 18 < GameManager.instance.currentRecipes.Count)
			{
				if (this.IsInSelectedStation() && GameManager.instance.CanCraftItem(this.itemIndex + GameManager.instance.recipeTab * 18))
				{
					this.displayItem.color = this.enabledColor;
					return;
				}
				this.displayItem.color = this.disabledColor;
			}
		}
	}

	// Token: 0x06000253 RID: 595 RVA: 0x00013750 File Offset: 0x00011950
	public void SelectRecipe()
	{
		if (this.itemIndex + GameManager.instance.recipeTab * 18 < GameManager.instance.currentRecipes.Count && this.IsInSelectedStation())
		{
			GameManager.instance.selectedRecipe = this.itemIndex + GameManager.instance.recipeTab * 18;
		}
	}

	// Token: 0x06000254 RID: 596 RVA: 0x000137A8 File Offset: 0x000119A8
	public void OnPointerEnter(PointerEventData pointerEventData)
	{
		GameManager.instance.hoveringButtonInv = true;
		GameManager.instance.selectingButtonInv = false;
		if (this.itemIndex + GameManager.instance.recipeTab * 18 < GameManager.instance.currentRecipes.Count && this.IsInSelectedStation())
		{
			GameManager.instance.tooltipCustom = "<size=30>" + GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(GameManager.instance.currentRecipes[Mathf.Clamp(this.itemIndex + GameManager.instance.recipeTab * 18, 0, GameManager.instance.currentRecipes.Count - 1)].result.COMP_ID).name + "</size>";
		}
	}

	// Token: 0x06000255 RID: 597 RVA: 0x00013869 File Offset: 0x00011A69
	public void OnPointerExit(PointerEventData pointerEventData)
	{
		GameManager.instance.hoveringButtonInv = false;
		GameManager.instance.selectingButtonInv = false;
		GameManager.instance.tooltipCustom = "";
	}

	// Token: 0x06000256 RID: 598 RVA: 0x00013890 File Offset: 0x00011A90
	private bool IsInSelectedStation()
	{
		return GameManager.instance.station == this.ReturnMyCraftingStation() || this.ReturnMyCraftingStation() == CraftingStation.ByHand || (GameManager.instance.station == CraftingStation.ChromiumAnvil && this.ReturnMyCraftingStation() == CraftingStation.Anvil) || GameManager.instance.station == CraftingStation.DebugCrafter;
	}

	// Token: 0x06000257 RID: 599 RVA: 0x000138DC File Offset: 0x00011ADC
	private CraftingStation ReturnMyCraftingStation()
	{
		return GameManager.instance.currentRecipes[this.itemIndex + GameManager.instance.recipeTab * 18].craftingStation;
	}

	// Token: 0x0400033C RID: 828
	[SerializeField]
	private Image displayItem;

	// Token: 0x0400033D RID: 829
	[SerializeField]
	private int itemIndex;

	// Token: 0x0400033E RID: 830
	[SerializeField]
	private Color enabledColor;

	// Token: 0x0400033F RID: 831
	[SerializeField]
	private Color disabledColor;
}
