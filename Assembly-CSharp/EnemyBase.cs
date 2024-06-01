using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000038 RID: 56
public class EnemyBase : MonoBehaviour
{
	// Token: 0x14000001 RID: 1
	// (add) Token: 0x060000CD RID: 205 RVA: 0x00005A74 File Offset: 0x00003C74
	// (remove) Token: 0x060000CE RID: 206 RVA: 0x00005AAC File Offset: 0x00003CAC
	public event Action OnEnemyDeath;

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x060000CF RID: 207 RVA: 0x00005AE4 File Offset: 0x00003CE4
	// (remove) Token: 0x060000D0 RID: 208 RVA: 0x00005B1C File Offset: 0x00003D1C
	public event Action<DamageSource> OnEnemyDamaged;

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x060000D1 RID: 209 RVA: 0x00005B54 File Offset: 0x00003D54
	// (remove) Token: 0x060000D2 RID: 210 RVA: 0x00005B8C File Offset: 0x00003D8C
	public event Action OnPlayerCollision;

	// Token: 0x060000D3 RID: 211 RVA: 0x00005BC4 File Offset: 0x00003DC4
	private void Start()
	{
		if (WorldPolicy.hardcoreMode || GameManager.instance.EquippedAccesory("ring_of_doom"))
		{
			this.maxHealth *= 2f;
		}
		if (WorldPolicy.hellholeMode)
		{
			this.maxHealth *= 2f;
			this.dealtDamage *= 2;
		}
		if (WorldPolicy.trainwreckMode)
		{
			this.maxHealth *= 3f;
			this.dealtDamage *= 3;
		}
		this.health = this.maxHealth;
		if (this.countsToCap)
		{
			GameManager.instance.enemiesInWorld++;
		}
		GameManager.instance.enemyList.Add(this);
		base.InvokeRepeating("ChangeWanderDirection", 0f, 3f);
		if (!GameManager.instance.nostalgiaSeed && GameManager.instance.enemyHealthBars)
		{
			this.CreateHealthBar();
		}
		base.gameObject.layer = 7;
	}

	// Token: 0x060000D4 RID: 212 RVA: 0x00005CBE File Offset: 0x00003EBE
	private void CreateHealthBar()
	{
		Object.Instantiate<GameObject>(GameManager.instance.enemyBar).GetComponent<EnemyBar>().SetEnemy(this);
	}

	// Token: 0x060000D5 RID: 213 RVA: 0x00005CDA File Offset: 0x00003EDA
	private void ChangeWanderDirection()
	{
		this.wander = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
	}

	// Token: 0x060000D6 RID: 214 RVA: 0x00005D08 File Offset: 0x00003F08
	private void Update()
	{
		if (this.canSeePlayer && this.doTargetPlayer)
		{
			this.toTarget = (GameManager.instance.playerPos - base.transform.position).normalized;
		}
		else
		{
			this.toTarget = Vector2.SmoothDamp(this.toTarget, this.wander, ref this.currentVel, 0.2f);
		}
		if (this.milkable && this.milk && Input.GetMouseButtonDown(0) && GameManager.instance.ReturnSelectedSlot().ITEM_ID == "bottle")
		{
			GameManager.instance.RemoveCurrentHeldItemOne();
			GameManager.instance.InitializePickupItem(GameManager.instance.playerPos, "milk", 1, true);
		}
		this.canSeePlayer = this.UpdateCanSeePlayer();
		this.knockback = Vector2.SmoothDamp(this.knockback, Vector2.zero, ref this.kbVel, 0.2f);
		this.invinciblityFrame = Mathf.Max(this.invinciblityFrame - Time.deltaTime, 0f);
		this.glowStingEffect = Mathf.Max(this.glowStingEffect - Time.deltaTime, 0f);
		if (Vector2.Distance(base.transform.position, GameManager.instance.playerPos) > 60f && this.doDespawn)
		{
			Object.Destroy(base.gameObject);
		}
		this.health = Mathf.Clamp(Mathf.Round(this.health), 0f, this.maxHealth);
		this.dealtDamage = Mathf.Clamp(this.dealtDamage, 0, 10000);
		if (this.health <= 0f)
		{
			GameManager.instance.invasionEnemiesLeft--;
			GameManager.instance.tooltipCustom = "";
			AudioManager.instance.Play("KillEnemy");
			GameManager.instance.InitializePickupItem(base.transform.position + new Vector2(Random.Range(-2f, 1f), Random.Range(-2f, 1f)), "money", Random.Range(this.moneyMin, this.moneyMax), true);
			if (this.achievement)
			{
				AchievementManager.instance.AddAchievement(156204);
				if (WorldPolicy.trainwreckMode)
				{
					AchievementManager.instance.AddAchievement(179825);
				}
			}
			if (GameManager.instance.enemyCorpses)
			{
				this.SpawnCorpse();
			}
			this.IterDrops();
			if ((float)GameManager.instance.health < (float)GameManager.instance.ReturnMaxHealth() * 0.7f)
			{
				GameManager.instance.InitializePickupItem(base.transform.position + new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f)), "heart", GameManager.instance.HasStatusEffect("heart") ? 3 : 1, true);
			}
			if (this.OnEnemyDeath != null)
			{
				this.OnEnemyDeath();
			}
			Object.Destroy(base.gameObject);
		}
		this.UpdateGraphics();
	}

	// Token: 0x060000D7 RID: 215 RVA: 0x00006024 File Offset: 0x00004224
	private void UpdateGraphics()
	{
		foreach (SpriteRenderer spriteRenderer in this.flipped)
		{
			if (this.toTarget.x < 0f)
			{
				spriteRenderer.flipX = true;
			}
			else
			{
				spriteRenderer.flipX = false;
			}
		}
		foreach (SpriteRenderer spriteRenderer2 in this.glowStingAffected)
		{
			if (this.glowStingEffect > 0f)
			{
				spriteRenderer2.material = GameManager.instance.Unlit;
			}
			else
			{
				spriteRenderer2.material = GameManager.instance.Lit;
			}
		}
	}

	// Token: 0x060000D8 RID: 216 RVA: 0x000060B4 File Offset: 0x000042B4
	private bool UpdateCanSeePlayer()
	{
		if (GameManager.instance.bed != null || GameManager.instance.selectedTeleporter != null || (WorldPolicy.creativeMode && this.cantSeePlayerInCreative) || GameManager.instance.HasStatusEffect("invisiblity"))
		{
			return false;
		}
		RaycastHit2D raycastHit2D = Physics2D.Raycast(base.transform.position, GameManager.instance.playerPos - base.transform.position, float.PositiveInfinity, GameManager.instance.enemyLineOfSightMask);
		return raycastHit2D.collider != null && raycastHit2D.collider.CompareTag("Player");
	}

	// Token: 0x060000D9 RID: 217 RVA: 0x00006177 File Offset: 0x00004377
	public string ReturnEnemyName()
	{
		return this.enemyName;
	}

	// Token: 0x060000DA RID: 218 RVA: 0x0000617F File Offset: 0x0000437F
	public bool CanSeePlayer()
	{
		return this.canSeePlayer;
	}

	// Token: 0x060000DB RID: 219 RVA: 0x00006187 File Offset: 0x00004387
	public float ReturnAngleToTarget()
	{
		return Mathf.Atan2(this.toTarget.y, this.toTarget.x) * 57.29578f;
	}

	// Token: 0x060000DC RID: 220 RVA: 0x000061AA File Offset: 0x000043AA
	public Vector2 ReturnToTarget()
	{
		return this.toTarget;
	}

	// Token: 0x060000DD RID: 221 RVA: 0x000061B2 File Offset: 0x000043B2
	public float ReturnMaxHealth()
	{
		return this.maxHealth;
	}

	// Token: 0x060000DE RID: 222 RVA: 0x000061BA File Offset: 0x000043BA
	public int ReturnBossSoundtrackId()
	{
		return this.bossSoundtrackId;
	}

	// Token: 0x060000DF RID: 223 RVA: 0x000061C2 File Offset: 0x000043C2
	public void SetDealtDamage(int dmg)
	{
		this.dealtDamage = Mathf.Clamp(dmg, 0, 10000);
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x000061D6 File Offset: 0x000043D6
	public bool IsCollidingWithMeleeWeapon()
	{
		return this.itemCol;
	}

	// Token: 0x060000E1 RID: 225 RVA: 0x000061DE File Offset: 0x000043DE
	public void RemoveMeleeCollision()
	{
		this.itemCol = false;
	}

	// Token: 0x060000E2 RID: 226 RVA: 0x000061E8 File Offset: 0x000043E8
	private void SpawnCorpse()
	{
		GameObject gameObject = Object.Instantiate<GameObject>(GameManager.instance.enemyCorpse);
		gameObject.transform.position = base.transform.position;
		gameObject.transform.localScale = base.transform.localScale;
		gameObject.GetComponent<EnemyCorpse>().enemySprite = base.GetComponent<SpriteRenderer>().sprite;
		gameObject.GetComponent<EnemyCorpse>().enemyColor = base.GetComponent<SpriteRenderer>().color;
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x0000625C File Offset: 0x0000445C
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
		if (WorldPolicy.hellholeMode)
		{
			foreach (LootDrop lootDrop2 in this.hellholeModeDrops)
			{
				if (UtilityMath.RandomChance(1f / (lootDrop2.DROP_CHANCE - 1f)) && lootDrop2.DROP_ID != "null" && lootDrop2.DROP_STACK_MIN != 0 && lootDrop2.DROP_STACK_MAX != 0)
				{
					if (GameManager.instance.EquippedAccesory("dice") && Random.Range(1, 4) == 1)
					{
						GameManager.instance.InitializePickupItem(base.transform.position + new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f)), lootDrop2.DROP_ID, Random.Range(lootDrop2.DROP_STACK_MIN, lootDrop2.DROP_STACK_MAX) * 2, true);
					}
					else
					{
						GameManager.instance.InitializePickupItem(base.transform.position + new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f)), lootDrop2.DROP_ID, Random.Range(lootDrop2.DROP_STACK_MIN, lootDrop2.DROP_STACK_MAX), true);
					}
				}
			}
		}
		if (this.dropsKey && Random.Range(1, 16) == 1)
		{
			GameManager.instance.InitializePickupItem(base.transform.position + new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f)), "key", 1, true);
		}
		if (this.dropsKey && GameManager.instance.kyivSeed && Random.Range(1, 8) == 1)
		{
			GameManager.instance.InitializePickupItem(base.transform.position + new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f)), "ukrainium", Random.Range(30, 41), true);
		}
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x00006618 File Offset: 0x00004818
	public void TakeDamage(float takedPoints, bool isPlayerSource, bool ignoreCanNotBeHit, Vector2 kb, DamageSource damageSource)
	{
		if ((this.canBeHit || ignoreCanNotBeHit) && takedPoints > 0f && this.invinciblityFrame <= 0f)
		{
			AudioManager.instance.Play("HitEnemy");
			if ((this.vulnerablities.Count <= 0 || this.vulnerablities.Contains(damageSource)) && !this.immunities.Contains(damageSource))
			{
				bool flag = Random.Range(1, Mathf.Max(21 - GameManager.instance.extraCritChance, 1)) == 1 && isPlayerSource;
				if (flag)
				{
					this.knockback = kb.normalized * -5f * this.knockbackPercentage;
					if (GameManager.instance.EquippedAccesory("inferno_emblem") && !GameManager.instance.HasStatusEffect("inferno_emblem"))
					{
						GameManager.instance.AddStatusEffect("inferno_emblem");
					}
					ParticleManager.instance.SpawnParticle("critical", base.transform.position);
				}
				else
				{
					this.knockback = kb.normalized * -3f * this.knockbackPercentage;
				}
				if (!this.immuneToPlayerDamageSources || !isPlayerSource)
				{
					if (flag)
					{
						this.health -= Mathf.Clamp(takedPoints * 2f, 1f, 10000f);
					}
					else
					{
						this.health -= Mathf.Clamp(takedPoints, 1f, 10000f);
					}
				}
				if (damageSource == DamageSource.PlayerMeleeWeaponGlowSting)
				{
					this.glowStingEffect = 8f;
				}
				if (this.OnEnemyDamaged != null)
				{
					this.OnEnemyDamaged(damageSource);
				}
				this.invinciblityFrame = 0.3f;
			}
		}
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x000067C8 File Offset: 0x000049C8
	public void SetDoTargetPlayer(bool value)
	{
		this.doTargetPlayer = value;
	}

	// Token: 0x060000E6 RID: 230 RVA: 0x000067D1 File Offset: 0x000049D1
	private void OnCollisionEnter2D(Collision2D collision)
	{
		this.ChangeWanderDirection();
	}

	// Token: 0x060000E7 RID: 231 RVA: 0x000067D9 File Offset: 0x000049D9
	private void OnCollisionStay2D(Collision2D collision)
	{
		this.ChangeWanderDirection();
	}

	// Token: 0x060000E8 RID: 232 RVA: 0x000067E1 File Offset: 0x000049E1
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("ItemHand"))
		{
			this.itemCol = true;
		}
		if (collision.CompareTag("SelSquare"))
		{
			GameManager.instance.enemyBlockingBuild = true;
			this.milk = true;
		}
	}

	// Token: 0x060000E9 RID: 233 RVA: 0x00006818 File Offset: 0x00004A18
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("SelSquare"))
		{
			GameManager.instance.enemyBlockingBuild = true;
		}
		if (collision.CompareTag("Flamethrower"))
		{
			this.TakeDamage(1f + GameManager.instance.extraDamage, true, false, (base.transform.position - GameManager.instance.playerPos).normalized, DamageSource.Flamethrower);
		}
		if (collision.CompareTag("Player") && GameManager.instance.playerMovement != null)
		{
			if (GameManager.instance.playerMovement.IsDashing())
			{
				if (GameManager.instance.playerMovement.TryDashAttack())
				{
					this.TakeDamage(15f, true, false, GameManager.instance.playerMovement.dash.normalized, DamageSource.Dash);
					GameManager.instance.playerMovement.BounceOffDash();
				}
			}
			else
			{
				if (this.OnPlayerCollision != null)
				{
					this.OnPlayerCollision();
				}
				GameManager.instance.DealDamage(this.dealtDamage, DamageSource.EnemyMeleeAttack);
			}
		}
		if (collision.CompareTag("Campfire"))
		{
			this.TakeDamage(5f, true, false, Vector2.zero, DamageSource.Campfire);
		}
		if (collision.CompareTag("FireRing"))
		{
			this.TakeDamage(2f, true, false, Vector2.zero, DamageSource.InfernoEmblemFire);
		}
	}

	// Token: 0x060000EA RID: 234 RVA: 0x00006969 File Offset: 0x00004B69
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("ItemHand"))
		{
			this.itemCol = false;
		}
		if (collision.CompareTag("SelSquare"))
		{
			GameManager.instance.enemyBlockingBuild = false;
			this.milk = false;
		}
	}

	// Token: 0x060000EB RID: 235 RVA: 0x0000699E File Offset: 0x00004B9E
	private void OnMouseEnter()
	{
		GameManager.instance.tooltipCustom = string.Format("{0} {1}/{2}", this.enemyName, this.health, this.maxHealth);
	}

	// Token: 0x060000EC RID: 236 RVA: 0x000069D0 File Offset: 0x00004BD0
	private void OnMouseOver()
	{
		GameManager.instance.tooltipCustom = string.Format("{0} {1}/{2}", this.enemyName, this.health, this.maxHealth);
	}

	// Token: 0x060000ED RID: 237 RVA: 0x00006A02 File Offset: 0x00004C02
	private void OnMouseExit()
	{
		GameManager.instance.tooltipCustom = "";
	}

	// Token: 0x060000EE RID: 238 RVA: 0x00006A13 File Offset: 0x00004C13
	private void OnDestroy()
	{
		if (this.countsToCap)
		{
			GameManager.instance.enemiesInWorld--;
		}
		GameManager.instance.enemyList.Remove(this);
	}

	// Token: 0x040000EB RID: 235
	[SerializeField]
	private string enemyName = "";

	// Token: 0x040000EC RID: 236
	[SerializeField]
	private float maxHealth = 10f;

	// Token: 0x040000ED RID: 237
	public float health = 1f;

	// Token: 0x040000EE RID: 238
	[SerializeField]
	private float knockbackPercentage = 1f;

	// Token: 0x040000EF RID: 239
	[SerializeField]
	private int dealtDamage;

	// Token: 0x040000F0 RID: 240
	[SerializeField]
	private List<LootDrop> drops = new List<LootDrop>();

	// Token: 0x040000F1 RID: 241
	[SerializeField]
	private List<LootDrop> hellholeModeDrops = new List<LootDrop>();

	// Token: 0x040000F2 RID: 242
	[SerializeField]
	private bool canBeHit = true;

	// Token: 0x040000F3 RID: 243
	[SerializeField]
	private bool doDespawn = true;

	// Token: 0x040000F4 RID: 244
	[Header("Leave empty to accept all damage sources")]
	[SerializeField]
	private List<DamageSource> vulnerablities = new List<DamageSource>();

	// Token: 0x040000F5 RID: 245
	[SerializeField]
	private List<DamageSource> immunities = new List<DamageSource>();

	// Token: 0x040000F6 RID: 246
	[SerializeField]
	private bool immuneToPlayerDamageSources;

	// Token: 0x040000F7 RID: 247
	[SerializeField]
	private int bossSoundtrackId = 5;

	// Token: 0x040000F8 RID: 248
	[SerializeField]
	private SpriteRenderer[] flipped;

	// Token: 0x040000F9 RID: 249
	[SerializeField]
	private SpriteRenderer[] glowStingAffected;

	// Token: 0x040000FD RID: 253
	[SerializeField]
	private int moneyMin = 1;

	// Token: 0x040000FE RID: 254
	[SerializeField]
	private int moneyMax = 16;

	// Token: 0x040000FF RID: 255
	[SerializeField]
	private bool achievement;

	// Token: 0x04000100 RID: 256
	[SerializeField]
	private bool cantSeePlayerInCreative = true;

	// Token: 0x04000101 RID: 257
	[SerializeField]
	private bool countsToCap = true;

	// Token: 0x04000102 RID: 258
	[SerializeField]
	private bool dropsKey = true;

	// Token: 0x04000103 RID: 259
	[SerializeField]
	private bool milkable;

	// Token: 0x04000104 RID: 260
	private bool canSeePlayer;

	// Token: 0x04000105 RID: 261
	private bool doTargetPlayer = true;

	// Token: 0x04000106 RID: 262
	private bool milk;

	// Token: 0x04000107 RID: 263
	private bool itemCol;

	// Token: 0x04000108 RID: 264
	private float invinciblityFrame;

	// Token: 0x04000109 RID: 265
	private float glowStingEffect;

	// Token: 0x0400010A RID: 266
	public Vector2 knockback;

	// Token: 0x0400010B RID: 267
	private Vector2 toTarget;

	// Token: 0x0400010C RID: 268
	private Vector2 wander;

	// Token: 0x0400010D RID: 269
	private Vector2 currentVel;

	// Token: 0x0400010E RID: 270
	private Vector2 kbVel;
}
