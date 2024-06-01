using System;
using UnityEngine;

// Token: 0x02000014 RID: 20
public class Zombie : MonoBehaviour
{
	// Token: 0x0600005A RID: 90 RVA: 0x0000360F File Offset: 0x0000180F
	private void Start()
	{
		this.enemyBase = base.GetComponent<EnemyBase>();
	}

	// Token: 0x0600005B RID: 91 RVA: 0x0000361D File Offset: 0x0000181D
	private void Update()
	{
		this.enemyBase.SetDoTargetPlayer(GameManager.instance.IsNight());
	}

	// Token: 0x04000049 RID: 73
	private EnemyBase enemyBase;
}
