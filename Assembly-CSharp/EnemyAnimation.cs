using System;
using UnityEngine;

// Token: 0x02000035 RID: 53
[Serializable]
public class EnemyAnimation
{
	// Token: 0x060000BF RID: 191 RVA: 0x00005849 File Offset: 0x00003A49
	public void Reset()
	{
		this.frame = 0;
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x00005852 File Offset: 0x00003A52
	public void NextFrame()
	{
		this.frame = (this.frame + 1) % this.sprites.Length;
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x0000586B File Offset: 0x00003A6B
	public Sprite ReturnSprite()
	{
		return this.sprites[this.frame];
	}

	// Token: 0x040000E0 RID: 224
	public Sprite[] sprites;

	// Token: 0x040000E1 RID: 225
	public float frameChangeInterval;

	// Token: 0x040000E2 RID: 226
	private int frame;
}
