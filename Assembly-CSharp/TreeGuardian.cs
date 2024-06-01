using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000012 RID: 18
[RequireComponent(typeof(EnemyBase), typeof(Rigidbody2D))]
public class TreeGuardian : MonoBehaviour
{
	// Token: 0x06000049 RID: 73 RVA: 0x00003228 File Offset: 0x00001428
	private void Start()
	{
		this.physicsComponent = base.GetComponent<Rigidbody2D>();
		this.enemyBase = base.GetComponent<EnemyBase>();
		this.anim = base.GetComponent<EnemyAnimator>();
		this.enemyBase.OnEnemyDeath += this.DeadAchievement;
		base.StartCoroutine("WalkCycle");
		base.StartCoroutine("Dash");
		base.StartCoroutine("Projectile");
		base.StartCoroutine("PlankSpawn");
	}

	// Token: 0x0600004A RID: 74 RVA: 0x000032A0 File Offset: 0x000014A0
	private void DeadAchievement()
	{
		AchievementManager.instance.AddAchievement(159732);
	}

	// Token: 0x0600004B RID: 75 RVA: 0x000032B4 File Offset: 0x000014B4
	private void PlankPattern(int planks, float startAngle)
	{
		float num = startAngle;
		for (int i = 0; i < planks; i++)
		{
			EnemyProjectile component = Object.Instantiate<GameObject>(this.plankProjectile).GetComponent<EnemyProjectile>();
			component.direction = new Vector2(Mathf.Cos(num * 0.0174532924f), Mathf.Sin(num * 0.0174532924f));
			component.transform.position = base.transform.position + component.direction * 1f;
			num += 360f / (float)planks;
		}
	}

	// Token: 0x0600004C RID: 76 RVA: 0x00003343 File Offset: 0x00001543
	private IEnumerator PlankSpawn()
	{
		for (;;)
		{
			if (this.IsSecondPhase() || WorldPolicy.hellholeMode)
			{
				yield return new WaitForSeconds(1.5f);
				this.PlankPattern(6, 90f);
				yield return new WaitForSeconds(1.5f);
				this.PlankPattern(9, 90f);
				yield return new WaitForSeconds(1.5f);
				this.PlankPattern(12, 45f);
				yield return new WaitForSeconds(1.5f);
			}
			else
			{
				yield return new WaitForSeconds(3f);
				this.PlankPattern(4, 90f);
				yield return new WaitForSeconds(3f);
				this.PlankPattern(4, 45f);
				yield return new WaitForSeconds(3f);
				this.PlankPattern(7, 90f);
			}
		}
		yield break;
	}

	// Token: 0x0600004D RID: 77 RVA: 0x00003352 File Offset: 0x00001552
	private IEnumerator WalkCycle()
	{
		for (;;)
		{
			yield return new WaitForSeconds(9f);
			this.isWalking = false;
			yield return new WaitForSeconds(3f);
			this.isWalking = true;
		}
		yield break;
	}

	// Token: 0x0600004E RID: 78 RVA: 0x00003361 File Offset: 0x00001561
	private bool IsSecondPhase()
	{
		return this.enemyBase.health / this.enemyBase.ReturnMaxHealth() <= 0.4f;
	}

	// Token: 0x0600004F RID: 79 RVA: 0x00003384 File Offset: 0x00001584
	private IEnumerator Dash()
	{
		for (;;)
		{
			yield return new WaitForSeconds(5f);
			if (this.IsSecondPhase())
			{
				this.speed = 15.5f;
			}
		}
		yield break;
	}

	// Token: 0x06000050 RID: 80 RVA: 0x00003393 File Offset: 0x00001593
	private IEnumerator Projectile()
	{
		for (;;)
		{
			if (this.isWalking)
			{
				if (this.IsSecondPhase())
				{
					yield return new WaitForSeconds(2.5f);
				}
				else
				{
					yield return new WaitForSeconds(4f);
				}
			}
			else
			{
				yield return new WaitForSeconds(0.5f);
			}
			GameObject gameObject = Object.Instantiate<GameObject>(this.leafProjectile);
			if (this.IsSecondPhase())
			{
				gameObject.transform.position = GameManager.instance.playerPos + new Vector2((float)(7 * UtilityMath.NegativePositiveOne()), (float)(7 * UtilityMath.NegativePositiveOne()));
			}
			else
			{
				gameObject.transform.position = GameManager.instance.playerPos + new Vector2((float)(6 * UtilityMath.NegativePositiveOne()), (float)(6 * UtilityMath.NegativePositiveOne()));
			}
		}
		yield break;
	}

	// Token: 0x06000051 RID: 81 RVA: 0x000033A4 File Offset: 0x000015A4
	private void Update()
	{
		if (this.isWalking)
		{
			this.physicsComponent.velocity = (new Vector2(Mathf.Cos(this.enemyBase.ReturnAngleToTarget() * 0.0174532924f), Mathf.Sin(this.enemyBase.ReturnAngleToTarget() * 0.0174532924f)) + this.enemyBase.knockback) * this.speed;
			if (this.anim != null)
			{
				this.anim.SetAnimation(1);
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
		this.UpdatePhase();
	}

	// Token: 0x06000052 RID: 82 RVA: 0x0000346C File Offset: 0x0000166C
	private void UpdatePhase()
	{
		if (this.IsSecondPhase())
		{
			this.speed = Mathf.SmoothDamp(this.speed, 4f, ref this.currentVel, 0.3f);
			return;
		}
		this.speed = Mathf.SmoothDamp(this.speed, 1.5f, ref this.currentVel, 0.3f);
	}

	// Token: 0x0400003B RID: 59
	private Rigidbody2D physicsComponent;

	// Token: 0x0400003C RID: 60
	private EnemyBase enemyBase;

	// Token: 0x0400003D RID: 61
	private EnemyAnimator anim;

	// Token: 0x0400003E RID: 62
	public float speed = 2f;

	// Token: 0x0400003F RID: 63
	private bool isWalking = true;

	// Token: 0x04000040 RID: 64
	private float currentVel;

	// Token: 0x04000041 RID: 65
	public GameObject leafProjectile;

	// Token: 0x04000042 RID: 66
	public GameObject plankProjectile;
}
