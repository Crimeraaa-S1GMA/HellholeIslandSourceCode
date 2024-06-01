using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200001B RID: 27
public class BestiaryGui : MonoBehaviour
{
	// Token: 0x06000079 RID: 121 RVA: 0x0000413C File Offset: 0x0000233C
	private void Update()
	{
		this.entryImage.sprite = GameManager.instance.bestiaryRegistryReference.bestiaryEntries[this.bestiaryPage].entryPreview;
		this.entryImage.color = GameManager.instance.bestiaryRegistryReference.bestiaryEntries[this.bestiaryPage].entryTint;
		this.entryTitle.text = GameManager.instance.bestiaryRegistryReference.bestiaryEntries[this.bestiaryPage].entryName;
		this.entryDesc.text = GameManager.instance.bestiaryRegistryReference.bestiaryEntries[this.bestiaryPage].entryDescription;
		this.entryHealth.text = GameManager.instance.bestiaryRegistryReference.bestiaryEntries[this.bestiaryPage].enemyHealth.ToString();
		this.SetSpawnCondition();
	}

	// Token: 0x0600007A RID: 122 RVA: 0x00004214 File Offset: 0x00002414
	private void SetSpawnCondition()
	{
		switch (GameManager.instance.bestiaryRegistryReference.bestiaryEntries[this.bestiaryPage].spawnCondition)
		{
		case BestiarySpawnCondition.Day:
			this.entrySpawnConditionImage.sprite = this.spawnConditionDay;
			this.entrySpawnConditionText.text = "Day";
			return;
		case BestiarySpawnCondition.Night:
			this.entrySpawnConditionImage.sprite = this.spawnConditionNight;
			this.entrySpawnConditionText.text = "Night";
			return;
		case BestiarySpawnCondition.Rain:
			this.entrySpawnConditionImage.sprite = this.spawnConditionRain;
			this.entrySpawnConditionText.text = "Rain";
			return;
		case BestiarySpawnCondition.CubicChaos:
			this.entrySpawnConditionImage.sprite = this.spawnConditionCubicChaos;
			this.entrySpawnConditionText.text = "Cubic Chaos";
			return;
		case BestiarySpawnCondition.Mines:
			this.entrySpawnConditionImage.sprite = this.spawnConditionMines;
			this.entrySpawnConditionText.text = "Mines";
			return;
		case BestiarySpawnCondition.Boss:
			this.entrySpawnConditionImage.sprite = this.spawnConditionBoss;
			this.entrySpawnConditionText.text = "Boss";
			return;
		default:
			this.entrySpawnConditionImage.sprite = this.spawnConditionDay;
			this.entrySpawnConditionText.text = "?";
			return;
		}
	}

	// Token: 0x0600007B RID: 123 RVA: 0x0000434D File Offset: 0x0000254D
	public void BestiaryPage(int changedAmount)
	{
		this.bestiaryPage = Mathf.Clamp(this.bestiaryPage + changedAmount, 0, GameManager.instance.bestiaryRegistryReference.bestiaryEntries.Length - 1);
	}

	// Token: 0x04000056 RID: 86
	[HideInInspector]
	public int bestiaryPage;

	// Token: 0x04000057 RID: 87
	[SerializeField]
	private Image entryImage;

	// Token: 0x04000058 RID: 88
	[SerializeField]
	private Text entryTitle;

	// Token: 0x04000059 RID: 89
	[SerializeField]
	private Text entryDesc;

	// Token: 0x0400005A RID: 90
	[SerializeField]
	private Text entryHealth;

	// Token: 0x0400005B RID: 91
	[SerializeField]
	private Text entrySpawnConditionText;

	// Token: 0x0400005C RID: 92
	[SerializeField]
	private Image entrySpawnConditionImage;

	// Token: 0x0400005D RID: 93
	[SerializeField]
	private Sprite spawnConditionDay;

	// Token: 0x0400005E RID: 94
	[SerializeField]
	private Sprite spawnConditionNight;

	// Token: 0x0400005F RID: 95
	[SerializeField]
	private Sprite spawnConditionRain;

	// Token: 0x04000060 RID: 96
	[SerializeField]
	private Sprite spawnConditionCubicChaos;

	// Token: 0x04000061 RID: 97
	[SerializeField]
	private Sprite spawnConditionMines;

	// Token: 0x04000062 RID: 98
	[SerializeField]
	private Sprite spawnConditionBoss;
}
