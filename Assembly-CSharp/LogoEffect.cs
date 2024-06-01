using System;
using UnityEngine;

// Token: 0x02000062 RID: 98
public class LogoEffect : MonoBehaviour
{
	// Token: 0x060001CC RID: 460 RVA: 0x0000FC34 File Offset: 0x0000DE34
	private void Update()
	{
		base.transform.localScale = new Vector3(1.2f + Mathf.Sin(this.time) * 0.2f + this.extraScale, 1.2f + Mathf.Sin(this.time) * 0.2f + this.extraScale, 1f);
		this.extraScale = Mathf.SmoothDamp(this.extraScale, 0f, ref this.extraScaleVel, 0.3f);
		this.time += Time.deltaTime * 0.8f;
	}

	// Token: 0x0400028A RID: 650
	private float time = -0.5f;

	// Token: 0x0400028B RID: 651
	private float extraScale = 20f;

	// Token: 0x0400028C RID: 652
	private float extraScaleVel;
}
