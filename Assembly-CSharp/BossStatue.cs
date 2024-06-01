using System;
using UnityEngine;

// Token: 0x02000025 RID: 37
public class BossStatue : InteractableBlock
{
	// Token: 0x0600008C RID: 140 RVA: 0x000045CB File Offset: 0x000027CB
	private void Start()
	{
		this.bossId = Random.Range(0, 6);
	}

	// Token: 0x0600008D RID: 141 RVA: 0x000045DC File Offset: 0x000027DC
	private void Update()
	{
		if (base.ReturnBlockInteraction() && GameManager.instance.boss == null && GameManager.instance.invasionEvent == Invasion.None)
		{
			GameManager.instance.tooltipCustom = "";
			base.GetComponent<PlacedBlock>().actualBlockHealth = 0f;
			base.GetComponent<PlacedBlock>().CheckForDeletion();
			Object.FindObjectOfType<EnemySpawner>().SpawnEnemyAtPosition(this.BossId(), base.transform.position + new Vector2((float)(9 * UtilityMath.NegativePositiveOne()), (float)(9 * UtilityMath.NegativePositiveOne())));
		}
		this.DetectSelSquare();
	}

	// Token: 0x0600008E RID: 142 RVA: 0x00004680 File Offset: 0x00002880
	private void UpdateTooltip()
	{
		if (this.isSelecting && GameManager.instance.selectedTeleporter == null && GameManager.instance.sign == null)
		{
			GameManager.instance.tooltipCustom = "Fight " + this.BossName();
			return;
		}
		GameManager.instance.tooltipCustom = "";
	}

	// Token: 0x0600008F RID: 143 RVA: 0x000046E4 File Offset: 0x000028E4
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

	// Token: 0x06000090 RID: 144 RVA: 0x00004740 File Offset: 0x00002940
	private string BossId()
	{
		switch (this.bossId)
		{
		case 0:
			return "oregolem";
		case 1:
			return "joe";
		case 2:
			return "treeguardian";
		case 3:
			return "hellbot";
		case 4:
			return "crazyclown";
		case 5:
			return "jeff";
		default:
			return "dontspawn";
		}
	}

	// Token: 0x06000091 RID: 145 RVA: 0x000047A0 File Offset: 0x000029A0
	private string BossName()
	{
		switch (this.bossId)
		{
		case 0:
			return "Ore Golem";
		case 1:
			return "Joe";
		case 2:
			return "Tree Guardian";
		case 3:
			return "Magma Monster";
		case 4:
			return "Crazy Clown";
		case 5:
			return "Jeff";
		default:
			return string.Empty;
		}
	}

	// Token: 0x040000AA RID: 170
	public int bossId;

	// Token: 0x040000AB RID: 171
	private bool isSelecting;
}
