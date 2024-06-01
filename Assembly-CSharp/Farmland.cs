using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000040 RID: 64
public class Farmland : InteractableBlock
{
	// Token: 0x06000109 RID: 265 RVA: 0x00007760 File Offset: 0x00005960
	private void Start()
	{
		if (base.GetComponent<PlacedBlock>().blockMetadata.Count < 2)
		{
			base.GetComponent<PlacedBlock>().blockMetadata.Add("0");
			base.GetComponent<PlacedBlock>().blockMetadata.Add("0");
		}
		GameManager.instance.farmlands.Add(this);
		this.placedBlock = base.GetComponent<PlacedBlock>();
		try
		{
			this.crop = Convert.ToInt32(this.placedBlock.blockMetadata[0]);
			this.cropStage = Convert.ToInt32(this.placedBlock.blockMetadata[1]);
		}
		catch
		{
			this.crop = 0;
			this.cropStage = 0;
		}
		base.StartCoroutine("SaveCropAttributes");
	}

	// Token: 0x0600010A RID: 266 RVA: 0x00007830 File Offset: 0x00005A30
	private IEnumerator SaveCropAttributes()
	{
		for (;;)
		{
			yield return new WaitForSeconds(0.2f);
			this.placedBlock.blockMetadata[0] = this.crop.ToString();
			this.placedBlock.blockMetadata[1] = this.cropStage.ToString();
		}
		yield break;
	}

	// Token: 0x0600010B RID: 267 RVA: 0x00007840 File Offset: 0x00005A40
	private void Update()
	{
		if (Random.Range(1, (GameManager.instance.rain ? 5501 : 9001) + (WorldPolicy.hellholeMode ? 700 : 0)) == 1 && this.crop != 0 && !GameManager.instance.generatingWorld)
		{
			this.cropStage = Mathf.Clamp(this.cropStage + 1, 0, 3);
		}
		else
		{
			this.cropStage = Mathf.Clamp(this.cropStage, 0, 3);
		}
		switch (this.crop)
		{
		case 0:
			this.placedBlock.cropSprite.sprite = null;
			this.cropStage = 0;
			break;
		case 1:
			this.placedBlock.cropSprite.sprite = GameManager.instance.blockRegistryReference.wheatStages[this.cropStage];
			break;
		case 2:
			this.placedBlock.cropSprite.sprite = GameManager.instance.blockRegistryReference.tomatoStages[this.cropStage];
			break;
		case 3:
			this.placedBlock.cropSprite.sprite = GameManager.instance.blockRegistryReference.carrotStages[this.cropStage];
			break;
		}
		this.placedBlock.cropSprite.sortingOrder = ((this.cropStage < 2) ? -1 : 0);
		if (this.cropStage == 3 && base.ReturnBlockInteractionFarmland())
		{
			switch (this.crop)
			{
			case 1:
				GameManager.instance.InitializePickupItem(base.transform.position, "wheat", Random.Range(1, 4), true);
				GameManager.instance.InitializePickupItem(base.transform.position, "wheat_seeds", Random.Range(1, 3), true);
				break;
			case 2:
				GameManager.instance.InitializePickupItem(base.transform.position, "tomato", Random.Range(1, 4), true);
				GameManager.instance.InitializePickupItem(base.transform.position, "tomato_seeds", Random.Range(1, 3), true);
				break;
			case 3:
				GameManager.instance.InitializePickupItem(base.transform.position, "carrot", Random.Range(1, 4), true);
				GameManager.instance.InitializePickupItem(base.transform.position, "carrot_seeds", Random.Range(1, 3), true);
				break;
			}
			AchievementManager.instance.AddAchievement(156203);
			AudioManager.instance.Play("Harvest");
			this.crop = 0;
			this.cropStage = 0;
		}
	}

	// Token: 0x0600010C RID: 268 RVA: 0x00007AE5 File Offset: 0x00005CE5
	public int ReturnCrop()
	{
		return this.crop;
	}

	// Token: 0x0600010D RID: 269 RVA: 0x00007AED File Offset: 0x00005CED
	public int ReturnCropStage()
	{
		return this.cropStage;
	}

	// Token: 0x0600010E RID: 270 RVA: 0x00007AF5 File Offset: 0x00005CF5
	public void SetUpCrop(int id, int stage)
	{
		this.crop = Mathf.Clamp(id, 0, 3);
		this.cropStage = Mathf.Clamp(stage, 0, 3);
	}

	// Token: 0x0600010F RID: 271 RVA: 0x00007B13 File Offset: 0x00005D13
	public void Fertilize()
	{
		this.cropStage = 3;
	}

	// Token: 0x06000110 RID: 272 RVA: 0x00007B1C File Offset: 0x00005D1C
	private void OnDestroy()
	{
		GameManager.instance.farmlands.Remove(this);
	}

	// Token: 0x0400012F RID: 303
	private PlacedBlock placedBlock;

	// Token: 0x04000130 RID: 304
	private int crop;

	// Token: 0x04000131 RID: 305
	private int cropStage;
}
