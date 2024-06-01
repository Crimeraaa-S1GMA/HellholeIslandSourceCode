using System;
using UnityEngine;

// Token: 0x02000024 RID: 36
public class BossBar : MonoBehaviour
{
	// Token: 0x0600008A RID: 138 RVA: 0x00004550 File Offset: 0x00002750
	private void Update()
	{
		if (GameManager.instance.boss != null)
		{
			this.barFill.localScale = new Vector3(Mathf.Clamp(GameManager.instance.boss.enemy.health / GameManager.instance.boss.enemy.ReturnMaxHealth(), 0f, 1f), 1f, 1f);
		}
	}

	// Token: 0x040000A9 RID: 169
	[SerializeField]
	private Transform barFill;
}
