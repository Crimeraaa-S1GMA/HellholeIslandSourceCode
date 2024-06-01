using System;
using UnityEngine;

// Token: 0x0200006A RID: 106
public class NpcShopContainer : MonoBehaviour
{
	// Token: 0x060001EC RID: 492 RVA: 0x000107E6 File Offset: 0x0000E9E6
	private void Start()
	{
		this.rectTransform = base.GetComponent<RectTransform>();
	}

	// Token: 0x060001ED RID: 493 RVA: 0x000107F4 File Offset: 0x0000E9F4
	private void Update()
	{
		if (GameManager.instance.npcShop != null)
		{
			this.rectTransform.sizeDelta = new Vector2(0f, (float)Mathf.Max(GameManager.instance.npcShop.ReturnStoreSize() * 50 + 5, 350));
		}
	}

	// Token: 0x040002C8 RID: 712
	private RectTransform rectTransform;

	// Token: 0x040002C9 RID: 713
	private int len;
}
