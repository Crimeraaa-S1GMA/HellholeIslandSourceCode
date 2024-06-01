using System;
using UnityEngine;

// Token: 0x02000013 RID: 19
[RequireComponent(typeof(EnemyBase), typeof(Rigidbody2D))]
public class WalkingEnemy : MonoBehaviour
{
	// Token: 0x06000054 RID: 84 RVA: 0x000034DE File Offset: 0x000016DE
	private void Start()
	{
		this.physicsComponent = base.GetComponent<Rigidbody2D>();
		this.enemyBase = base.GetComponent<EnemyBase>();
	}

	// Token: 0x06000055 RID: 85 RVA: 0x000034F8 File Offset: 0x000016F8
	private void Update()
	{
		if (this.tiltTowardsPlayer)
		{
			base.transform.right = new Vector2(Mathf.Cos(this.enemyBase.ReturnAngleToTarget() * 0.0174532924f), Mathf.Sin(this.enemyBase.ReturnAngleToTarget() * 0.0174532924f));
		}
		this.physicsComponent.velocity = (new Vector2(Mathf.Cos(this.enemyBase.ReturnAngleToTarget() * 0.0174532924f), Mathf.Sin(this.enemyBase.ReturnAngleToTarget() * 0.0174532924f)) + this.enemyBase.knockback) * (this.speed + (this.isOnSlimeFloor ? 2f : 0f));
	}

	// Token: 0x06000056 RID: 86 RVA: 0x000035BA File Offset: 0x000017BA
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("SlimeFloor"))
		{
			this.isOnSlimeFloor = true;
		}
	}

	// Token: 0x06000057 RID: 87 RVA: 0x000035D0 File Offset: 0x000017D0
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("SlimeFloor"))
		{
			this.isOnSlimeFloor = true;
		}
	}

	// Token: 0x06000058 RID: 88 RVA: 0x000035E6 File Offset: 0x000017E6
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("SlimeFloor"))
		{
			this.isOnSlimeFloor = false;
		}
	}

	// Token: 0x04000043 RID: 67
	private Rigidbody2D physicsComponent;

	// Token: 0x04000044 RID: 68
	private EnemyBase enemyBase;

	// Token: 0x04000045 RID: 69
	public float speed = 2f;

	// Token: 0x04000046 RID: 70
	public bool tiltTowardsPlayer;

	// Token: 0x04000047 RID: 71
	public bool isSlime;

	// Token: 0x04000048 RID: 72
	private bool isOnSlimeFloor;
}
