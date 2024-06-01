using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000007 RID: 7
[RequireComponent(typeof(EnemyBase), typeof(Rigidbody2D))]
public class FriendlyEnemy : MonoBehaviour
{
	// Token: 0x06000013 RID: 19 RVA: 0x00002430 File Offset: 0x00000630
	private void Start()
	{
		this.physicsComponent = base.GetComponent<Rigidbody2D>();
		this.enemyBase = base.GetComponent<EnemyBase>();
		this.anim = base.GetComponent<EnemyAnimator>();
		this.npcShop = base.GetComponent<NpcShop>();
		base.StartCoroutine("WalkCycle");
	}

	// Token: 0x06000014 RID: 20 RVA: 0x0000246E File Offset: 0x0000066E
	private IEnumerator WalkCycle()
	{
		for (;;)
		{
			yield return new WaitForSeconds((float)Random.Range(2, 6));
			this.isWalking = false;
			yield return new WaitForSeconds((float)Random.Range(5, 15));
			if (this.npcShop == null || GameManager.instance.npcShop != this.npcShop)
			{
				this.isWalking = true;
			}
		}
		yield break;
	}

	// Token: 0x06000015 RID: 21 RVA: 0x00002480 File Offset: 0x00000680
	private void Update()
	{
		if (this.isWalking && this.npcShop != null && GameManager.instance.npcShop == this.npcShop)
		{
			this.isWalking = false;
		}
		if (this.isWalking)
		{
			this.physicsComponent.velocity = (new Vector2(Mathf.Cos(this.enemyBase.ReturnAngleToTarget() * 0.0174532924f), Mathf.Sin(this.enemyBase.ReturnAngleToTarget() * 0.0174532924f)) + this.enemyBase.knockback) * this.speed;
			if (this.anim != null)
			{
				this.anim.SetAnimation(1);
				return;
			}
		}
		else
		{
			this.physicsComponent.velocity = Vector2.zero + this.enemyBase.knockback;
			if (this.anim != null)
			{
				this.anim.SetAnimation(0);
			}
		}
	}

	// Token: 0x0400000D RID: 13
	private Rigidbody2D physicsComponent;

	// Token: 0x0400000E RID: 14
	private EnemyBase enemyBase;

	// Token: 0x0400000F RID: 15
	private EnemyAnimator anim;

	// Token: 0x04000010 RID: 16
	public float speed = 2f;

	// Token: 0x04000011 RID: 17
	private bool isWalking = true;

	// Token: 0x04000012 RID: 18
	private NpcShop npcShop;
}
