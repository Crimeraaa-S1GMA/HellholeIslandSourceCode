using System;
using UnityEngine;

// Token: 0x020000A5 RID: 165
public class WaterBlock : MonoBehaviour
{
	// Token: 0x06000324 RID: 804 RVA: 0x00017DCE File Offset: 0x00015FCE
	private void Awake()
	{
		this.blockComp = base.GetComponent<PlacedBlock>();
	}

	// Token: 0x06000325 RID: 805 RVA: 0x00017DDC File Offset: 0x00015FDC
	private void Update()
	{
		if (GameManager.instance.waterPositions.Contains(base.transform.position + Vector2.up))
		{
			this.blockComp.SetAnimationFrame(0);
			return;
		}
		this.blockComp.SetAnimationFrame(1);
	}

	// Token: 0x040003DC RID: 988
	private PlacedBlock blockComp;
}
