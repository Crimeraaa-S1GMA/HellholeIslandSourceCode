using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000010 RID: 16
[RequireComponent(typeof(EnemyBase))]
public class SquareShooterEnemy : MonoBehaviour
{
	// Token: 0x06000040 RID: 64 RVA: 0x00002FEF File Offset: 0x000011EF
	private void Start()
	{
		base.StartCoroutine("ShootProjectiles");
	}

	// Token: 0x06000041 RID: 65 RVA: 0x00003000 File Offset: 0x00001200
	private void Update()
	{
		this.gunSprite.enabled = this.eBase.CanSeePlayer();
		this.gunSprite.flipY = ((GameManager.instance.playerPos - base.transform.position).normalized.x < 0f);
		this.gunSprite.transform.right = (GameManager.instance.playerPos - base.transform.position).normalized;
	}

	// Token: 0x06000042 RID: 66 RVA: 0x0000309D File Offset: 0x0000129D
	private IEnumerator ShootProjectiles()
	{
		for (;;)
		{
			yield return new WaitForSeconds(this.shootInterval);
			if (this.eBase.CanSeePlayer())
			{
				this.Shoot();
			}
		}
		yield break;
	}

	// Token: 0x06000043 RID: 67 RVA: 0x000030AC File Offset: 0x000012AC
	private void Shoot()
	{
		AudioManager.instance.Play("Shoot");
		foreach (float num in this.projectileAngles)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.bulletProj);
			gameObject.transform.position = base.transform.position;
			Vector2 normalized = (GameManager.instance.playerPos - base.transform.position).normalized;
			float num2 = Mathf.Atan2(normalized.y, normalized.x) * 57.29578f;
			num2 += num;
			normalized = new Vector2(Mathf.Cos(num2 * 0.0174532924f), Mathf.Sin(num2 * 0.0174532924f));
			gameObject.GetComponent<EnemyProjectile>().direction = normalized;
			gameObject.transform.Translate(normalized);
		}
	}

	// Token: 0x04000033 RID: 51
	public EnemyBase eBase;

	// Token: 0x04000034 RID: 52
	public GameObject bulletProj;

	// Token: 0x04000035 RID: 53
	public SpriteRenderer gunSprite;

	// Token: 0x04000036 RID: 54
	public float[] projectileAngles = new float[1];

	// Token: 0x04000037 RID: 55
	public float shootInterval = 4f;
}
