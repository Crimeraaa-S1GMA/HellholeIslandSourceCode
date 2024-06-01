using System;
using UnityEngine;

// Token: 0x0200003A RID: 58
public class EnemyCorpse : MonoBehaviour
{
	// Token: 0x060000F1 RID: 241 RVA: 0x00006B0C File Offset: 0x00004D0C
	private void Awake()
	{
		this.spriteRenderer = base.GetComponent<SpriteRenderer>();
		base.transform.eulerAngles = Vector3.forward * 90f * (float)UtilityMath.NegativePositiveOne();
		Object.Destroy(base.gameObject, 20f);
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x00006B5A File Offset: 0x00004D5A
	private void Update()
	{
		this.spriteRenderer.sprite = this.enemySprite;
		this.spriteRenderer.color = this.enemyColor;
	}

	// Token: 0x04000114 RID: 276
	private SpriteRenderer spriteRenderer;

	// Token: 0x04000115 RID: 277
	public Sprite enemySprite;

	// Token: 0x04000116 RID: 278
	public Color enemyColor;
}
