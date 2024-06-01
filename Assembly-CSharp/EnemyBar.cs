using System;
using UnityEngine;

// Token: 0x02000037 RID: 55
public class EnemyBar : MonoBehaviour
{
	// Token: 0x060000C9 RID: 201 RVA: 0x00005949 File Offset: 0x00003B49
	private void Start()
	{
		this.sprite = base.GetComponent<SpriteRenderer>();
	}

	// Token: 0x060000CA RID: 202 RVA: 0x00005958 File Offset: 0x00003B58
	private void Update()
	{
		if (this.enemyBase != null)
		{
			if (this.enemyBase.health >= this.enemyBase.ReturnMaxHealth())
			{
				this.sprite.sprite = null;
			}
			else
			{
				this.sprite.sprite = this.barSprites[Mathf.Clamp(Mathf.RoundToInt(this.enemyBase.health / this.enemyBase.ReturnMaxHealth() * 10f), 0, 10)];
			}
			base.transform.position = this.enemyBase.transform.position + Vector3.up * (float)(this.enemySprite.texture.height / 2 + 5) / this.enemySprite.pixelsPerUnit;
			return;
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060000CB RID: 203 RVA: 0x00005A33 File Offset: 0x00003C33
	public void SetEnemy(EnemyBase enemy)
	{
		if (this.enemyBase == null)
		{
			this.enemyBase = enemy;
			if (enemy.GetComponent<SpriteRenderer>() != null)
			{
				this.enemySprite = enemy.GetComponent<SpriteRenderer>().sprite;
			}
		}
	}

	// Token: 0x040000E7 RID: 231
	[SerializeField]
	private Sprite[] barSprites;

	// Token: 0x040000E8 RID: 232
	private SpriteRenderer sprite;

	// Token: 0x040000E9 RID: 233
	private EnemyBase enemyBase;

	// Token: 0x040000EA RID: 234
	private Sprite enemySprite;
}
