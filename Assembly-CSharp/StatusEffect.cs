using System;
using UnityEngine;

// Token: 0x0200008E RID: 142
[Serializable]
public class StatusEffect
{
	// Token: 0x04000363 RID: 867
	public string idName;

	// Token: 0x04000364 RID: 868
	public string displayName;

	// Token: 0x04000365 RID: 869
	[TextArea]
	public string desc;

	// Token: 0x04000366 RID: 870
	public Sprite icon;

	// Token: 0x04000367 RID: 871
	public float defaultDurationSeconds = 30f;
}
