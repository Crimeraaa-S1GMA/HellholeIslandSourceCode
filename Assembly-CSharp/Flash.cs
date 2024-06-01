using System;
using UnityEngine;

// Token: 0x02000043 RID: 67
public class Flash : MonoBehaviour
{
	// Token: 0x06000116 RID: 278 RVA: 0x00007D2D File Offset: 0x00005F2D
	private void Awake()
	{
		this.s_renderer = base.GetComponent<SpriteRenderer>();
	}

	// Token: 0x06000117 RID: 279 RVA: 0x00007D3C File Offset: 0x00005F3C
	private void Update()
	{
		this.s_renderer.color = new Color(this.s_renderer.color.r, this.s_renderer.color.g, this.s_renderer.color.b, Mathf.Clamp(this.s_renderer.color.a - 1f * Time.deltaTime, 0f, 1f));
	}

	// Token: 0x06000118 RID: 280 RVA: 0x00007DB4 File Offset: 0x00005FB4
	public void PlayFlashAnim()
	{
		this.s_renderer.color = this.white;
	}

	// Token: 0x04000135 RID: 309
	private SpriteRenderer s_renderer;

	// Token: 0x04000136 RID: 310
	[SerializeField]
	private Color white;
}
