using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200000A RID: 10
[RequireComponent(typeof(EnemyBase))]
public class Jeff : MonoBehaviour
{
	// Token: 0x06000023 RID: 35 RVA: 0x00002830 File Offset: 0x00000A30
	private void Start()
	{
		this.enemyBase = base.GetComponent<EnemyBase>();
		this.enemyAnim = base.GetComponent<EnemyAnimator>();
		this.physicsComponent = base.GetComponent<Rigidbody2D>();
		this.enemyBase.OnEnemyDeath += this.DeadAchievement;
		base.StartCoroutine("BossCoroutine");
	}

	// Token: 0x06000024 RID: 36 RVA: 0x00002884 File Offset: 0x00000A84
	private void Update()
	{
		if (this.isBeamMode)
		{
			this.physicsComponent.velocity = Vector2.zero;
			this.enemyAnim.SetAnimation(1);
			return;
		}
		this.physicsComponent.velocity = (new Vector2(Mathf.Cos(this.enemyBase.ReturnAngleToTarget() * 0.0174532924f), Mathf.Sin(this.enemyBase.ReturnAngleToTarget() * 0.0174532924f)) + this.enemyBase.knockback) * (this.IsSecondPhase() ? 7f : 4f);
		this.enemyAnim.SetAnimation(0);
	}

	// Token: 0x06000025 RID: 37 RVA: 0x00002927 File Offset: 0x00000B27
	private void DeadAchievement()
	{
		AchievementManager.instance.AddAchievement(156196);
	}

	// Token: 0x06000026 RID: 38 RVA: 0x00002938 File Offset: 0x00000B38
	private IEnumerator BossCoroutine()
	{
		for (;;)
		{
			if (GameManager.instance.playerMovement != null)
			{
				this.isBeamMode = false;
				if (this.IsSecondPhase() || WorldPolicy.hellholeMode)
				{
					yield return new WaitForSeconds(1.5f);
					this.UkrainiumPattern(5, 90f);
					yield return new WaitForSeconds(1.5f);
					this.UkrainiumPattern(10, 90f);
					yield return new WaitForSeconds(1.5f);
					this.UkrainiumPattern(12, 45f);
					yield return new WaitForSeconds(1.5f);
				}
				else
				{
					yield return new WaitForSeconds(3f);
					this.UkrainiumPattern(6, 90f);
					yield return new WaitForSeconds(3f);
					this.UkrainiumPattern(6, 45f);
					yield return new WaitForSeconds(3f);
					this.UkrainiumPattern(6, 90f);
					yield return new WaitForSeconds(3f);
					this.UkrainiumPattern(6, 45f);
				}
				this.beamAngle = new Vector2(Mathf.Cos(this.enemyBase.ReturnAngleToTarget() * 0.0174532924f), Mathf.Sin(this.enemyBase.ReturnAngleToTarget() * 0.0174532924f));
				yield return new WaitForSeconds(2f);
				this.isBeamMode = true;
				this.UkrainiumBeam();
				yield return new WaitForSeconds(10f);
				this.isBeamMode = false;
			}
			else
			{
				this.isBeamMode = false;
			}
		}
		yield break;
	}

	// Token: 0x06000027 RID: 39 RVA: 0x00002948 File Offset: 0x00000B48
	private void UkrainiumPattern(int projectiles, float startAngle)
	{
		float num = startAngle;
		for (int i = 0; i < projectiles; i++)
		{
			EnemyProjectile component = Object.Instantiate<GameObject>(this.ukrainium).GetComponent<EnemyProjectile>();
			component.direction = new Vector2(Mathf.Cos(num * 0.0174532924f), Mathf.Sin(num * 0.0174532924f));
			component.transform.position = base.transform.position + component.direction * 1f;
			num += 360f / (float)projectiles;
		}
	}

	// Token: 0x06000028 RID: 40 RVA: 0x000029D7 File Offset: 0x00000BD7
	private void UkrainiumBeam()
	{
		Object.Instantiate<GameObject>(this.ukrainiumBeam, base.transform).GetComponent<EnemyProjectile>().direction = this.beamAngle;
	}

	// Token: 0x06000029 RID: 41 RVA: 0x000029FA File Offset: 0x00000BFA
	private bool IsSecondPhase()
	{
		return this.enemyBase.health / this.enemyBase.ReturnMaxHealth() <= 0.3f;
	}

	// Token: 0x04000018 RID: 24
	public GameObject ukrainium;

	// Token: 0x04000019 RID: 25
	public GameObject ukrainiumBeam;

	// Token: 0x0400001A RID: 26
	private EnemyBase enemyBase;

	// Token: 0x0400001B RID: 27
	private EnemyAnimator enemyAnim;

	// Token: 0x0400001C RID: 28
	private Rigidbody2D physicsComponent;

	// Token: 0x0400001D RID: 29
	private Vector2 beamAngle;

	// Token: 0x0400001E RID: 30
	private bool isBeamMode;
}
