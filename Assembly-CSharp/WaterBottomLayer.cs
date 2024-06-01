using System;
using UnityEngine;

// Token: 0x020000A6 RID: 166
public class WaterBottomLayer : MonoBehaviour
{
	// Token: 0x06000327 RID: 807 RVA: 0x00017E35 File Offset: 0x00016035
	private void Awake()
	{
		this.sr = base.GetComponent<SpriteRenderer>();
	}

	// Token: 0x06000328 RID: 808 RVA: 0x00017E44 File Offset: 0x00016044
	private void Update()
	{
		this.sr.enabled = !GameManager.instance.sniperSkillsSeed;
		base.transform.position = new Vector2(Camera.main.transform.position.x, (float)(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-148) - 13));
	}

	// Token: 0x040003DD RID: 989
	private SpriteRenderer sr;
}
