using System;

// Token: 0x0200007D RID: 125
[Serializable]
public class Recipe
{
	// Token: 0x04000328 RID: 808
	public string recipeDescription;

	// Token: 0x04000329 RID: 809
	public CraftingStation craftingStation;

	// Token: 0x0400032A RID: 810
	public bool isAchievement;

	// Token: 0x0400032B RID: 811
	public int achievementId;

	// Token: 0x0400032C RID: 812
	public RecipeComponent[] ingredients;

	// Token: 0x0400032D RID: 813
	public RecipeComponent result;

	// Token: 0x0400032E RID: 814
	public bool experimental;
}
