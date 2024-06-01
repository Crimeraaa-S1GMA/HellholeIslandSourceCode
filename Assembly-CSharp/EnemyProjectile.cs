using System;
using UnityEngine;

// Token: 0x0200003C RID: 60
public class EnemyProjectile : MonoBehaviour
{
	// Token: 0x060000F8 RID: 248 RVA: 0x00006BE4 File Offset: 0x00004DE4
	private void Start()
	{
		if (WorldPolicy.hardcoreMode)
		{
			this.damage = (float)((int)(this.damage * 1.7f));
		}
		if (WorldPolicy.hellholeMode)
		{
			this.damage = (float)((int)(this.damage * 2.1f));
		}
		if (WorldPolicy.trainwreckMode)
		{
			this.damage = (float)((int)(this.damage * 2.2f));
		}
		this.physicsBody = base.GetComponent<Rigidbody2D>();
		Object.Destroy(base.gameObject, this.lifetime);
		base.InvokeRepeating("FlipDirection", this.rotateFlipSpeed, this.rotateFlipSpeed);
	}

	// Token: 0x060000F9 RID: 249 RVA: 0x00006C78 File Offset: 0x00004E78
	private void Update()
	{
		if (this.homing && GameManager.instance.playerMovement != null)
		{
			this.direction = Vector2.Lerp(this.direction, (GameManager.instance.playerPos - base.transform.position).normalized, this.homingSpeed);
		}
		if (this.rotateTowardsPlayer && GameManager.instance.playerMovement != null)
		{
			this.RotateTowardsPlayer(this.rotateSpeed);
		}
		if (this.physicsBody != null)
		{
			this.physicsBody.velocity = this.direction.normalized * this.projectileSpeed;
		}
		if (this.rotate)
		{
			base.transform.right = this.direction;
		}
	}

	// Token: 0x060000FA RID: 250 RVA: 0x00006D50 File Offset: 0x00004F50
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Block") && !this.penetrateWalls)
		{
			AudioManager.instance.Play("ProjectileHit");
			if (this.bouncy)
			{
				this.direction *= -1f;
			}
			else
			{
				Object.Destroy(base.gameObject);
			}
		}
		if (collision.CompareTag("Player"))
		{
			if (GameManager.instance.invinciblityFrame <= 0f)
			{
				GameManager.instance.DealDamage((int)this.damage, this.damageSource);
			}
			if (!this.pierce)
			{
				Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x060000FB RID: 251 RVA: 0x00006DF4 File Offset: 0x00004FF4
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (this.pierce && collision.CompareTag("Player") && GameManager.instance.invinciblityFrame <= 0f)
		{
			GameManager.instance.DealDamage((int)this.damage, this.damageSource);
		}
	}

	// Token: 0x060000FC RID: 252 RVA: 0x00006E34 File Offset: 0x00005034
	private void RotateTowardsPlayer(float speed)
	{
		Vector2 normalized = this.direction.normalized;
		float num = Mathf.Atan2(normalized.y, normalized.x) * 57.29578f;
		num += (float)this.rotateDirection * speed * Time.deltaTime;
		Vector2 vector = new Vector2(Mathf.Cos(num * 0.0174532924f), Mathf.Sin(num * 0.0174532924f));
		this.direction = vector;
	}

	// Token: 0x060000FD RID: 253 RVA: 0x00006E9D File Offset: 0x0000509D
	private void FlipDirection()
	{
		this.rotateDirection *= -1;
	}

	// Token: 0x0400011B RID: 283
	public float projectileSpeed = 4f;

	// Token: 0x0400011C RID: 284
	[SerializeField]
	private float damage = 5f;

	// Token: 0x0400011D RID: 285
	[SerializeField]
	private bool rotate;

	// Token: 0x0400011E RID: 286
	[SerializeField]
	private bool penetrateWalls;

	// Token: 0x0400011F RID: 287
	[SerializeField]
	private bool bouncy;

	// Token: 0x04000120 RID: 288
	[SerializeField]
	private bool homing;

	// Token: 0x04000121 RID: 289
	[SerializeField]
	private bool pierce;

	// Token: 0x04000122 RID: 290
	[SerializeField]
	private float homingSpeed = 0.08f;

	// Token: 0x04000123 RID: 291
	[SerializeField]
	private bool rotateTowardsPlayer;

	// Token: 0x04000124 RID: 292
	[SerializeField]
	private float rotateSpeed = 1f;

	// Token: 0x04000125 RID: 293
	[SerializeField]
	private float rotateFlipSpeed = 4f;

	// Token: 0x04000126 RID: 294
	[SerializeField]
	private float lifetime = 15f;

	// Token: 0x04000127 RID: 295
	[SerializeField]
	private DamageSource damageSource = DamageSource.EnemyProjectileGeneric;

	// Token: 0x04000128 RID: 296
	[HideInInspector]
	public Vector2 direction;

	// Token: 0x04000129 RID: 297
	private Rigidbody2D physicsBody;

	// Token: 0x0400012A RID: 298
	private int rotateDirection = 1;
}
