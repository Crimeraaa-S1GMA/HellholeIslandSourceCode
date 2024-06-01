using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000082 RID: 130
public class ResultDisplay : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x06000259 RID: 601 RVA: 0x00013910 File Offset: 0x00011B10
	private void Update()
	{
		if (GameManager.instance.selectedRecipe == -1)
		{
			this.ingredientSprite.enabled = false;
			this.ingredientAmount.text = "";
			return;
		}
		this.ingredientSprite.enabled = true;
		this.ingredientSprite.sprite = GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(this.GetResult().COMP_ID).sprite;
		this.ingredientAmount.text = this.GetResult().COMP_STACK.ToString();
	}

	// Token: 0x0600025A RID: 602 RVA: 0x00013998 File Offset: 0x00011B98
	private RecipeComponent GetResult()
	{
		return GameManager.instance.currentRecipes[GameManager.instance.selectedRecipe].result;
	}

	// Token: 0x0600025B RID: 603 RVA: 0x000139B8 File Offset: 0x00011BB8
	public void OnPointerEnter(PointerEventData pointerEventData)
	{
		if (GameManager.instance.selectedRecipe != -1)
		{
			GameManager.instance.tooltipCustom = string.Format("<size=30>{0} ({1})</size>", GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(this.GetResult().COMP_ID).name, this.GetResult().COMP_STACK);
		}
	}

	// Token: 0x0600025C RID: 604 RVA: 0x00013A15 File Offset: 0x00011C15
	public void OnPointerExit(PointerEventData pointerEventData)
	{
		GameManager.instance.tooltipCustom = string.Empty;
	}

	// Token: 0x04000340 RID: 832
	[SerializeField]
	private Image ingredientSprite;

	// Token: 0x04000341 RID: 833
	[SerializeField]
	private Text ingredientAmount;
}
