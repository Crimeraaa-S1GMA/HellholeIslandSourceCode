using System;
using UnityEngine;

// Token: 0x020000A7 RID: 167
public class Wire : MonoBehaviour
{
	// Token: 0x0600032A RID: 810 RVA: 0x00017EB2 File Offset: 0x000160B2
	private void Start()
	{
		this.spriteRenderer = base.GetComponent<SpriteRenderer>();
	}

	// Token: 0x0600032B RID: 811 RVA: 0x00017EC0 File Offset: 0x000160C0
	private void Update()
	{
		this.DetectWires();
		this.FindSpriteAndApplyIt();
	}

	// Token: 0x0600032C RID: 812 RVA: 0x00017ED0 File Offset: 0x000160D0
	private void DetectWires()
	{
		if (GameManager.instance.wirePositions.Contains(base.transform.position + Vector2.up))
		{
			this.up = "1";
		}
		else
		{
			this.up = "0";
		}
		if (GameManager.instance.wirePositions.Contains(base.transform.position + Vector2.right))
		{
			this.right = "1";
		}
		else
		{
			this.right = "0";
		}
		if (GameManager.instance.wirePositions.Contains(base.transform.position + Vector2.down))
		{
			this.bottom = "1";
		}
		else
		{
			this.bottom = "0";
		}
		if (GameManager.instance.wirePositions.Contains(base.transform.position + Vector2.left))
		{
			this.left = "1";
			return;
		}
		this.left = "0";
	}

	// Token: 0x0600032D RID: 813 RVA: 0x00017FE8 File Offset: 0x000161E8
	private void FindSpriteAndApplyIt()
	{
		WireSprite wireSprite = Array.Find<WireSprite>(GameManager.instance.blockRegistryReference.wireSprites, (WireSprite w) => w.key == this.up + this.right + this.bottom + this.left);
		if (wireSprite != null)
		{
			this.spriteRenderer.sprite = wireSprite.correspondingSprite;
		}
	}

	// Token: 0x040003DE RID: 990
	private SpriteRenderer spriteRenderer;

	// Token: 0x040003DF RID: 991
	private string up;

	// Token: 0x040003E0 RID: 992
	private string right;

	// Token: 0x040003E1 RID: 993
	private string bottom;

	// Token: 0x040003E2 RID: 994
	private string left;
}
