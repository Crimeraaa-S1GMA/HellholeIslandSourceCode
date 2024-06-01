using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000015 RID: 21
public class AmbienceSounds : MonoBehaviour
{
	// Token: 0x0600005D RID: 93 RVA: 0x0000363C File Offset: 0x0000183C
	private void Start()
	{
		base.StartCoroutine("Ambience");
	}

	// Token: 0x0600005E RID: 94 RVA: 0x0000364A File Offset: 0x0000184A
	private IEnumerator Ambience()
	{
		for (;;)
		{
			yield return new WaitForSeconds(Random.Range(5f, 20f));
			this.PlaySound();
		}
		yield break;
	}

	// Token: 0x0600005F RID: 95 RVA: 0x00003659 File Offset: 0x00001859
	private void PlaySound()
	{
		if (!GameManager.instance.generatingWorld && !GameManager.instance.isInMines && GameManager.instance.ambience)
		{
			AudioManager.instance.Play(this.ReturnSound());
		}
	}

	// Token: 0x06000060 RID: 96 RVA: 0x00003690 File Offset: 0x00001890
	private string ReturnSound()
	{
		if (GameManager.instance.IsNight())
		{
			if (Random.Range(1, 501) == 1)
			{
				return "WolfRare";
			}
			switch (Random.Range(1, 6))
			{
			case 1:
				return "Owl1";
			case 2:
				return "Owl2";
			case 3:
				return "Wolf1";
			case 4:
				return "Wolf2";
			case 5:
				return "Frog1";
			default:
				return "Owl1";
			}
		}
		else
		{
			int num = Random.Range(1, 3);
			if (num == 1)
			{
				return "Bird1";
			}
			if (num != 2)
			{
				return "Bird1";
			}
			return "Bird2";
		}
	}
}
