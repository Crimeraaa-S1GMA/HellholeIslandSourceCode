using System;
using UnityEngine;

// Token: 0x02000023 RID: 35
public class Boss : MonoBehaviour
{
	// Token: 0x06000085 RID: 133 RVA: 0x000044A0 File Offset: 0x000026A0
	private void Start()
	{
		this.enemy = base.GetComponent<EnemyBase>();
		GameManager.instance.boss = this;
		this.enemy.OnEnemyDeath += this.UnloadBoss;
		this.enemy.OnEnemyDeath += this.BossDeath;
	}

	// Token: 0x06000086 RID: 134 RVA: 0x000044F2 File Offset: 0x000026F2
	private void Update()
	{
		if (GameManager.instance.playerMovement == null)
		{
			this.UnloadBoss();
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06000087 RID: 135 RVA: 0x00004517 File Offset: 0x00002717
	private void BossDeath()
	{
		GameManager.instance.AddBossFlag(this.bossFlagName);
	}

	// Token: 0x06000088 RID: 136 RVA: 0x00004529 File Offset: 0x00002729
	private void UnloadBoss()
	{
		if (GameManager.instance.boss == this)
		{
			GameManager.instance.boss = null;
		}
	}

	// Token: 0x040000A7 RID: 167
	[HideInInspector]
	public EnemyBase enemy;

	// Token: 0x040000A8 RID: 168
	public string bossFlagName;
}
