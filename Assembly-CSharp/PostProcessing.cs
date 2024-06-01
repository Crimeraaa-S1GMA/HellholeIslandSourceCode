using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000078 RID: 120
public class PostProcessing : MonoBehaviour
{
	// Token: 0x0600023E RID: 574 RVA: 0x00012D3A File Offset: 0x00010F3A
	private void Awake()
	{
		this.volume = base.GetComponent<Volume>();
	}

	// Token: 0x0600023F RID: 575 RVA: 0x00012D48 File Offset: 0x00010F48
	private void Update()
	{
		if (GameManager.instance.postProcessing)
		{
			this.volume.weight = 1f;
			return;
		}
		this.volume.weight = 0f;
	}

	// Token: 0x04000311 RID: 785
	private Volume volume;
}
