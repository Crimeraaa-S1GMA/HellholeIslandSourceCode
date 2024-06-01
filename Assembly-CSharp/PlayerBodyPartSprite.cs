using System;
using UnityEngine;

// Token: 0x02000073 RID: 115
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerBodyPartSprite : MonoBehaviour
{
	// Token: 0x06000221 RID: 545 RVA: 0x000121F4 File Offset: 0x000103F4
	private void Awake()
	{
		this.sr = base.GetComponent<SpriteRenderer>();
	}

	// Token: 0x06000222 RID: 546 RVA: 0x00012204 File Offset: 0x00010404
	private void Update()
	{
		this.sr.sprite = GameManager.instance.ReturnPlayerPartSprite(this.bodyPartIndex);
		this.sr.material = (GameManager.instance.IsArmorGlowmask(this.armorSlotId) ? GameManager.instance.Unlit : GameManager.instance.Lit);
	}

	// Token: 0x040002FA RID: 762
	public int bodyPartIndex;

	// Token: 0x040002FB RID: 763
	public int armorSlotId = 37;

	// Token: 0x040002FC RID: 764
	private SpriteRenderer sr;
}
