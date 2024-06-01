using System;
using UnityEngine;

// Token: 0x02000064 RID: 100
public class MapsContainer : MonoBehaviour
{
	// Token: 0x060001D0 RID: 464 RVA: 0x000101C7 File Offset: 0x0000E3C7
	private void Start()
	{
		this.rectTransform = base.GetComponent<RectTransform>();
	}

	// Token: 0x060001D1 RID: 465 RVA: 0x000101D8 File Offset: 0x0000E3D8
	private void Update()
	{
		this.rectTransform.sizeDelta = new Vector2(0f, Mathf.Clamp(15f + (float)Mathf.Clamp(GameManager.instance.loadedMaps.Count, 0, 20) * 100f, 362.5237f, 10000f));
	}

	// Token: 0x040002BB RID: 699
	private RectTransform rectTransform;
}
