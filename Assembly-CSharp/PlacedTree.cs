using System;
using UnityEngine;

// Token: 0x02000072 RID: 114
[RequireComponent(typeof(SpriteRenderer))]
public class PlacedTree : MonoBehaviour
{
	// Token: 0x0600021A RID: 538 RVA: 0x00011E1A File Offset: 0x0001001A
	private void Awake()
	{
		this.treeSprite = base.GetComponent<SpriteRenderer>();
		GameManager.instance.placedTrees.Add(this);
	}

	// Token: 0x0600021B RID: 539 RVA: 0x00011E38 File Offset: 0x00010038
	public void PrepareTree(string key, bool matureTree)
	{
		this.treeKey = key;
		this.actualTreeHealth = this.ReturnTree().maxTreeHealth;
		if (matureTree)
		{
			this.treeGrowthProgress = 1f;
		}
		base.GetComponent<OptimizeRenderer>().StartUpdateRender();
	}

	// Token: 0x0600021C RID: 540 RVA: 0x00011E6C File Offset: 0x0001006C
	private void Update()
	{
		this.treeGrowthProgress += this.ReturnTree().growthSpeed * Time.deltaTime;
		this.treeGrowthProgress = Mathf.Clamp(this.treeGrowthProgress, 0f, 1f);
		if (this.treeGrowthProgress >= 1f)
		{
			this.treeSprite.sprite = this.ReturnTree().treeSprite;
		}
		else
		{
			this.treeSprite.sprite = this.ReturnTree().saplingSprite;
		}
		if (GameManager.instance.playerPos.y > base.transform.position.y && Vector2.Distance(base.transform.position, GameManager.instance.playerPos) < 3f && this.treeGrowthProgress >= 1f && Mathf.Abs(base.transform.position.x - GameManager.instance.playerPos.x) < 1f && GameManager.instance.treeTransparency)
		{
			this.treeSprite.color = Color.Lerp(this.treeSprite.color, this.transparencies[1], 0.12f);
			return;
		}
		this.treeSprite.color = Color.Lerp(this.treeSprite.color, this.transparencies[0], 0.12f);
	}

	// Token: 0x0600021D RID: 541 RVA: 0x00011FD5 File Offset: 0x000101D5
	public Tree ReturnTree()
	{
		return GameManager.instance.treeRegistryReference.FindTreeByInternalIdentifier(this.treeKey);
	}

	// Token: 0x0600021E RID: 542 RVA: 0x00011FEC File Offset: 0x000101EC
	public void CheckForDeletion()
	{
		if (this.actualTreeHealth <= 0f)
		{
			GameManager.instance.treePositions.Remove(base.transform.position);
			if (this.treeGrowthProgress >= 1f)
			{
				GameManager.instance.InitializePickupItem(base.transform.position + new Vector2(Random.Range(-2f, 1f), Random.Range(-2f, 1f)), this.ReturnTree().woodId, Random.Range(10, Random.Range(11, 51)), true);
				GameManager.instance.InitializePickupItem(base.transform.position + new Vector2(Random.Range(-2f, 1f), Random.Range(-2f, 1f)), this.ReturnTree().saplingId, Random.Range(1, 3), true);
				if (this.ReturnTree().dropExtraItems)
				{
					GameManager.instance.InitializePickupItem(base.transform.position + new Vector2(Random.Range(-2f, 1f), Random.Range(-2f, 1f)), "leaf", Random.Range(2, 6), true);
					if (Random.Range(1, 11) == 1)
					{
						GameManager.instance.InitializePickupItem(base.transform.position + new Vector2(Random.Range(-2f, 1f), Random.Range(-2f, 1f)), "apple", Random.Range(1, 4), true);
					}
				}
			}
			AchievementManager.instance.AddAchievement(156193);
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0600021F RID: 543 RVA: 0x000121C3 File Offset: 0x000103C3
	private void OnDestroy()
	{
		GameManager.instance.placedTrees.Remove(this);
	}

	// Token: 0x040002F5 RID: 757
	[HideInInspector]
	public float actualTreeHealth;

	// Token: 0x040002F6 RID: 758
	public float treeGrowthProgress = 0.4f;

	// Token: 0x040002F7 RID: 759
	private SpriteRenderer treeSprite;

	// Token: 0x040002F8 RID: 760
	public Color[] transparencies;

	// Token: 0x040002F9 RID: 761
	public string treeKey = "standardtree";
}
