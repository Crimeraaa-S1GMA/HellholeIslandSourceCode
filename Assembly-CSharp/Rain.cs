using System;
using UnityEngine;

// Token: 0x0200007C RID: 124
public class Rain : MonoBehaviour
{
	// Token: 0x0600024C RID: 588 RVA: 0x000135AB File Offset: 0x000117AB
	private void Start()
	{
		this.myRenderer = base.GetComponent<SpriteRenderer>();
		this.raindropEmission = this.raindrop.emission;
	}

	// Token: 0x0600024D RID: 589 RVA: 0x000135CA File Offset: 0x000117CA
	private void Update()
	{
		if (GameManager.instance.rain && !GameManager.instance.isInMines)
		{
			this.raindropEmission.rateOverTimeMultiplier = 60f;
			return;
		}
		this.raindropEmission.rateOverTimeMultiplier = 0f;
	}

	// Token: 0x04000325 RID: 805
	private SpriteRenderer myRenderer;

	// Token: 0x04000326 RID: 806
	public ParticleSystem raindrop;

	// Token: 0x04000327 RID: 807
	private ParticleSystem.EmissionModule raindropEmission;
}
