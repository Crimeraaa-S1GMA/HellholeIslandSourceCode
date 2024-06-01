using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200000D RID: 13
public class OreGolem : MonoBehaviour
{
	// Token: 0x06000032 RID: 50 RVA: 0x00002AE0 File Offset: 0x00000CE0
	private void Start()
	{
		base.StartCoroutine("SpawnProjectiles");
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00002AEE File Offset: 0x00000CEE
	private IEnumerator SpawnProjectiles()
	{
		for (;;)
		{
			foreach (GameObject projectile in this.projectiles)
			{
				yield return new WaitForSeconds(2f);
				int num;
				for (int i = 0; i < 10; i = num + 1)
				{
					if (GameManager.instance.playerMovement != null)
					{
						AudioManager.instance.Play("Shoot");
						GameObject gameObject = Object.Instantiate<GameObject>(projectile);
						gameObject.transform.position = base.transform.position;
						gameObject.GetComponent<EnemyProjectile>().direction = (GameManager.instance.playerPos - base.transform.position).normalized;
					}
					yield return new WaitForSeconds(1f - (float)i * 0.08f);
					num = i;
				}
				projectile = null;
			}
			GameObject[] array = null;
		}
		yield break;
	}

	// Token: 0x04000025 RID: 37
	public GameObject[] projectiles;
}
