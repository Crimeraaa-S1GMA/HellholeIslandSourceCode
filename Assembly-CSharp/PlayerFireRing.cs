using System;
using UnityEngine;

// Token: 0x02000075 RID: 117
public class PlayerFireRing : MonoBehaviour
{
	// Token: 0x06000228 RID: 552 RVA: 0x00012308 File Offset: 0x00010508
	private void Update()
	{
		this.fireRing.transform.Rotate(Vector3.forward * 240f * Time.deltaTime);
		this.fireRing.SetActive(GameManager.instance.HasStatusEffect("inferno_emblem"));
	}

	// Token: 0x040002FF RID: 767
	[SerializeField]
	private GameObject fireRing;
}
