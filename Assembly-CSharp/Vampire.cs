using System;
using UnityEngine;

// Token: 0x020000A4 RID: 164
public class Vampire : MonoBehaviour
{
	// Token: 0x0600031F RID: 799 RVA: 0x00017C20 File Offset: 0x00015E20
	private void Start()
	{
		this.physicsComponent = base.GetComponent<Rigidbody2D>();
		this.enemyBase = base.GetComponent<EnemyBase>();
		this.enemyAnimator = base.GetComponent<EnemyAnimator>();
		this.capsule = base.GetComponent<CapsuleCollider2D>();
		this.enemyBase.OnPlayerCollision += this.AttachToPlayer;
	}

	// Token: 0x06000320 RID: 800 RVA: 0x00017C74 File Offset: 0x00015E74
	private void Update()
	{
		if (GameManager.instance.playerMovement != null)
		{
			this.capsule.enabled = (GameManager.instance.playerMovement.vampire == null);
			if (GameManager.instance.playerMovement.vampire != null)
			{
				base.transform.position = GameManager.instance.playerPos - Vector2.right * 0.7f;
				this.physicsComponent.velocity = Vector2.zero;
				this.enemyAnimator.SetAnimation(1);
				GameManager.instance.DealDamage(5, DamageSource.VampireBite);
				return;
			}
			this.physicsComponent.velocity = (new Vector2(Mathf.Cos(this.enemyBase.ReturnAngleToTarget() * 0.0174532924f), Mathf.Sin(this.enemyBase.ReturnAngleToTarget() * 0.0174532924f)) + this.enemyBase.knockback) * 1.8f;
			this.enemyAnimator.SetAnimation(0);
		}
	}

	// Token: 0x06000321 RID: 801 RVA: 0x00017D87 File Offset: 0x00015F87
	private void AttachToPlayer()
	{
		if (GameManager.instance.playerMovement != null)
		{
			GameManager.instance.playerMovement.vampire = this;
		}
	}

	// Token: 0x06000322 RID: 802 RVA: 0x00017DAB File Offset: 0x00015FAB
	public void Death()
	{
		this.enemyBase.TakeDamage(420f, true, false, Vector2.zero, DamageSource.VampirePushOff);
	}

	// Token: 0x040003D8 RID: 984
	private Rigidbody2D physicsComponent;

	// Token: 0x040003D9 RID: 985
	private EnemyBase enemyBase;

	// Token: 0x040003DA RID: 986
	private EnemyAnimator enemyAnimator;

	// Token: 0x040003DB RID: 987
	private CapsuleCollider2D capsule;
}
