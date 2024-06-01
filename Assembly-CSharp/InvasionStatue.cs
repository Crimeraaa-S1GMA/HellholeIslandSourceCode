using System;

// Token: 0x02000055 RID: 85
public class InvasionStatue : InteractableBlock
{
	// Token: 0x060001B5 RID: 437 RVA: 0x0000F18B File Offset: 0x0000D38B
	private void Start()
	{
		this.invasion = Invasion.CubicChaos;
	}

	// Token: 0x060001B6 RID: 438 RVA: 0x0000F194 File Offset: 0x0000D394
	private void Update()
	{
		if (base.ReturnBlockInteraction() && GameManager.instance.boss == null && GameManager.instance.invasionEvent == Invasion.None)
		{
			GameManager.instance.tooltipCustom = "";
			base.GetComponent<PlacedBlock>().actualBlockHealth = 0f;
			base.GetComponent<PlacedBlock>().CheckForDeletion();
			GameManager.instance.InvasionEvent(GameManager.instance.InvasionEnemyAmount(this.invasion), this.invasion);
		}
		this.DetectSelSquare();
	}

	// Token: 0x060001B7 RID: 439 RVA: 0x0000F218 File Offset: 0x0000D418
	private void UpdateTooltip()
	{
		if (this.isSelecting && GameManager.instance.selectedTeleporter == null && GameManager.instance.sign == null)
		{
			GameManager.instance.tooltipCustom = "Fight off " + GameManager.instance.InvasionName(this.invasion);
			return;
		}
		GameManager.instance.tooltipCustom = "";
	}

	// Token: 0x060001B8 RID: 440 RVA: 0x0000F288 File Offset: 0x0000D488
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

	// Token: 0x0400022E RID: 558
	public Invasion invasion;

	// Token: 0x0400022F RID: 559
	private bool isSelecting;
}
