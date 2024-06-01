using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200003B RID: 59
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyEyes : MonoBehaviour
{
	// Token: 0x060000F4 RID: 244 RVA: 0x00006B86 File Offset: 0x00004D86
	private void Awake()
	{
		this.sr = base.GetComponent<SpriteRenderer>();
		base.StartCoroutine("Eyes");
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x00006BA0 File Offset: 0x00004DA0
	private void Update()
	{
		this.sr.sprite = (this.eyesOpen ? this.openEyes : this.closedEyes);
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x00006BC3 File Offset: 0x00004DC3
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

	// Token: 0x04000117 RID: 279
	public Sprite openEyes;

	// Token: 0x04000118 RID: 280
	public Sprite closedEyes;

	// Token: 0x04000119 RID: 281
	private SpriteRenderer sr;

	// Token: 0x0400011A RID: 282
	private bool eyesOpen = true;
}
