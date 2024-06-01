using System;
using UnityEngine;

// Token: 0x02000060 RID: 96
public class LeafBlock : MonoBehaviour
{
	// Token: 0x060001C6 RID: 454 RVA: 0x0000FAF6 File Offset: 0x0000DCF6
	private void Awake()
	{
		this.blockComp = base.GetComponent<PlacedBlock>();
	}

	// Token: 0x060001C7 RID: 455 RVA: 0x0000FB04 File Offset: 0x0000DD04
	private void Update()
	{
		if (GameManager.instance.leafBlockPositions.Contains(base.transform.position + Vector2.down))
		{
			this.blockComp.SetAnimationFrame(1);
			return;
		}
		this.blockComp.SetAnimationFrame(0);
	}

	// Token: 0x04000286 RID: 646
	private PlacedBlock blockComp;
}
