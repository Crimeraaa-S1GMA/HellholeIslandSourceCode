using System;

// Token: 0x0200004C RID: 76
[Serializable]
public class ActiveStatusEffect
{
	// Token: 0x0600018A RID: 394 RVA: 0x0000D3E3 File Offset: 0x0000B5E3
	public StatusEffect ReturnStatusEffectObject()
	{
		return GameManager.instance.statusEffectRegistryReference.FindStatusEffectByInternalIdentifier(this.statusId);
	}

	// Token: 0x040001FA RID: 506
	public string statusId;

	// Token: 0x040001FB RID: 507
	public float statusTimeLeft = 30f;
}
