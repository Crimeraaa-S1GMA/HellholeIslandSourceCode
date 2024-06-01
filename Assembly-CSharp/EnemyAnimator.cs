using System;
using UnityEngine;

// Token: 0x02000036 RID: 54
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyAnimator : MonoBehaviour
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x060000C3 RID: 195 RVA: 0x00005882 File Offset: 0x00003A82
	// (set) Token: 0x060000C4 RID: 196 RVA: 0x0000588A File Offset: 0x00003A8A
	public int AnimationIndex { get; private set; }

	// Token: 0x060000C5 RID: 197 RVA: 0x00005893 File Offset: 0x00003A93
	private void Awake()
	{
		this.sr = base.GetComponent<SpriteRenderer>();
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x000058A4 File Offset: 0x00003AA4
	private void Update()
	{
		this.time += Time.deltaTime;
		if (this.time >= this.animations[this.AnimationIndex].frameChangeInterval)
		{
			this.time = 0f;
			this.animations[this.AnimationIndex].NextFrame();
		}
		this.sr.sprite = this.animations[this.AnimationIndex].ReturnSprite();
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x00005917 File Offset: 0x00003B17
	public void SetAnimation(int index)
	{
		if (this.AnimationIndex != index)
		{
			this.time = 0f;
			this.AnimationIndex = index;
			this.animations[index].Reset();
		}
	}

	// Token: 0x040000E3 RID: 227
	public EnemyAnimation[] animations;

	// Token: 0x040000E5 RID: 229
	private float time;

	// Token: 0x040000E6 RID: 230
	private SpriteRenderer sr;
}
