using System;
using UnityEngine;

// Token: 0x02000071 RID: 113
public class PlacedFloor : MonoBehaviour
{
	// Token: 0x06000213 RID: 531 RVA: 0x00011BFB File Offset: 0x0000FDFB
	private void Awake()
	{
		GameManager.instance.placedFloors.Add(this);
	}

	// Token: 0x06000214 RID: 532 RVA: 0x00011C10 File Offset: 0x0000FE10
	public void PrepareFloor(string key)
	{
		this.floorKey = key;
		this.actualWallHealth = this.ReturnFloor().maxFloorHealth;
		if (this.ReturnFloor().isStonePath)
		{
			this.floorSprite.sprite = GameManager.instance.floorRegistryReference.stonePathSelection[Random.Range(0, GameManager.instance.floorRegistryReference.stonePathSelection.Length)];
		}
		else
		{
			this.floorSprite.sprite = this.ReturnFloor().floorSprite;
		}
		this.slimeFloorCollider.enabled = this.ReturnFloor().isSlimeFloor;
		base.GetComponent<OptimizeRenderer>().StartUpdateRender();
	}

	// Token: 0x06000215 RID: 533 RVA: 0x00011CAD File Offset: 0x0000FEAD
	public Floor ReturnFloor()
	{
		return GameManager.instance.floorRegistryReference.FindFloorByInternalIdentifier(this.floorKey);
	}

	// Token: 0x06000216 RID: 534 RVA: 0x00011CC4 File Offset: 0x0000FEC4
	public void CheckForDeletion()
	{
		if (this.actualWallHealth <= 0f)
		{
			GameManager.instance.floorPositions.Remove(base.transform.position);
			this.IterDrops();
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06000217 RID: 535 RVA: 0x00011D04 File Offset: 0x0000FF04
	private void IterDrops()
	{
		foreach (LootDrop lootDrop in this.ReturnFloor().loot)
		{
			if (UtilityMath.RandomChance(1f / (lootDrop.DROP_CHANCE - 1f)) && lootDrop.DROP_ID != "null" && lootDrop.DROP_STACK_MIN != 0 && lootDrop.DROP_STACK_MAX != 0)
			{
				GameManager.instance.InitializePickupItem(base.transform.position + new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f)), lootDrop.DROP_ID, Random.Range(lootDrop.DROP_STACK_MIN, lootDrop.DROP_STACK_MAX), true);
			}
		}
	}

	// Token: 0x06000218 RID: 536 RVA: 0x00011DF4 File Offset: 0x0000FFF4
	private void OnDestroy()
	{
		GameManager.instance.placedFloors.Remove(this);
	}

	// Token: 0x040002F1 RID: 753
	public string floorKey = "";

	// Token: 0x040002F2 RID: 754
	public SpriteRenderer floorSprite;

	// Token: 0x040002F3 RID: 755
	public BoxCollider2D slimeFloorCollider;

	// Token: 0x040002F4 RID: 756
	[HideInInspector]
	public float actualWallHealth;
}
