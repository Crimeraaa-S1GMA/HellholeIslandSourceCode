using System;
using UnityEngine;

// Token: 0x0200005F RID: 95
public class Lake : MonoBehaviour
{
	// Token: 0x060001C3 RID: 451 RVA: 0x0000F8F2 File Offset: 0x0000DAF2
	private void Start()
	{
		GameManager.instance.highlightableBlocks.Add(base.transform.position);
	}

	// Token: 0x060001C4 RID: 452 RVA: 0x0000F914 File Offset: 0x0000DB14
	private void Update()
	{
		if (GameManager.instance.ReturnSelectedItem().specialUsage == "Fishing" && GameManager.instance.selectionSquare == base.transform.position && GameManager.instance.selectedTeleporter == null && !GameManager.instance.generatingWorld && GameManager.instance.bed == null && GameManager.instance.chair == null && GameManager.instance.sign == null && Input.GetMouseButtonDown(0) && GameManager.instance.lake == null && GameManager.instance.fishingCooldown <= 0f)
		{
			AudioManager.instance.Play("Water");
			GameManager.instance.lake = this;
		}
		if (GameManager.instance.ReturnSelectedSlot().ITEM_ID == "bottle" && GameManager.instance.selectionSquare == base.transform.position && GameManager.instance.selectedTeleporter == null && !GameManager.instance.generatingWorld && Input.GetMouseButtonDown(0) && GameManager.instance.lake == null)
		{
			GameManager.instance.RemoveCurrentHeldItemOne();
			AudioManager.instance.Play("Water");
			GameManager.instance.InitializePickupItem(GameManager.instance.playerPos, "water_bottle", 1, true);
		}
		if (GameManager.instance.lake == this && Vector2.Distance(base.transform.position, GameManager.instance.playerPos) > 7f)
		{
			GameManager.instance.lake = null;
		}
	}
}
