using System;
using UnityEngine;

// Token: 0x02000089 RID: 137
public class StaticBackgroundRepeat : MonoBehaviour
{
	// Token: 0x06000278 RID: 632 RVA: 0x00014C6C File Offset: 0x00012E6C
	private void Start()
	{
		this.startPos = base.transform.position;
		Sprite sprite = base.GetComponent<SpriteRenderer>().sprite;
		Texture2D texture = sprite.texture;
		this.textureUnitSizeX = (float)texture.width / sprite.pixelsPerUnit * base.transform.localScale.x;
		this.textureUnitSizeY = (float)texture.height / sprite.pixelsPerUnit * base.transform.localScale.y;
	}

	// Token: 0x06000279 RID: 633 RVA: 0x00014CEC File Offset: 0x00012EEC
	private void Update()
	{
		base.transform.position = this.Pos();
	}

	// Token: 0x0600027A RID: 634 RVA: 0x00014D04 File Offset: 0x00012F04
	private Vector2 Pos()
	{
		Vector2 vector = Camera.main.transform.position;
		Vector2 vector2 = this.startPos;
		float num = (vector.x - vector2.x) / this.textureUnitSizeX;
		float num2 = (vector.y - vector2.y) / this.textureUnitSizeY;
		int num3;
		if (Mathf.Abs(num) == num)
		{
			num3 = Mathf.FloorToInt(num);
		}
		else
		{
			num3 = Mathf.CeilToInt(num);
		}
		int num4;
		if (Mathf.Abs(num2) == num2)
		{
			num4 = Mathf.FloorToInt(num2);
		}
		else
		{
			num4 = Mathf.CeilToInt(num2);
		}
		return vector2 + new Vector2((float)num3 * this.textureUnitSizeX, (float)num4 * this.textureUnitSizeY);
	}

	// Token: 0x04000357 RID: 855
	private float textureUnitSizeX;

	// Token: 0x04000358 RID: 856
	private float textureUnitSizeY;

	// Token: 0x04000359 RID: 857
	private Vector2 startPos;
}
