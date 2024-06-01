using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000074 RID: 116
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerEyes : MonoBehaviour
{
	// Token: 0x06000224 RID: 548 RVA: 0x0001226F File Offset: 0x0001046F
	private void Awake()
	{
		this.sr = base.GetComponent<SpriteRenderer>();
		base.StartCoroutine("Eyes");
	}

	// Token: 0x06000225 RID: 549 RVA: 0x0001228C File Offset: 0x0001048C
	private void Update()
	{
		this.sr.sprite = GameManager.instance.ReturnPlayerPartSprite(this.eyesOpen ? 2 : 3);
		this.sr.material = (GameManager.instance.IsArmorGlowmask(37) ? GameManager.instance.Unlit : GameManager.instance.Lit);
	}

	// Token: 0x06000226 RID: 550 RVA: 0x000122E9 File Offset: 0x000104E9
	private IEnumerator Eyes()
	{
		for (;;)
		{
			yield return new WaitForSeconds(4f);
			this.eyesOpen = false;
			yield return new WaitForSeconds(0.15f);
			this.eyesOpen = true;
		}
		yield break;
	}

	// Token: 0x040002FD RID: 765
	private SpriteRenderer sr;

	// Token: 0x040002FE RID: 766
	private bool eyesOpen = true;
}
