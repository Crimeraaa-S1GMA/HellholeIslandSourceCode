using System;
using UnityEngine;

// Token: 0x0200000C RID: 12
public class NpcHand : MonoBehaviour
{
	// Token: 0x0600002F RID: 47 RVA: 0x00002A7E File Offset: 0x00000C7E
	private void Start()
	{
		this.hand = base.GetComponent<SpriteRenderer>();
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00002A8C File Offset: 0x00000C8C
	private void Update()
	{
		if (this.body.sprite == this.thatOneNpcFrame)
		{
			this.hand.sprite = this.noArmor[1];
			return;
		}
		this.hand.sprite = this.noArmor[0];
	}

	// Token: 0x04000021 RID: 33
	public Sprite thatOneNpcFrame;

	// Token: 0x04000022 RID: 34
	public Sprite[] noArmor;

	// Token: 0x04000023 RID: 35
	public SpriteRenderer body;

	// Token: 0x04000024 RID: 36
	private SpriteRenderer hand;
}
