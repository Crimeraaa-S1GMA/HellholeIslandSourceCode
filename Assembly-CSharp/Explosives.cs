using System;
using UnityEngine;

// Token: 0x0200003F RID: 63
public class Explosives : MonoBehaviour
{
	// Token: 0x06000106 RID: 262 RVA: 0x0000740E File Offset: 0x0000560E
	private void Start()
	{
		base.Invoke("Explode", 8f);
	}

	// Token: 0x06000107 RID: 263 RVA: 0x00007420 File Offset: 0x00005620
	public void Explode()
	{
		if (!this.hasActivated)
		{
			this.hasActivated = true;
			foreach (Explosives explosives in Object.FindObjectsOfType<Explosives>())
			{
				if (Vector2.Distance(base.transform.position, explosives.transform.position) < 3f)
				{
					explosives.Invoke("Explode", 0.1f);
				}
			}
			foreach (PlacedBlock placedBlock in GameManager.instance.placedBlocks)
			{
				if (!placedBlock.ReturnBlock().immuneToExplosions && Vector2.Distance(base.transform.position, placedBlock.transform.position) < 3f)
				{
					placedBlock.actualBlockHealth = 0f;
					placedBlock.CheckForDeletion();
				}
			}
			foreach (PlacedFloor placedFloor in GameManager.instance.placedFloors)
			{
				if (!placedFloor.ReturnFloor().immuneToExplosions && Vector2.Distance(base.transform.position, placedFloor.transform.position) < 3f)
				{
					placedFloor.actualWallHealth = 0f;
					placedFloor.CheckForDeletion();
				}
			}
			foreach (PlacedTree placedTree in GameManager.instance.placedTrees)
			{
				if (Vector2.Distance(base.transform.position, placedTree.transform.position) < 3f)
				{
					placedTree.actualTreeHealth = 0f;
					placedTree.CheckForDeletion();
				}
			}
			foreach (EnemyBase enemyBase in GameManager.instance.enemyList)
			{
				if (Vector2.Distance(base.transform.position, enemyBase.transform.position) < 3f)
				{
					enemyBase.TakeDamage(100f, true, true, Vector2.zero, DamageSource.ExplosionBomb);
				}
			}
			if (Vector2.Distance(base.transform.position, GameManager.instance.playerPos) < 3f && GameManager.instance.playerMovement != null)
			{
				GameManager.instance.DealDamage(100, DamageSource.ExplosionBomb);
			}
			AudioManager.instance.Play("Explosion");
			ParticleManager.instance.SpawnParticle("explosion", base.transform.position);
			GameManager.instance.blockPositions.Remove(base.transform.position);
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0400012E RID: 302
	private bool hasActivated;
}
