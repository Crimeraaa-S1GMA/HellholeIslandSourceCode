using System;
using UnityEngine;

// Token: 0x02000084 RID: 132
public class Sign : InteractableBlock
{
	// Token: 0x06000261 RID: 609 RVA: 0x00013EFE File Offset: 0x000120FE
	private void Start()
	{
		this.block = base.GetComponent<PlacedBlock>();
		if (this.block.blockMetadata.Count <= 0)
		{
			this.block.blockMetadata.Add("");
		}
	}

	// Token: 0x06000262 RID: 610 RVA: 0x00013F34 File Offset: 0x00012134
	private void Update()
	{
		if (base.ReturnBlockInteraction())
		{
			GameManager.instance.fullInventoryOpen = true;
			GameManager.instance.CloseAllMenus();
			GameManager.instance.sign = this;
			Object.FindObjectOfType<InventoryUiEvents>().LoadSignContent();
		}
		this.DetectSelSquare();
	}

	// Token: 0x06000263 RID: 611 RVA: 0x00013F70 File Offset: 0x00012170
	public void SaveContent(string signContent)
	{
		this.block.blockMetadata[0] = signContent;
		if (this.isStructureBlock)
		{
			try
			{
				this.block.actualBlockHealth = 0f;
				this.block.CheckForDeletion();
				Object.FindObjectOfType<WorldGeneration>().SpawnStructure(GameManager.instance.structureRegistryReference.FindStructureByInternalIdentifier(signContent), base.transform.position - Vector2.right * 0.5f);
			}
			catch
			{
				this.block.actualBlockHealth = 0f;
				this.block.CheckForDeletion();
			}
		}
	}

	// Token: 0x06000264 RID: 612 RVA: 0x00014020 File Offset: 0x00012220
	public PlacedBlock ReturnBlockComp()
	{
		return this.block;
	}

	// Token: 0x06000265 RID: 613 RVA: 0x00014028 File Offset: 0x00012228
	private void UpdateTooltip()
	{
		if (this.isSelecting && GameManager.instance.selectedTeleporter == null && GameManager.instance.sign == null)
		{
			GameManager.instance.tooltipCustom = this.block.blockMetadata[0];
			return;
		}
		GameManager.instance.tooltipCustom = "";
	}

	// Token: 0x06000266 RID: 614 RVA: 0x0001408C File Offset: 0x0001228C
	private void DetectSelSquare()
	{
		if (base.transform.position == GameManager.instance.selectionSquare)
		{
			if (!this.isSelecting)
			{
				this.isSelecting = true;
				this.UpdateTooltip();
				return;
			}
		}
		else if (this.isSelecting)
		{
			this.isSelecting = false;
			this.UpdateTooltip();
		}
	}

	// Token: 0x04000344 RID: 836
	public bool isStructureBlock;

	// Token: 0x04000345 RID: 837
	private bool isSelecting;

	// Token: 0x04000346 RID: 838
	private PlacedBlock block;
}
