using System;
using UnityEngine;

// Token: 0x02000033 RID: 51
[CreateAssetMenu(fileName = "NewEmoteReg", menuName = "Registries/EmoteRegistry", order = 9)]
public class EmoteRegistry : ScriptableObject
{
	// Token: 0x040000DB RID: 219
	public Emote[] emotes;
}
