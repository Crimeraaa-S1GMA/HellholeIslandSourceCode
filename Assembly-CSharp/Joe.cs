using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200000B RID: 11
[RequireComponent(typeof(EnemyBase))]
public class Joe : MonoBehaviour
{
	// Token: 0x0600002B RID: 43 RVA: 0x00002A25 File Offset: 0x00000C25
	private void Start()
	{
		this.enemyBase = base.GetComponent<EnemyBase>();
		this.enemyBase.OnEnemyDeath += this.DeadAchievement;
		base.StartCoroutine("SpawnFireballs");
	}

	// Token: 0x0600002C RID: 44 RVA: 0x00002A56 File Offset: 0x00000C56
	private void DeadAchievement()
	{
		AchievementManager.instance.AddAchievement(156196);
	}

	// Token: 0x0600002D RID: 45 RVA: 0x00002A67 File Offset: 0x00000C67
	private IEnumerator SpawnFireballs()
	{
		for (;;)
		{
			yield return new WaitForSeconds((this.enemyBase.health < this.enemyBase.ReturnMaxHealth() / 2f || WorldPolicy.hellholeMode) ? 0.4f : 1.5f);
			if (GameManager.instance.playerMovement != null)
			{
				AudioManager.instance.Play("Shoot");
				GameObject gameObject = Object.Instantiate<GameObject>(this.fireball);
				if (this.enemyBase.health < this.enemyBase.ReturnMaxHealth() / 2f)
				{
					gameObject.transform.position = GameManager.instance.playerPos + new Vector2((float)(7 * UtilityMath.NegativePositiveOne()), (float)(7 * UtilityMath.NegativePositiveOne()));
					ParticleManager.instance.SpawnParticle("joeboom", gameObject.transform.position);
					gameObject.GetComponent<EnemyProjectile>().projectileSpeed = 4.5f;
				}
				else
				{
					gameObject.transform.position = GameManager.instance.playerPos + new Vector2((float)(6 * UtilityMath.NegativePositiveOne()), (float)(6 * UtilityMath.NegativePositiveOne()));
					ParticleManager.instance.SpawnParticle("joeboom", gameObject.transform.position);
				}
				gameObject.GetComponent<EnemyProjectile>().direction = (GameManager.instance.playerPos - gameObject.transform.position).normalized;
			}
		}
		yield break;
	}

	// Token: 0x0400001F RID: 31
	public GameObject fireball;

	// Token: 0x04000020 RID: 32
	private EnemyBase enemyBase;
}
