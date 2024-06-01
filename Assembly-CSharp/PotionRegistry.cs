using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000079 RID: 121
[CreateAssetMenu(fileName = "NewPotionReg", menuName = "Registries/PotionRegistry", order = 8)]
public class PotionRegistry : ScriptableObject
{
	// Token: 0x06000241 RID: 577 RVA: 0x00012D80 File Offset: 0x00010F80
	public string FindPotionByIngredients(List<string> ingredients)
	{
		string result = "failed_potion";
		Potion potion = Array.Find<Potion>(this.potions, (Potion p) => (from x in p.ingredients
		orderby x
		select x).ToList<string>().SequenceEqual((from x in ingredients
		orderby x
		select x).ToList<string>()));
		if (potion != null)
		{
			result = potion.potionId;
		}
		return result;
	}

	// Token: 0x04000312 RID: 786
	public Potion[] potions;
}
