using System;
using UnityEngine;

// Token: 0x02000087 RID: 135
[Serializable]
public sealed class Sound
{
	// Token: 0x0400034E RID: 846
	public string name;

	// Token: 0x0400034F RID: 847
	public AudioClip clip;

	// Token: 0x04000350 RID: 848
	[Range(0f, 1f)]
	public float volume;

	// Token: 0x04000351 RID: 849
	[Range(0.1f, 1f)]
	public float pitch;

	// Token: 0x04000352 RID: 850
	public bool loop;

	// Token: 0x04000353 RID: 851
	public bool ambience;

	// Token: 0x04000354 RID: 852
	[HideInInspector]
	public AudioSource source;
}
