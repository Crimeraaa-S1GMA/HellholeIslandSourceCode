using System;
using UnityEngine;

// Token: 0x02000027 RID: 39
public class CampfireCollider : MonoBehaviour
{
	// Token: 0x06000099 RID: 153 RVA: 0x00004870 File Offset: 0x00002A70
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			GameManager.instance.DealDamage(5, DamageSource.Campfire);
		}
	}
}
