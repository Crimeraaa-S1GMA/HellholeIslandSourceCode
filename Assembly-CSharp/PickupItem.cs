using System;
using UnityEngine;

// Token: 0x0200006F RID: 111
public class PickupItem : MonoBehaviour
{
	// Token: 0x060001FB RID: 507 RVA: 0x00010D1E File Offset: 0x0000EF1E
	private void Start()
	{
		this.itemCapacityLeft = this.itemQuantity;
		this.sprite = base.GetComponent<SpriteRenderer>();
		base.Invoke("EnablePickingUp", 5f);
	}

	// Token: 0x060001FC RID: 508 RVA: 0x00010D48 File Offset: 0x0000EF48
	private void Update()
	{
		if (this.itemId == "null" || this.itemQuantity <= 0)
		{
			this.sprite.sprite = null;
		}
		else
		{
			this.sprite.sprite = GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(this.itemId).sprite;
			this.sprite.material = (GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(this.itemId).spriteIsUnlit ? GameManager.instance.Unlit : GameManager.instance.Lit);
		}
		if (this.itemCapacityLeft <= 0)
		{
			Object.Destroy(base.gameObject);
		}
		if (GameManager.instance.playerMovement != null && this.canPickUp && (GameManager.instance.GetSlotForPickup(this.itemId) != -1 || this.itemId == "money" || this.itemId == "heart") && Vector2.Distance(base.transform.position, GameManager.instance.playerPos) < 3f + GameManager.instance.extraPickupRange)
		{
			base.transform.Translate((GameManager.instance.playerPos - base.transform.position).normalized * 20f * Time.deltaTime);
		}
		base.transform.position = new Vector2(base.transform.position.x, Mathf.Max(base.transform.position.y, (float)WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-148) + 0.2f));
	}

	// Token: 0x060001FD RID: 509 RVA: 0x00010F1C File Offset: 0x0000F11C
	public void EnablePickingUp()
	{
		this.canPickUp = true;
	}

	// Token: 0x060001FE RID: 510 RVA: 0x00010F28 File Offset: 0x0000F128
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && this.canPickUp)
		{
			if (this.itemId == "heart")
			{
				GameManager.instance.health += 4;
				this.itemCapacityLeft = 0;
				AudioManager.instance.Play("Harvest");
			}
			else if (this.itemId == "money")
			{
				GameManager.instance.money += this.itemCapacityLeft;
				this.itemCapacityLeft = 0;
				AudioManager.instance.Play("Harvest");
			}
			else if (this.itemCapacityLeft > 0)
			{
				int num = GameManager.instance.AddItem(this.itemId, this.itemQuantity, false);
				this.itemCapacityLeft -= num;
				if (num > 0)
				{
					AudioManager.instance.Play("Harvest");
				}
			}
		}
		if ((collision.CompareTag("Block") || collision.CompareTag("BlockProj")) && GameManager.instance.playerMovement != null)
		{
			base.transform.Translate((GameManager.instance.playerPos - base.transform.position).normalized * 20f * Time.deltaTime);
		}
	}

	// Token: 0x060001FF RID: 511 RVA: 0x00011089 File Offset: 0x0000F289
	public void InitializePickupItem(string id, int quantity)
	{
		this.itemId = id;
		this.itemQuantity = quantity;
		this.itemCapacityLeft = this.itemQuantity;
	}

	// Token: 0x06000200 RID: 512 RVA: 0x000110A8 File Offset: 0x0000F2A8
	private void OnMouseEnter()
	{
		if (this.itemId == "null" || this.itemQuantity <= 0)
		{
			GameManager.instance.tooltipCustom = "";
			return;
		}
		GameManager.instance.tooltipCustom = string.Format("{0} ({1})", GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(this.itemId).name, this.itemQuantity);
	}

	// Token: 0x06000201 RID: 513 RVA: 0x00011119 File Offset: 0x0000F319
	private void OnMouseExit()
	{
		GameManager.instance.tooltipCustom = "";
	}

	// Token: 0x040002D9 RID: 729
	public string itemId;

	// Token: 0x040002DA RID: 730
	public int itemQuantity;

	// Token: 0x040002DB RID: 731
	private int itemCapacityLeft;

	// Token: 0x040002DC RID: 732
	private SpriteRenderer sprite;

	// Token: 0x040002DD RID: 733
	private bool canPickUp;
}
