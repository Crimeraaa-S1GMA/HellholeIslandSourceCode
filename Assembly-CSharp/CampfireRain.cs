using System;
using UnityEngine;

// Token: 0x02000028 RID: 40
public class CampfireRain : MonoBehaviour
{
	// Token: 0x0600009B RID: 155 RVA: 0x00004894 File Offset: 0x00002A94
	private void Awake()
	{
		this.stationBlock = base.GetComponent<StationBlock>();
		this.block = base.GetComponent<PlacedBlock>();
		if (this.block.blockMetadata.Count <= 0)
		{
			this.block.blockMetadata.Add("");
		}
		this.block.campfireCollider.enabled = true;
		base.InvokeRepeating("Animate", 0.2f, 0.2f);
	}

	// Token: 0x0600009C RID: 156 RVA: 0x00004908 File Offset: 0x00002B08
	private void Animate()
	{
		if (this.block.blockMetadata[0] != "R" && GameManager.instance.blockAnimations)
		{
			if (this.block.ReturnFrame() > 1)
			{
				this.block.SetAnimationFrame(0);
				return;
			}
			this.block.FrameMove(1);
		}
	}

	// Token: 0x0600009D RID: 157 RVA: 0x00004968 File Offset: 0x00002B68
	private void Update()
	{
		if (this.block.blockMetadata[0] == "R")
		{
			this.block.SetAnimationFrame(3);
			if (this.stationBlock != null)
			{
				Object.Destroy(this.stationBlock);
			}
			if (this.block.lightComp != null)
			{
				Object.Destroy(this.block.lightComp.gameObject);
			}
			this.block.campfireCollider.enabled = false;
			return;
		}
		if (GameManager.instance.rain && base.transform.position.x < 500f)
		{
			this.block.blockMetadata[0] = "R";
		}
	}

	// Token: 0x040000B1 RID: 177
	private StationBlock stationBlock;

	// Token: 0x040000B2 RID: 178
	private PlacedBlock block;
}
