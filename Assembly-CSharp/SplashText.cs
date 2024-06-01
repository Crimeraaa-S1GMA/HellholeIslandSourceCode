using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000088 RID: 136
[RequireComponent(typeof(Text))]
public class SplashText : MonoBehaviour
{
	// Token: 0x06000274 RID: 628 RVA: 0x00014C25 File Offset: 0x00012E25
	private void Awake()
	{
		this.textComp = base.GetComponent<Text>();
		this.UpdateSplash();
	}

	// Token: 0x06000275 RID: 629 RVA: 0x00014C39 File Offset: 0x00012E39
	private void UpdateSplash()
	{
		this.textComp.text = this.splashes[Random.Range(0, this.splashes.Length)];
	}

	// Token: 0x06000276 RID: 630 RVA: 0x00014C5B File Offset: 0x00012E5B
	private void OnEnable()
	{
		this.UpdateSplash();
	}

	// Token: 0x04000355 RID: 853
	[SerializeField]
	private string[] splashes;

	// Token: 0x04000356 RID: 854
	private Text textComp;
}
