using System;
using UnityEngine;

// Token: 0x02000080 RID: 128
[CreateAssetMenu(fileName = "NewRecReg", menuName = "Registries/Recipe Registry", order = 1)]
public class RecipeRegistry : ScriptableObject
{
	// Token: 0x0400033B RID: 827
	public Recipe[] recipes;
}
