using System;
using UnityEngine;

// Token: 0x02000076 RID: 118
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerHeadAccesory : MonoBehaviour
{
	// Token: 0x0600022A RID: 554 RVA: 0x00012360 File Offset: 0x00010560
	private void Awake()
	{
		this.sr = base.GetComponent<SpriteRenderer>();
	}

	// Token: 0x0600022B RID: 555 RVA: 0x00012370 File Offset: 0x00010570
	private void Update()
	{
		this.sr.sprite = GameManager.instance.ReturnHeadAccesory();
		this.sr.material = (GameManager.instance.IsMiningHelmet() ? GameManager.instance.Unlit : GameManager.instance.Lit);
		this.helmetLight.SetActive(GameManager.instance.IsMiningHelmet());
	}

	// Token: 0x04000300 RID: 768
	public GameObject helmetLight;

	// Token: 0x04000301 RID: 769
	private SpriteRenderer sr;
}
