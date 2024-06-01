using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000A2 RID: 162
[RequireComponent(typeof(Animator))]
public class UkrainiumGuardAnimation : MonoBehaviour
{
	// Token: 0x06000315 RID: 789 RVA: 0x00017B30 File Offset: 0x00015D30
	private void Awake()
	{
		UkrainiumGuardAnimation.instance = this;
		this.animator = base.GetComponent<Animator>();
	}

	// Token: 0x06000316 RID: 790 RVA: 0x00017B44 File Offset: 0x00015D44
	private IEnumerator Animation()
	{
		UkrainiumGuardAnimation.instance.animator.Play("Play");
		yield return new WaitForSeconds(3f);
		UkrainiumGuardAnimation.instance.animator.Play("Idle");
		yield break;
	}

	// Token: 0x06000317 RID: 791 RVA: 0x00017B4C File Offset: 0x00015D4C
	public static void PlayAnimation()
	{
		if (UkrainiumGuardAnimation.instance != null)
		{
			UkrainiumGuardAnimation.instance.StartCoroutine("Animation");
		}
	}

	// Token: 0x040003D6 RID: 982
	public static UkrainiumGuardAnimation instance;

	// Token: 0x040003D7 RID: 983
	private Animator animator;
}
