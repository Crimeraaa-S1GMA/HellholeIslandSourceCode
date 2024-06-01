using System;
using UnityEngine;

// Token: 0x02000019 RID: 25
public class BedHalf : MonoBehaviour
{
	// Token: 0x06000071 RID: 113 RVA: 0x00003CD0 File Offset: 0x00001ED0
	private void Awake()
	{
		GameManager.instance.bedHalfPositions.Add(base.transform.position);
	}

	// Token: 0x06000072 RID: 114 RVA: 0x00003CF1 File Offset: 0x00001EF1
	private void OnDestroy()
	{
		GameManager.instance.bedHalfPositions.Remove(base.transform.position);
	}
}
