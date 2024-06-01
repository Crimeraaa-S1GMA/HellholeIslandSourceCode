using System;
using UnityEngine;

// Token: 0x020000B1 RID: 177
public class WorldNameGenerator : MonoBehaviour
{
	// Token: 0x0600036B RID: 875 RVA: 0x00019DB6 File Offset: 0x00017FB6
	private void Awake()
	{
		if (WorldNameGenerator.instance == null)
		{
			WorldNameGenerator.instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
			return;
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x0600036C RID: 876 RVA: 0x00019DE4 File Offset: 0x00017FE4
	public string ReturnRandomWorldName()
	{
		string text = this.adjectives[Random.Range(0, this.adjectives.Length)];
		string text2 = this.nouns[Random.Range(0, this.nouns.Length)];
		string text3 = this.prefixes[Random.Range(0, this.prefixes.Length)];
		if (Random.Range(1, 4) == 1)
		{
			return string.Concat(new string[]
			{
				text3,
				" ",
				text,
				" ",
				text2
			});
		}
		return text + " " + text2;
	}

	// Token: 0x04000429 RID: 1065
	public static WorldNameGenerator instance;

	// Token: 0x0400042A RID: 1066
	public string[] adjectives;

	// Token: 0x0400042B RID: 1067
	public string[] nouns;

	// Token: 0x0400042C RID: 1068
	public string[] prefixes;
}
