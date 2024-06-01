using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000003 RID: 3
public class AngryFirefly : MonoBehaviour
{
	// Token: 0x06000004 RID: 4 RVA: 0x000020B7 File Offset: 0x000002B7
	private void Start()
	{
		this.enemyBase = base.GetComponent<EnemyBase>();
		this.walkingEnemy = base.GetComponent<WalkingEnemy>();
		this.enemyAnimator = base.GetComponent<EnemyAnimator>();
		base.StartCoroutine("FireflyLight");
	}

	// Token: 0x06000005 RID: 5 RVA: 0x000020EC File Offset: 0x000002EC
	private void Update()
	{
		if (this.enemyBase.health < this.enemyBase.ReturnMaxHealth() * 0.5f)
		{
			this.walkingEnemy.speed = 8f;
			this.enemyBase.SetDealtDamage(13);
			if (this.enemyAnimator != null)
			{
				this.enemyAnimator.SetAnimation(1);
				return;
			}
		}
		else
		{
			this.walkingEnemy.speed = 3f;
			this.enemyBase.SetDealtDamage(5);
			if (this.enemyAnimator != null)
			{
				this.enemyAnimator.SetAnimation(0);
			}
		}
	}

	// Token: 0x06000006 RID: 6 RVA: 0x00002185 File Offset: 0x00000385
	private IEnumerator FireflyLight()
	{
		for (;;)
		{
			yield return new WaitForSeconds(3.5f);
			this.fireflyLight.material = GameManager.instance.Unlit;
			yield return new WaitForSeconds(0.3f);
			this.fireflyLight.material = GameManager.instance.Lit;
		}
		yield break;
	}

	// Token: 0x04000002 RID: 2
	public SpriteRenderer fireflyLight;

	// Token: 0x04000003 RID: 3
	private EnemyBase enemyBase;

	// Token: 0x04000004 RID: 4
	private WalkingEnemy walkingEnemy;

	// Token: 0x04000005 RID: 5
	private EnemyAnimator enemyAnimator;
}
