using System;
using UnityEngine;

// Token: 0x02000031 RID: 49
public class DoorMechanic : MonoBehaviour
{
	// Token: 0x060000B7 RID: 183 RVA: 0x00005598 File Offset: 0x00003798
	private void Start()
	{
		this.placedBlock = base.GetComponent<PlacedBlock>();
		this.hitbox = base.GetComponent<BoxCollider2D>();
		this.sprite = base.GetComponent<SpriteRenderer>();
		if (this.placedBlock.blockMetadata.Count <= 0)
		{
			this.placedBlock.blockMetadata.Add("1");
		}
	}

	// Token: 0x060000B8 RID: 184 RVA: 0x000055F4 File Offset: 0x000037F4
	private void Update()
	{
		if (this.placedBlock.blockMetadata[0] == "0")
		{
			this.hitbox.enabled = false;
			this.placedBlock.SetAnimationFrame(1);
		}
		else
		{
			this.hitbox.enabled = true;
			this.placedBlock.SetAnimationFrame(0);
		}
		if (!(GameManager.instance.playerMovement != null))
		{
			this.ChangeDoorState("1");
			return;
		}
		if (Vector2.Distance(base.transform.position, GameManager.instance.playerPos) < 2f)
		{
			this.ChangeDoorState("0");
			return;
		}
		this.ChangeDoorState("1");
	}

	// Token: 0x060000B9 RID: 185 RVA: 0x000056AC File Offset: 0x000038AC
	private void ChangeDoorState(string state)
	{
		if (this.placedBlock.blockMetadata[0] != state)
		{
			if (state == "0")
			{
				this.placedBlock.blockMetadata[0] = "0";
				AudioManager.instance.Play("DoorOpen");
				return;
			}
			this.placedBlock.blockMetadata[0] = "1";
			AudioManager.instance.Play("DoorClose");
		}
	}

	// Token: 0x040000D6 RID: 214
	private PlacedBlock placedBlock;

	// Token: 0x040000D7 RID: 215
	private BoxCollider2D hitbox;

	// Token: 0x040000D8 RID: 216
	private SpriteRenderer sprite;
}
