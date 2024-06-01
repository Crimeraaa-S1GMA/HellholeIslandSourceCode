using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

// Token: 0x02000061 RID: 97
public class LevelLight : MonoBehaviour
{
	// Token: 0x060001C9 RID: 457 RVA: 0x0000FB5D File Offset: 0x0000DD5D
	private void Start()
	{
		this.light2d = base.GetComponent<Light2D>();
	}

	// Token: 0x060001CA RID: 458 RVA: 0x0000FB6C File Offset: 0x0000DD6C
	private void Update()
	{
		if (GameManager.instance.isInMines)
		{
			this.light2d.intensity = 0.2f;
			this.light2d.color = Color.Lerp(this.light2d.color, this.normalLightColor, 0.02f);
			return;
		}
		if (GameManager.instance.rain)
		{
			this.light2d.color = Color.Lerp(this.light2d.color, this.rainLightColor, 0.02f);
		}
		else
		{
			this.light2d.color = Color.Lerp(this.light2d.color, this.normalLightColor, 0.02f);
		}
		this.light2d.intensity = GameManager.instance.lightLevel;
	}

	// Token: 0x04000287 RID: 647
	private Light2D light2d;

	// Token: 0x04000288 RID: 648
	public Color normalLightColor;

	// Token: 0x04000289 RID: 649
	public Color rainLightColor;
}
