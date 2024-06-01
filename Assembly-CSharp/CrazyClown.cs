using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000005 RID: 5
[RequireComponent(typeof(EnemyBase), typeof(Rigidbody2D))]
public class CrazyClown : MonoBehaviour
{
	// Token: 0x0600000B RID: 11 RVA: 0x000022C8 File Offset: 0x000004C8
	private void Start()
	{
		this.physicsComponent = base.GetComponent<Rigidbody2D>();
		this.enemyBase = base.GetComponent<EnemyBase>();
		this.anim = base.GetComponent<EnemyAnimator>();
		this.enemyBase.OnEnemyDeath += this.DeadAchievement;
		base.StartCoroutine("WalkCycle");
	}

	// Token: 0x0600000C RID: 12 RVA: 0x0000231C File Offset: 0x0000051C
	private void DeadAchievement()
	{
		AchievementManager.instance.AddAchievement(176352);
	}

	// Token: 0x0600000D RID: 13 RVA: 0x0000232D File Offset: 0x0000052D
	private IEnumerator WalkCycle()
	{
		for (;;)
		{
			yield return new WaitForSeconds(3f);
			if (GameManager.instance.playerMovement != null)
			{
				int num;
				for (int i = 0; i < 3; i = num + 1)
				{
					yield return new WaitForSeconds(3.5f);
					this.clownBombPosition = GameManager.instance.playerPos;
					yield return new WaitForSeconds(0.5f);
					AudioManager.instance.Play("ClownLaugh");
					Object.FindObjectOfType<EnemySpawner>().SpawnEnemyAtPosition("clownlandmine", this.clownBombPosition);
					this.isWalking = false;
					num = i;
				}
			}
			yield return new WaitForSeconds(4f);
			this.isWalking = true;
		}
		yield break;
	}

	// Token: 0x0600000E RID: 14 RVA: 0x0000233C File Offset: 0x0000053C
	private void Update()
	{
		if (this.isWalking)
		{
			this.physicsComponent.velocity = (new Vector2(Mathf.Cos(this.enemyBase.ReturnAngleToTarget() * 0.0174532924f), Mathf.Sin(this.enemyBase.ReturnAngleToTarget() * 0.0174532924f)) + this.enemyBase.knockback) * 2f;
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

	// Token: 0x04000006 RID: 6
	private Rigidbody2D physicsComponent;

	// Token: 0x04000007 RID: 7
	private EnemyBase enemyBase;

	// Token: 0x04000008 RID: 8
	private EnemyAnimator anim;

	// Token: 0x04000009 RID: 9
	private bool isWalking = true;

	// Token: 0x0400000A RID: 10
	private Vector2 clownBombPosition;
}
