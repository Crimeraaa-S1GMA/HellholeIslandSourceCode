using System;
using UnityEngine;

// Token: 0x0200001F RID: 31
public class BlockAnimator : MonoBehaviour
{
	// Token: 0x0600007F RID: 127 RVA: 0x000043AC File Offset: 0x000025AC
	private void Awake()
	{
		this.blockComp = base.GetComponent<PlacedBlock>();
		base.InvokeRepeating("NextFrame", this.blockComp.ReturnBlock().frameChangeInterval, this.blockComp.ReturnBlock().frameChangeInterval);
	}

	// Token: 0x06000080 RID: 128 RVA: 0x000043E5 File Offset: 0x000025E5
	private void NextFrame()
	{
		this.blockComp.NextFrame();
	}

	// Token: 0x04000072 RID: 114
	private PlacedBlock blockComp;
}
