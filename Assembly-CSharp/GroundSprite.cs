using System;
using UnityEngine;

// Token: 0x0200004D RID: 77
public class GroundSprite : MonoBehaviour
{
	// Token: 0x0600018C RID: 396 RVA: 0x0000D40D File Offset: 0x0000B60D
	private void Awake()
	{
		this.sr = base.GetComponent<SpriteRenderer>();
	}

	// Token: 0x0600018D RID: 397 RVA: 0x0000D41B File Offset: 0x0000B61B
	private void Update()
	{
		this.sr.sprite = this.ReturnSprite();
	}

	// Token: 0x0600018E RID: 398 RVA: 0x0000D430 File Offset: 0x0000B630
	private Sprite ReturnSprite()
	{
		if (GameManager.instance.isInMines)
		{
			return this.sprites[1];
		}
		if (GameManager.instance.sniperSkillsSeed)
		{
			return this.sprites[3];
		}
		if (GameManager.instance.nostalgiaSeed)
		{
			return this.sprites[2];
		}
		return this.sprites[0];
	}

	// Token: 0x040001FC RID: 508
	public Sprite[] sprites;

	// Token: 0x040001FD RID: 509
	private SpriteRenderer sr;
}
