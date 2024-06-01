using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000085 RID: 133
public class SimpleScaler : MonoBehaviour
{
	// Token: 0x06000268 RID: 616 RVA: 0x000140ED File Offset: 0x000122ED
	private void Start()
	{
		this.scaler = base.GetComponent<CanvasScaler>();
	}

	// Token: 0x06000269 RID: 617 RVA: 0x000140FB File Offset: 0x000122FB
	private void Update()
	{
		this.scaler.referenceResolution = Vector2.one * GameManager.instance.guiScale;
	}

	// Token: 0x04000347 RID: 839
	private CanvasScaler scaler;
}
