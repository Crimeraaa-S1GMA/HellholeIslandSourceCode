using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200007B RID: 123
public class Projectile : MonoBehaviour
{
	// Token: 0x06000244 RID: 580 RVA: 0x00012DDE File Offset: 0x00010FDE
	private void Start()
	{
		this.physicsBody = base.GetComponent<Rigidbody2D>();
		Object.Destroy(base.gameObject, this.lifetime);
	}

	// Token: 0x06000245 RID: 581 RVA: 0x00012E00 File Offset: 0x00011000
	private void Update()
	{
		if (this.homing && GameManager.instance.enemyList.Count > 0)
		{
			if (this.homingTarget != null)
			{
				this.direction = Vector2.Lerp(this.direction, (this.homingTarget.transform.position - base.transform.position).normalized, this.homingSpeed);
			}
			else
			{
				this.FindEnemyTarget();
			}
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

	// Token: 0x06000246 RID: 582 RVA: 0x00012ED4 File Offset: 0x000110D4
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Block") && !this.penetrateWalls)
		{
			AudioManager.instance.Play("ProjectileHit");
			this.IterDrops();
			if (this.explode)
			{
				this.Explode();
			}
			Object.Destroy(base.gameObject);
		}
		if (collision.CompareTag("Enemy"))
		{
			this.homingTarget = null;
			collision.gameObject.GetComponent<EnemyBase>().TakeDamage(this.damage + GameManager.instance.extraDamage, true, false, -this.direction.normalized, this.damageSource);
			if (this.explode)
			{
				this.Explode();
			}
			if (!this.pierce && !GameManager.instance.EquippedAccesory("sussy_quiver"))
			{
				this.IterDrops();
				Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x06000247 RID: 583 RVA: 0x00012FAC File Offset: 0x000111AC
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Enemy"))
		{
			this.homingTarget = null;
			collision.gameObject.GetComponent<EnemyBase>().TakeDamage(this.damage + GameManager.instance.extraDamage, true, false, -this.direction.normalized, this.damageSource);
		}
	}

	// Token: 0x06000248 RID: 584 RVA: 0x00013008 File Offset: 0x00011208
	private void FindEnemyTarget()
	{
		EnemyBase x = null;
		float num = -1f;
		foreach (EnemyBase enemyBase in GameManager.instance.enemyList)
		{
			if (x == null || Vector2.Distance(base.transform.position, enemyBase.transform.position) < num)
			{
				num = Vector2.Distance(base.transform.position, enemyBase.transform.position);
				x = enemyBase;
			}
		}
		this.homingTarget = x;
	}

	// Token: 0x06000249 RID: 585 RVA: 0x000130C0 File Offset: 0x000112C0
	public void Explode()
	{
		foreach (Explosives explosives in Object.FindObjectsOfType<Explosives>())
		{
			if (Vector2.Distance(base.transform.position, explosives.transform.position) < 3f)
			{
				explosives.Invoke("Explode", 0.1f);
			}
		}
		if (this.explosionDestroyBlocks)
		{
			foreach (PlacedBlock placedBlock in GameManager.instance.placedBlocks)
			{
				if (!placedBlock.ReturnBlock().immuneToExplosions && Vector2.Distance(base.transform.position, placedBlock.transform.position) < 3f)
				{
					placedBlock.actualBlockHealth = 0f;
					placedBlock.CheckForDeletion();
				}
			}
			foreach (PlacedFloor placedFloor in GameManager.instance.placedFloors)
			{
				if (!placedFloor.ReturnFloor().immuneToExplosions && Vector2.Distance(base.transform.position, placedFloor.transform.position) < 3f)
				{
					placedFloor.actualWallHealth = 0f;
					placedFloor.CheckForDeletion();
				}
			}
			foreach (PlacedTree placedTree in GameManager.instance.placedTrees)
			{
				if (Vector2.Distance(base.transform.position, placedTree.transform.position) < 3f)
				{
					placedTree.actualTreeHealth = 0f;
					placedTree.CheckForDeletion();
				}
			}
		}
		foreach (EnemyBase enemyBase in GameManager.instance.enemyList)
		{
			if (Vector2.Distance(base.transform.position, enemyBase.transform.position) < 3f)
			{
				enemyBase.TakeDamage(this.explosionDamage, true, false, Vector2.zero, DamageSource.ExplosionPlayerProjectile);
			}
		}
		if (Vector2.Distance(base.transform.position, GameManager.instance.playerPos) < 3f && GameManager.instance.playerMovement != null)
		{
			GameManager.instance.DealDamage((int)this.explosionDamage, DamageSource.ExplosionPlayerProjectile);
		}
		AudioManager.instance.Play("Explosion");
		ParticleManager.instance.SpawnParticle("explosion", base.transform.position);
	}

	// Token: 0x0600024A RID: 586 RVA: 0x000133CC File Offset: 0x000115CC
	private void IterDrops()
	{
		foreach (LootDrop lootDrop in this.drops)
		{
			if (UtilityMath.RandomChance(1f / (lootDrop.DROP_CHANCE - 1f)) && lootDrop.DROP_ID != "null" && lootDrop.DROP_STACK_MIN != 0 && lootDrop.DROP_STACK_MAX != 0)
			{
				if (GameManager.instance.EquippedAccesory("dice") && Random.Range(1, 4) == 1)
				{
					GameManager.instance.InitializePickupItem(base.transform.position + new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f)), lootDrop.DROP_ID, Random.Range(lootDrop.DROP_STACK_MIN, lootDrop.DROP_STACK_MAX) * 2, true);
				}
				else
				{
					GameManager.instance.InitializePickupItem(base.transform.position + new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f)), lootDrop.DROP_ID, Random.Range(lootDrop.DROP_STACK_MIN, lootDrop.DROP_STACK_MAX), true);
				}
			}
		}
	}

	// Token: 0x04000315 RID: 789
	[SerializeField]
	private float projectileSpeed = 4f;

	// Token: 0x04000316 RID: 790
	[SerializeField]
	private float damage = 5f;

	// Token: 0x04000317 RID: 791
	[SerializeField]
	private float lifetime = 15f;

	// Token: 0x04000318 RID: 792
	[SerializeField]
	private bool rotate;

	// Token: 0x04000319 RID: 793
	[SerializeField]
	private bool penetrateWalls;

	// Token: 0x0400031A RID: 794
	[SerializeField]
	private bool pierce;

	// Token: 0x0400031B RID: 795
	[SerializeField]
	private float explosionDamage = 45f;

	// Token: 0x0400031C RID: 796
	[SerializeField]
	private bool explode;

	// Token: 0x0400031D RID: 797
	[SerializeField]
	private bool explosionDestroyBlocks = true;

	// Token: 0x0400031E RID: 798
	[SerializeField]
	private bool homing;

	// Token: 0x0400031F RID: 799
	[SerializeField]
	private float homingSpeed = 0.08f;

	// Token: 0x04000320 RID: 800
	[SerializeField]
	private DamageSource damageSource = DamageSource.PlayerProjectileGeneric;

	// Token: 0x04000321 RID: 801
	[HideInInspector]
	public Vector2 direction;

	// Token: 0x04000322 RID: 802
	[SerializeField]
	private List<LootDrop> drops = new List<LootDrop>();

	// Token: 0x04000323 RID: 803
	private Rigidbody2D physicsBody;

	// Token: 0x04000324 RID: 804
	private EnemyBase homingTarget;
}
