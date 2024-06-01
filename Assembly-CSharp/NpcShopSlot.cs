using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200006B RID: 107
public class NpcShopSlot : MonoBehaviour
{
	// Token: 0x060001EF RID: 495 RVA: 0x0001084F File Offset: 0x0000EA4F
	private void Awake()
	{
		this.button = base.GetComponent<Button>();
	}

	// Token: 0x060001F0 RID: 496 RVA: 0x00010860 File Offset: 0x0000EA60
	private void Update()
	{
		if (GameManager.instance.npcShop != null)
		{
			this.slotImage.sprite = GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(GameManager.instance.npcShop.ReturnStoreItemId(this.slotId)).sprite;
			if (GameManager.instance.npcShop.ReturnStoreItemId(this.slotId) != "null")
			{
				this.slotText.text = GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(GameManager.instance.npcShop.ReturnStoreItemId(this.slotId)).name + "\n" + string.Format("{0}$", GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(GameManager.instance.npcShop.ReturnStoreItemId(this.slotId)).price);
				this.button.interactable = true;
				return;
			}
			this.slotText.text = "";
			this.button.interactable = false;
		}
	}

	// Token: 0x060001F1 RID: 497 RVA: 0x0001097C File Offset: 0x0000EB7C
	public void BuyItem()
	{
		if (GameManager.instance.npcShop.ReturnStoreItemId(this.slotId) != "null" && GameManager.instance.money >= GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(GameManager.instance.npcShop.ReturnStoreItemId(this.slotId)).price)
		{
			AchievementManager.instance.AddAchievement(156205);
			GameManager.instance.InitializePickupItem(GameManager.instance.playerPos, GameManager.instance.npcShop.ReturnStoreItemId(this.slotId), 1, true);
			GameManager.instance.money -= GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(GameManager.instance.npcShop.ReturnStoreItemId(this.slotId)).price;
		}
	}

	// Token: 0x040002CA RID: 714
	public int slotId;

	// Token: 0x040002CB RID: 715
	public Image slotImage;

	// Token: 0x040002CC RID: 716
	public Text slotText;

	// Token: 0x040002CD RID: 717
	private Button button;
}
