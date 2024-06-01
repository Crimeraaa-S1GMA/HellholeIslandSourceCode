using System;
using System.Collections.Generic;

// Token: 0x02000029 RID: 41
public class Cauldron : InteractableBlock
{
	// Token: 0x0600009F RID: 159 RVA: 0x00004A32 File Offset: 0x00002C32
	private void Start()
	{
		this.placedBlock = base.GetComponent<PlacedBlock>();
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x00004A40 File Offset: 0x00002C40
	private void Update()
	{
		if (base.ReturnBlockInteraction())
		{
			if (GameManager.instance.ReturnSelectedSlot().ITEM_ID == "bottle" && this.ingredients.Count > 0)
			{
				GameManager.instance.RemoveCurrentHeldItemOne();
				string text = GameManager.instance.potionRegistryReference.FindPotionByIngredients(this.ingredients);
				AudioManager.instance.Play("Water");
				GameManager.instance.InitializePickupItem(GameManager.instance.playerPos, text, 1, true);
				if (text != "failed_potion")
				{
					AchievementManager.instance.AddAchievement(179820);
				}
				this.ingredients.Clear();
			}
			else if (GameManager.instance.ReturnSelectedSlot().ITEM_ID != "null" && GameManager.instance.ReturnSelectedSlot().ITEM_STACK > 0 && GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(GameManager.instance.ReturnSelectedSlot().ITEM_ID).isPotionIngredient && this.ingredients.Count < 6)
			{
				this.ingredients.Add(GameManager.instance.ReturnSelectedSlot().ITEM_ID);
				GameManager.instance.RemoveCurrentHeldItemOne();
				AudioManager.instance.Play("Water");
			}
		}
		this.SetIngredientPreviews();
	}

	// Token: 0x060000A1 RID: 161 RVA: 0x00004B90 File Offset: 0x00002D90
	private void SetIngredientPreviews()
	{
		for (int i = 0; i < this.placedBlock.ingredientSprites.Length; i++)
		{
			if (this.ingredients.Count - 1 < i)
			{
				this.placedBlock.ingredientSprites[i].sprite = null;
			}
			else
			{
				this.placedBlock.ingredientSprites[i].sprite = GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(this.ingredients[i]).sprite;
			}
		}
	}

	// Token: 0x040000B3 RID: 179
	private List<string> ingredients = new List<string>();

	// Token: 0x040000B4 RID: 180
	private PlacedBlock placedBlock;
}
