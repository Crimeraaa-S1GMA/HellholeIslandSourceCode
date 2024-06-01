using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200006C RID: 108
public class OptimizeRenderer : MonoBehaviour
{
	// Token: 0x060001F3 RID: 499 RVA: 0x00010A60 File Offset: 0x0000EC60
	public void StartUpdateRender()
	{
		this.block = base.GetComponent<PlacedBlock>();
		this.tree = base.GetComponent<PlacedTree>();
		this.floor = base.GetComponent<PlacedFloor>();
		this.rb = base.GetComponent<Rigidbody2D>();
		this.lake = base.GetComponent<Lake>();
		this.waterBlock = base.GetComponent<WaterBlock>();
		base.StartCoroutine("OptimizerCoroutine");
	}

	// Token: 0x060001F4 RID: 500 RVA: 0x00010AC1 File Offset: 0x0000ECC1
	private IEnumerator OptimizerCoroutine()
	{
		yield return new WaitForSeconds(1f);
		for (;;)
		{
			if (!GameManager.instance.generatingWorld)
			{
				this.UpdateEnable();
			}
			yield return new WaitForSeconds(3f);
		}
		yield break;
	}

	// Token: 0x060001F5 RID: 501 RVA: 0x00010AD0 File Offset: 0x0000ECD0
	private void UpdateEnable()
	{
		if (Vector2.Distance(base.transform.position, GameManager.instance.playerPos) > 9f || GameManager.instance.generatingWorld)
		{
			if (this.block != null)
			{
				this.block.enabled = false;
			}
			if (this.tree != null)
			{
				this.tree.enabled = false;
			}
			if (this.floor != null)
			{
				this.floor.enabled = false;
			}
			if (this.lake != null)
			{
				this.lake.enabled = false;
			}
			if (this.waterBlock != null)
			{
				this.waterBlock.enabled = false;
			}
			if (this.rb != null && !this.rb.IsSleeping())
			{
				this.rb.Sleep();
				return;
			}
		}
		else
		{
			if (this.block != null)
			{
				this.block.enabled = true;
			}
			if (this.tree != null)
			{
				this.tree.enabled = true;
			}
			if (this.floor != null)
			{
				this.floor.enabled = true;
			}
			if (this.lake != null)
			{
				this.lake.enabled = true;
			}
			if (this.waterBlock != null)
			{
				this.waterBlock.enabled = true;
			}
			if (this.rb != null && this.rb.IsSleeping())
			{
				this.rb.WakeUp();
			}
		}
	}

	// Token: 0x040002CE RID: 718
	private PlacedBlock block;

	// Token: 0x040002CF RID: 719
	private PlacedTree tree;

	// Token: 0x040002D0 RID: 720
	private PlacedFloor floor;

	// Token: 0x040002D1 RID: 721
	private Rigidbody2D rb;

	// Token: 0x040002D2 RID: 722
	private Lake lake;

	// Token: 0x040002D3 RID: 723
	private WaterBlock waterBlock;
}
