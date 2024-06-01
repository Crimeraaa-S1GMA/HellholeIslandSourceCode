using System;
using UnityEngine;

// Token: 0x02000004 RID: 4
public class ClownLandMine : MonoBehaviour
{
	// Token: 0x06000008 RID: 8 RVA: 0x0000219C File Offset: 0x0000039C
	private void Start()
	{
		base.Invoke("Explode", 7f);
	}

	// Token: 0x06000009 RID: 9 RVA: 0x000021B0 File Offset: 0x000003B0
	private void Explode()
	{
		foreach (EnemyBase enemyBase in GameManager.instance.enemyList)
		{
			if (Vector2.Distance(base.transform.position, enemyBase.transform.position) < 3f)
			{
				enemyBase.TakeDamage(50f, false, true, Vector2.zero, DamageSource.ClownBomb);
			}
		}
		if (Vector2.Distance(base.transform.position, GameManager.instance.playerPos) < 3f && GameManager.instance.playerMovement != null)
		{
			GameManager.instance.DealDamage(50, DamageSource.ExplosionEnemyProjectile);
		}
		AudioManager.instance.Play("Explosion");
		ParticleManager.instance.SpawnParticle("explosion", base.transform.position);
		Object.Destroy(base.gameObject);
	}
}
