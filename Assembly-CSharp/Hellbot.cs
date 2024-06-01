using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000009 RID: 9
public class Hellbot : MonoBehaviour
{
	// Token: 0x0600001B RID: 27 RVA: 0x00002706 File Offset: 0x00000906
	private void Start()
	{
		this.enemyBase.OnEnemyDeath += this.DeadAchievement;
		base.StartCoroutine("Teleport");
		base.StartCoroutine("Enemies");
		base.StartCoroutine("Explosions");
	}

	// Token: 0x0600001C RID: 28 RVA: 0x00002743 File Offset: 0x00000943
	private void DeadAchievement()
	{
		AchievementManager.instance.AddAchievement(161280);
	}

	// Token: 0x0600001D RID: 29 RVA: 0x00002754 File Offset: 0x00000954
	private string ReturnEnemyKey()
	{
		switch (Random.Range(1, 4))
		{
		case 1:
			return "rockgolem";
		case 2:
			return "zombie";
		case 3:
			return "fluffy";
		default:
			return "dontspawn";
		}
	}

	// Token: 0x0600001E RID: 30 RVA: 0x00002798 File Offset: 0x00000998
	private void Explode(Vector2 pos)
	{
		if (Vector2.Distance(pos, GameManager.instance.playerPos) < 3f && GameManager.instance.playerMovement != null)
		{
			GameManager.instance.DealDamage(10, DamageSource.ExplosionEnemy);
		}
		AudioManager.instance.Play("Explosion");
		ParticleManager.instance.SpawnParticle("explosion", pos);
	}

	// Token: 0x0600001F RID: 31 RVA: 0x000027FB File Offset: 0x000009FB
	private IEnumerator Explosions()
	{
		for (;;)
		{
			yield return new WaitForSeconds(WorldPolicy.hellholeMode ? 0.15f : 0.2f);
			if (this.enemyBase.health / this.enemyBase.ReturnMaxHealth() < 0.6f)
			{
				this.Explode(base.transform.position + new Vector2(Random.Range(14f, 24f) * (float)UtilityMath.NegativePositiveOne(), Random.Range(9f, 15f) * (float)UtilityMath.NegativePositiveOne()));
			}
		}
		yield break;
	}

	// Token: 0x06000020 RID: 32 RVA: 0x0000280A File Offset: 0x00000A0A
	private IEnumerator Enemies()
	{
		for (;;)
		{
			yield return new WaitForSeconds(1.5f);
			Object.FindObjectOfType<EnemySpawner>().SpawnEnemyAtPosition(this.ReturnEnemyKey(), Vector2.Lerp(GameManager.instance.playerPos, base.transform.position, 0.4f));
		}
		yield break;
	}

	// Token: 0x06000021 RID: 33 RVA: 0x00002819 File Offset: 0x00000A19
	private IEnumerator Teleport()
	{
		for (;;)
		{
			yield return new WaitForSeconds(4f);
			this.playerPosForTeleport = GameManager.instance.playerPos;
			yield return new WaitForSeconds(4f);
			Object.FindObjectOfType<Flash>().PlayFlashAnim();
			base.transform.position = this.playerPosForTeleport;
		}
		yield break;
	}

	// Token: 0x04000016 RID: 22
	private Vector2 playerPosForTeleport;

	// Token: 0x04000017 RID: 23
	public EnemyBase enemyBase;
}
