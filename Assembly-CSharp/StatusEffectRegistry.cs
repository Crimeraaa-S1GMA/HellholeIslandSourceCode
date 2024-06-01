using System;
using UnityEngine;

// Token: 0x0200008D RID: 141
[CreateAssetMenu(fileName = "NewStatusEffectReg", menuName = "Registries/StatusEffectRegistry", order = 7)]
public class StatusEffectRegistry : ScriptableObject
{
	// Token: 0x0600028A RID: 650 RVA: 0x00015108 File Offset: 0x00013308
	public StatusEffect FindStatusEffectByInternalIdentifier(string identifier)
	{
		StatusEffect statusEffect = Array.Find<StatusEffect>(this.statusEffects, (StatusEffect i) => i.idName == identifier);
		if (statusEffect != null)
		{
			return statusEffect;
		}
		return null;
	}

	// Token: 0x04000362 RID: 866
	public StatusEffect[] statusEffects;
}
