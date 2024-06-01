using System;
using UnityEngine;

// Token: 0x020000AE RID: 174
public class WorldContainer : MonoBehaviour
{
	// Token: 0x0600033A RID: 826 RVA: 0x000181AD File Offset: 0x000163AD
	private void Start()
	{
		this.rectTransform = base.GetComponent<RectTransform>();
	}

	// Token: 0x0600033B RID: 827 RVA: 0x000181BC File Offset: 0x000163BC
	private void Update()
	{
		this.rectTransform.sizeDelta = new Vector2(0f, Mathf.Clamp(15f + (float)Mathf.Clamp(Mathf.Clamp((GameManager.instance.worldsPage + 1) * 20, 0, GameManager.instance.availableLevels.Count) - GameManager.instance.worldsPage * 20, 0, 20) * 100f, 362.5237f, 100000f));
	}

	// Token: 0x04000418 RID: 1048
	private RectTransform rectTransform;
}
