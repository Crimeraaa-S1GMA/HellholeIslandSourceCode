using System;
using UnityEngine;

// Token: 0x020000A3 RID: 163
public static class UtilityMath
{
	// Token: 0x06000319 RID: 793 RVA: 0x00017B73 File Offset: 0x00015D73
	public static int NegativePositiveOne()
	{
		if (Random.Range(1, 3) == 1)
		{
			return -1;
		}
		return 1;
	}

	// Token: 0x0600031A RID: 794 RVA: 0x00017B84 File Offset: 0x00015D84
	public static bool IsNumeric(string s)
	{
		float num;
		return float.TryParse(s.TrimStart(new char[]
		{
			'-'
		}), out num);
	}

	// Token: 0x0600031B RID: 795 RVA: 0x00017BAC File Offset: 0x00015DAC
	public static long GetEpochTime()
	{
		return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
	}

	// Token: 0x0600031C RID: 796 RVA: 0x00017BD8 File Offset: 0x00015DD8
	public static string ReverseString(string str)
	{
		char[] array = str.ToCharArray();
		Array.Reverse(array);
		return new string(array);
	}

	// Token: 0x0600031D RID: 797 RVA: 0x00017BEB File Offset: 0x00015DEB
	public static bool RandomChanceInPercentage(float chance)
	{
		return Random.Range(0f, 100f) < Mathf.Clamp(chance, 0f, 100f);
	}

	// Token: 0x0600031E RID: 798 RVA: 0x00017C11 File Offset: 0x00015E11
	public static bool RandomChance(float chance)
	{
		return UtilityMath.RandomChanceInPercentage(chance * 100f);
	}
}
