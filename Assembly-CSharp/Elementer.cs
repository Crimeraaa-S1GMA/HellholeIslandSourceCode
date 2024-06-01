using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000006 RID: 6
public class Elementer : MonoBehaviour
{
	// Token: 0x06000010 RID: 16 RVA: 0x0000240B File Offset: 0x0000060B
	private void Start()
	{
		base.StartCoroutine("SpawnProjectiles");
	}

	// Token: 0x06000011 RID: 17 RVA: 0x00002419 File Offset: 0x00000619
	private IEnumerator SpawnProjectiles()
	{
		for (;;)
		{
			yield return new WaitForSeconds(2f);
			if (this.eBase.CanSeePlayer())
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.woodProj, base.transform);
				gameObject.transform.position = base.transform.position;
				gameObject.GetComponent<EnemyProjectile>().direction = (GameManager.instance.playerPos - base.transform.position).normalized;
				AudioManager.instance.Play("Shoot");
			}
		}
		yield break;
	}

	// Token: 0x0400000B RID: 11
	public EnemyBase eBase;

	// Token: 0x0400000C RID: 12
	public GameObject woodProj;
}
