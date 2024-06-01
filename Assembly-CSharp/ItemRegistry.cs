using System;
using UnityEngine;

// Token: 0x02000057 RID: 87
[CreateAssetMenu(fileName = "NewItemReg", menuName = "Registries/Item Registry", order = 0)]
public class ItemRegistry : ScriptableObject
{
	// Token: 0x060001BB RID: 443 RVA: 0x0000F318 File Offset: 0x0000D518
	public int FindItemIdByInternalIdentifier(string identifier)
	{
		int result = Array.IndexOf<Item>(this.items, Array.Find<Item>(this.items, (Item i) => i.internalIdentifier == "unloaded_item"));
		Item item = Array.Find<Item>(this.items, (Item i) => i.internalIdentifier == identifier);
		if (item != null && (!item.experimentalItem || WorldPolicy.experimentalFeatures))
		{
			result = Array.IndexOf<Item>(this.items, item);
		}
		return result;
	}

	// Token: 0x060001BC RID: 444 RVA: 0x0000F3A0 File Offset: 0x0000D5A0
	public Item FindItemByInternalIdentifier(string identifier)
	{
		Item result = Array.Find<Item>(this.items, (Item i) => i.internalIdentifier == "unloaded_item");
		Item item = Array.Find<Item>(this.items, (Item i) => i.internalIdentifier == identifier);
		if (item != null && (!item.experimentalItem || WorldPolicy.experimentalFeatures))
		{
			return item;
		}
		return result;
	}

	// Token: 0x04000232 RID: 562
	public Item[] items;
}
