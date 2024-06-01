using System;
using UnityEngine;

// Token: 0x0200003D RID: 61
public class EnemySpawner : MonoBehaviour
{
	// Token: 0x060000FF RID: 255 RVA: 0x00006F14 File Offset: 0x00005114
	private void Update()
	{
		if (Random.Range(1, this.ReturnSpawnRate()) == 1 && WorldPolicy.spawnMobs && GameManager.instance.bed == null && GameManager.instance.enemiesInWorld < 20 && !GameManager.instance.generatingWorld)
		{
			this.SpawnEnemy(this.ReturnRandomEnemyKey());
		}
	}

	// Token: 0x06000100 RID: 256 RVA: 0x00006F70 File Offset: 0x00005170
	private void SpawnEnemy(string enemyKey)
	{
		Vector2 vector = GameManager.instance.playerPos + new Vector2((float)(UtilityMath.NegativePositiveOne() * 35), (float)(UtilityMath.NegativePositiveOne() * 15));
		vector.y = Mathf.Max(vector.y, (float)(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-148) + 5));
		if (enemyKey != "dontspawn" && !GameManager.instance.spawnBlockerPositions.Contains(Vector2Int.RoundToInt(vector) + Vector2.right * 0.5f) && !GameManager.instance.floorPositions.Contains(Vector2Int.RoundToInt(vector) + Vector2.right * 0.5f))
		{
			Object.Instantiate<GameObject>(Array.Find<EnemyToSpawn>(this.enemyList, (EnemyToSpawn e) => e.ENEMY_ID == enemyKey).ENEMY).transform.position = vector;
		}
	}

	// Token: 0x06000101 RID: 257 RVA: 0x00007080 File Offset: 0x00005280
	public void SpawnEnemyAtPosition(string enemyKey, Vector2 position)
	{
		if (enemyKey != "dontspawn" && GameManager.instance.enemiesInWorld < 70)
		{
			Vector2 vector = position;
			vector.y = Mathf.Max(vector.y, (float)(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-148) + 5));
			Object.Instantiate<GameObject>(Array.Find<EnemyToSpawn>(this.enemyList, (EnemyToSpawn e) => e.ENEMY_ID == enemyKey).ENEMY).transform.position = vector;
		}
	}

	// Token: 0x06000102 RID: 258 RVA: 0x00007118 File Offset: 0x00005318
	private string ReturnRandomEnemyKey()
	{
		if (GameManager.instance.invasionEvent == Invasion.CubicChaos)
		{
			if (Random.Range(1, 61) == 1)
			{
				return "snipersquare";
			}
			if (Random.Range(1, 21) == 1)
			{
				return "shotgunnersquare";
			}
			if (Random.Range(1, 21) == 1)
			{
				return "machinegunsquare";
			}
			if (Random.Range(1, 11) == 1)
			{
				return "meleesquare";
			}
			return "regularsquare";
		}
		else if (GameManager.instance.sniperSkillsSeed)
		{
			if (Random.Range(1, 61) == 1)
			{
				return "snipersquare";
			}
			if (Random.Range(1, 21) == 1)
			{
				return "shotgunnersquare";
			}
			if (Random.Range(1, 21) == 1)
			{
				return "machinegunsquare";
			}
			if (Random.Range(1, 11) == 1)
			{
				return "meleesquare";
			}
			return "regularsquare";
		}
		else
		{
			if (GameManager.instance.isInMines && Random.Range(1, 4) == 1)
			{
				return "rockgolem";
			}
			if (GameManager.instance.isInMines)
			{
				return "caveslime";
			}
			if (Random.Range(1, 251) == 1 && WorldPolicy.hellholeMode)
			{
				return "toastertroll";
			}
			if (!GameManager.instance.IsNight() && Random.Range(1, 81) == 1)
			{
				return "merchant";
			}
			if (Random.Range(1, 24) == 1 && GameManager.instance.defeatBosses.Contains("ore_golem"))
			{
				return "elementer";
			}
			if (!GameManager.instance.IsNight() && Random.Range(1, 121) == 1)
			{
				return "gardengnome";
			}
			if (GameManager.instance.rain && Random.Range(1, 11) == 1)
			{
				return "furiouscloud";
			}
			if (GameManager.instance.rain && Random.Range(1, 4) == 1)
			{
				return "fluffy";
			}
			if (GameManager.instance.IsNight() && Random.Range(1, 21) == 1)
			{
				return "vampire";
			}
			if (GameManager.instance.IsNight() && Random.Range(1, 6) == 1)
			{
				return "angryfirefly";
			}
			if (GameManager.instance.IsNight() && Random.Range(1, 3) == 1)
			{
				return "skeleton";
			}
			if (GameManager.instance.IsNight())
			{
				return "zombie";
			}
			if (Random.Range(1, 14) == 1)
			{
				return "rockgolem";
			}
			if (!GameManager.instance.IsNight() && Random.Range(1, 6) == 1)
			{
				return "chicken";
			}
			if (!GameManager.instance.IsNight() && Random.Range(1, 6) == 1)
			{
				return "cow";
			}
			if (!GameManager.instance.IsNight() && Random.Range(1, 3) == 1)
			{
				return "greenslime";
			}
			if (!GameManager.instance.IsNight())
			{
				return "slime";
			}
			return "dontspawn";
		}
	}

	// Token: 0x06000103 RID: 259 RVA: 0x00007398 File Offset: 0x00005598
	private int ReturnSpawnRate()
	{
		if (GameManager.instance.HasStatusEffect("invisiblity"))
		{
			return 501;
		}
		if (GameManager.instance.invasionEvent == Invasion.CubicChaos)
		{
			return 51;
		}
		if (GameManager.instance.sniperSkillsSeed)
		{
			return 61;
		}
		if (WorldPolicy.hardcoreMode || GameManager.instance.EquippedAccesory("ring_of_doom"))
		{
			return 171;
		}
		return 331;
	}

	// Token: 0x0400012B RID: 299
	public EnemyToSpawn[] enemyList;
}
