using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200000E RID: 14
[RequireComponent(typeof(EnemyBase), typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class RainCloud : MonoBehaviour
{
	// Token: 0x06000035 RID: 53 RVA: 0x00002B05 File Offset: 0x00000D05
	private void Start()
	{
		this.physicsComponent = base.GetComponent<Rigidbody2D>();
		this.enemyBase = base.GetComponent<EnemyBase>();
		this.anim = base.GetComponent<EnemyAnimator>();
		this.spriteRenderer = base.GetComponent<SpriteRenderer>();
		base.StartCoroutine("SwitchEnemyState");
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00002B44 File Offset: 0x00000D44
	private void Update()
	{
		if (this.enemyState == 0)
		{
			this.physicsComponent.velocity = (new Vector2(Mathf.Cos(this.enemyBase.ReturnAngleToTarget() * 0.0174532924f), Mathf.Sin(this.enemyBase.ReturnAngleToTarget() * 0.0174532924f)) + this.enemyBase.knockback) * 2f;
			this.spriteRenderer.material = GameManager.instance.Lit;
			this.SetThunderPosition(Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero);
		}
		else
		{
			this.physicsComponent.velocity = Vector2.zero;
			this.spriteRenderer.material = GameManager.instance.Unlit;
			this.thunderTarget += (GameManager.instance.playerPos - (base.transform.position + this.thunderTarget)).normalized * 2f * Time.deltaTime;
			this.thunderTarget = new Vector2(Mathf.Clamp(this.thunderTarget.x, -16f, 16f), Mathf.Clamp(this.thunderTarget.y, -16f, 16f));
			this.SetThunderPosition(Vector2.zero, this.ThunderTargetBlock(this.thunderTarget) / 3f * 1f + new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f)), this.ThunderTargetBlock(this.thunderTarget) / 3f * 2f + new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f)), this.ThunderTargetBlock(this.thunderTarget) / 3f * 3f + new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f)), this.ThunderTargetBlock(this.thunderTarget));
			this.DamagePlayer();
		}
		if (this.anim != null)
		{
			this.anim.SetAnimation(this.enemyState);
		}
		if (!this.hasSeenPlayer && Vector2.Distance(base.transform.position, GameManager.instance.playerPos) < 8f && GameManager.instance.playerMovement != null)
		{
			this.hasSeenPlayer = true;
		}
	}

	// Token: 0x06000037 RID: 55 RVA: 0x00002E04 File Offset: 0x00001004
	private Vector2 ThunderTargetBlock(Vector2 targetOriginal)
	{
		RaycastHit2D raycastHit2D = Physics2D.Raycast(base.transform.position, targetOriginal, targetOriginal.magnitude, this.thunderLayerMaskWithBlock);
		if (!(raycastHit2D.collider != null))
		{
			return targetOriginal;
		}
		if (raycastHit2D.collider.CompareTag("Block") && !raycastHit2D.collider.isTrigger)
		{
			return raycastHit2D.point - base.transform.position;
		}
		return targetOriginal;
	}

	// Token: 0x06000038 RID: 56 RVA: 0x00002E8C File Offset: 0x0000108C
	private void DamagePlayer()
	{
		RaycastHit2D raycastHit2D = Physics2D.Raycast(base.transform.position, this.thunderTarget, this.thunderTarget.magnitude, this.thunderLayerMask);
		if (raycastHit2D.collider != null && raycastHit2D.collider.CompareTag("Player"))
		{
			GameManager.instance.DealDamage(2, DamageSource.RainCloudThunder);
		}
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00002EFC File Offset: 0x000010FC
	private void SetThunderPosition(Vector2 one, Vector2 two, Vector2 three, Vector2 four, Vector2 five)
	{
		this.thunderLine.SetPositions(new Vector3[]
		{
			one,
			two,
			three,
			four,
			five
		});
	}

	// Token: 0x0600003A RID: 58 RVA: 0x00002F5D File Offset: 0x0000115D
	private IEnumerator SwitchEnemyState()
	{
		for (;;)
		{
			this.enemyState = 0;
			yield return new WaitForSeconds(3f);
			this.thunderTarget = GameManager.instance.playerPos - base.transform.position;
			yield return new WaitForSeconds(2f);
			if (this.hasSeenPlayer)
			{
				this.enemyState = 1;
			}
			yield return new WaitForSeconds(4f);
			this.enemyState = 0;
		}
		yield break;
	}

	// Token: 0x04000026 RID: 38
	private Rigidbody2D physicsComponent;

	// Token: 0x04000027 RID: 39
	private EnemyBase enemyBase;

	// Token: 0x04000028 RID: 40
	private EnemyAnimator anim;

	// Token: 0x04000029 RID: 41
	private SpriteRenderer spriteRenderer;

	// Token: 0x0400002A RID: 42
	public LayerMask thunderLayerMask;

	// Token: 0x0400002B RID: 43
	public LayerMask thunderLayerMaskWithBlock;

	// Token: 0x0400002C RID: 44
	public LineRenderer thunderLine;

	// Token: 0x0400002D RID: 45
	private int enemyState;

	// Token: 0x0400002E RID: 46
	private Vector2 thunderTarget;

	// Token: 0x0400002F RID: 47
	private bool hasSeenPlayer;
}
