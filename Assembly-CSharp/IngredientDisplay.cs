using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000053 RID: 83
public class IngredientDisplay : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x060001AB RID: 427 RVA: 0x0000EE64 File Offset: 0x0000D064
	private void Update()
	{
		if (GameManager.instance.selectedRecipe == -1 || GameManager.instance.currentRecipes[GameManager.instance.selectedRecipe].ingredients.Length <= this.ingredientId)
		{
			this.ingredientSprite.enabled = false;
			this.ingredientAmount.text = "";
			return;
		}
		this.ingredientSprite.enabled = true;
		this.ingredientSprite.sprite = GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(this.GetIngredient().COMP_ID).sprite;
		if (GameManager.instance.ItemCount(this.GetIngredient().COMP_ID, true) < this.GetIngredient().COMP_STACK)
		{
			this.ingredientAmount.color = this.missingIngredientsColor;
			this.ingredientAmount.text = GameManager.instance.ItemCount(this.GetIngredient().COMP_ID, true).ToString() + "/" + this.GetIngredient().COMP_STACK.ToString();
			return;
		}
		this.ingredientAmount.color = this.normalColor;
		this.ingredientAmount.text = this.GetIngredient().COMP_STACK.ToString();
	}

	// Token: 0x060001AC RID: 428 RVA: 0x0000EF9D File Offset: 0x0000D19D
	private RecipeComponent GetIngredient()
	{
		return GameManager.instance.currentRecipes[GameManager.instance.selectedRecipe].ingredients[this.ingredientId];
	}

	// Token: 0x060001AD RID: 429 RVA: 0x0000EFC4 File Offset: 0x0000D1C4
	public void OnPointerEnter(PointerEventData pointerEventData)
	{
		if (GameManager.instance.selectedRecipe != -1 && GameManager.instance.currentRecipes[GameManager.instance.selectedRecipe].ingredients.Length > this.ingredientId)
		{
			if (GameManager.instance.ItemCount(this.GetIngredient().COMP_ID, true) < this.GetIngredient().COMP_STACK)
			{
				GameManager.instance.tooltipCustom = string.Format("<size=30>{0} (<color=#FF0000>{1}/{2}</color>)</size>", GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(this.GetIngredient().COMP_ID).name, GameManager.instance.ItemCount(this.GetIngredient().COMP_ID, true), this.GetIngredient().COMP_STACK);
				return;
			}
			GameManager.instance.tooltipCustom = string.Format("<size=30>{0} (<color=#00FF00>{1}</color>)</size>", GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(this.GetIngredient().COMP_ID).name, this.GetIngredient().COMP_STACK);
		}
	}

	// Token: 0x060001AE RID: 430 RVA: 0x0000F0D1 File Offset: 0x0000D2D1
	public void OnPointerExit(PointerEventData pointerEventData)
	{
		GameManager.instance.tooltipCustom = string.Empty;
	}

	// Token: 0x04000229 RID: 553
	[SerializeField]
	private int ingredientId;

	// Token: 0x0400022A RID: 554
	[SerializeField]
	private Image ingredientSprite;

	// Token: 0x0400022B RID: 555
	[SerializeField]
	private Text ingredientAmount;

	// Token: 0x0400022C RID: 556
	[SerializeField]
	private Color missingIngredientsColor;

	// Token: 0x0400022D RID: 557
	[SerializeField]
	private Color normalColor;
}
