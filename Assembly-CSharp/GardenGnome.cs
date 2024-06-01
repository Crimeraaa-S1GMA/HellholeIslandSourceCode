using System;
using UnityEngine;

// Token: 0x02000008 RID: 8
[RequireComponent(typeof(EnemyBase), typeof(Rigidbody2D))]
public class GardenGnome : MonoBehaviour
{
	// Token: 0x06000017 RID: 23 RVA: 0x0000258F File Offset: 0x0000078F
	private void Start()
	{
		this.physicsComponent = base.GetComponent<Rigidbody2D>();
		this.enemyBase = base.GetComponent<EnemyBase>();
		this.anim = base.GetComponent<EnemyAnimator>();
		base.InvokeRepeating("Heal", 0.24f, 0.24f);
	}

	// Token: 0x06000018 RID: 24 RVA: 0x000025CC File Offset: 0x000007CC
	private void Heal()
	{
		if (this.enemyBase.health < this.enemyBase.ReturnMaxHealth() * 0.4f)
		{
			this.enemyBase.health = Mathf.Clamp(this.enemyBase.health + 1f, 0f, this.enemyBase.ReturnMaxHealth());
		}
	}

	// Token: 0x06000019 RID: 25 RVA: 0x00002628 File Offset: 0x00000828
	private void Update()
	{
		if (this.enemyBase.health < this.enemyBase.ReturnMaxHealth() * 0.4f)
		{
			this.physicsComponent.velocity = Vector2.zero + this.enemyBase.knockback;
			if (this.anim != null)
			{
				this.anim.SetAnimation(1);
				return;
			}
		}
		else
		{
			this.physicsComponent.velocity = (new Vector2(Mathf.Cos(this.enemyBase.ReturnAngleToTarget() * 0.0174532924f), Mathf.Sin(this.enemyBase.ReturnAngleToTarget() * 0.0174532924f)) + this.enemyBase.knockback) * 3f;
			if (this.anim != null)
			{
				this.anim.SetAnimation(0);
			}
		}
	}

	// Token: 0x04000013 RID: 19
	private Rigidbody2D physicsComponent;

	// Token: 0x04000014 RID: 20
	private EnemyBase enemyBase;

	// Token: 0x04000015 RID: 21
	private EnemyAnimator anim;
}
