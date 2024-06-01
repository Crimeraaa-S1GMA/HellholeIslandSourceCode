using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000011 RID: 17
[RequireComponent(typeof(EnemyBase))]
public class ToasterTroll : MonoBehaviour
{
	// Token: 0x06000045 RID: 69 RVA: 0x000031AC File Offset: 0x000013AC
	private void Start()
	{
		base.StartCoroutine("SpawnProjectiles");
	}

	// Token: 0x06000046 RID: 70 RVA: 0x000031BC File Offset: 0x000013BC
	private void Update()
	{
		if (!this.hasSeenPlayer && Vector2.Distance(base.transform.position, GameManager.instance.playerPos) < 8f && GameManager.instance.playerMovement != null)
		{
			this.hasSeenPlayer = true;
		}
	}

	// Token: 0x06000047 RID: 71 RVA: 0x00003210 File Offset: 0x00001410
	private IEnumerator SpawnProjectiles()
	{
		for (;;)
		{
			yield return new WaitForSeconds(4f);
			int num;
			for (int i = 0; i < 8; i = num + 1)
			{
				if (this.eBase.CanSeePlayer() && this.hasSeenPlayer)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.toastProj);
					gameObject.transform.position = base.transform.position;
					gameObject.GetComponent<EnemyProjectile>().direction = (GameManager.instance.playerPos - base.transform.position).normalized;
					AudioManager.instance.Play("SwingWeapon");
				}
				yield return new WaitForSeconds(0.5f);
				num = i;
			}
		}
		yield break;
	}

	// Token: 0x04000038 RID: 56
	public EnemyBase eBase;

	// Token: 0x04000039 RID: 57
	public GameObject toastProj;

	// Token: 0x0400003A RID: 58
	private bool hasSeenPlayer;
}
