using System;
using UnityEngine;

// Token: 0x02000054 RID: 84
public class InteractableBlock : MonoBehaviour
{
	// Token: 0x060001B0 RID: 432 RVA: 0x0000F0EA File Offset: 0x0000D2EA
	protected bool ReturnOpeningBlockKey()
	{
		return Input.GetKeyDown(KeyCode.E);
	}

	// Token: 0x060001B1 RID: 433 RVA: 0x0000F0F3 File Offset: 0x0000D2F3
	protected bool ReturnBlockInteraction()
	{
		return base.transform.position == GameManager.instance.selectionSquare && this.ReturnOpeningBlockKey() && GameManager.instance.AllowInteractablity();
	}

	// Token: 0x060001B2 RID: 434 RVA: 0x0000F12A File Offset: 0x0000D32A
	protected bool ReturnBlockInteractionFarmland()
	{
		return base.transform.position == GameManager.instance.selectionSquare && Input.GetKey(KeyCode.E) && GameManager.instance.AllowInteractablity();
	}

	// Token: 0x060001B3 RID: 435 RVA: 0x0000F162 File Offset: 0x0000D362
	private void Awake()
	{
		GameManager.instance.highlightableBlocks.Add(base.transform.position);
	}
}
