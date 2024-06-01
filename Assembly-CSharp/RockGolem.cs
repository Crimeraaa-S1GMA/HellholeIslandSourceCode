using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200000F RID: 15
[RequireComponent(typeof(EnemyBase))]
public class RockGolem : MonoBehaviour
{
	// Token: 0x0600003C RID: 60 RVA: 0x00002F74 File Offset: 0x00001174
	private void Start()
	{
		base.StartCoroutine("SpawnProjectiles");
	}

	// Token: 0x0600003D RID: 61 RVA: 0x00002F84 File Offset: 0x00001184
	private void Update()
	{
		if (!this.hasSeenPlayer && Vector2.Distance(base.transform.position, GameManager.instance.playerPos) < 8f && GameManager.instance.playerMovement != null)
		{
			this.hasSeenPlayer = true;
		}
	}

	// Token: 0x0600003E RID: 62 RVA: 0x00002FD8 File Offset: 0x000011D8
	private IEnumerator SpawnProjectiles()
	{
		for (;;)
		{
			yield return new WaitForSeconds(6f);
			int num;
			for (int i = 0; i < 3; i = num + 1)
			{
				if (this.eBase.CanSeePlayer() && this.hasSeenPlayer)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.rockProj);
					gameObject.transform.position = base.transform.position;
					gameObject.GetComponent<EnemyProjectile>().direction = (GameManager.instance.playerPos - base.transform.position).normalized;
					AudioManager.instance.Play("SwingWeapon");
				}
				yield return new WaitForSeconds(0.4f);
				num = i;
			}
		}
		yield break;
	}

	// Token: 0x04000030 RID: 48
	public EnemyBase eBase;

	// Token: 0x04000031 RID: 49
	public GameObject rockProj;

	// Token: 0x04000032 RID: 50
	private bool hasSeenPlayer;
}
