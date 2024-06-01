using System;
using System.Collections;
using Discord;
using UnityEngine;

// Token: 0x0200008B RID: 139
public class StatusController : MonoBehaviour
{
	// Token: 0x0600027E RID: 638 RVA: 0x00014E56 File Offset: 0x00013056
	private void Awake()
	{
		if (StatusController.instance == null)
		{
			StatusController.instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
			return;
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x0600027F RID: 639 RVA: 0x00014E84 File Offset: 0x00013084
	private void Start()
	{
		this.secondsSinceEpoch = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
		try
		{
			this.discord = new Discord(1055943872473202770L, 1UL);
		}
		catch
		{
		}
		base.StartCoroutine("UpdateStatus");
	}

	// Token: 0x06000280 RID: 640 RVA: 0x00014EF0 File Offset: 0x000130F0
	private void Update()
	{
		if (this.discord != null)
		{
			try
			{
				this.discord.RunCallbacks();
			}
			catch
			{
			}
		}
	}

	// Token: 0x06000281 RID: 641 RVA: 0x00014F28 File Offset: 0x00013128
	private void OnApplicationQuit()
	{
		if (this.discord != null)
		{
			this.discord.Dispose();
		}
	}

	// Token: 0x06000282 RID: 642 RVA: 0x00014F3D File Offset: 0x0001313D
	private IEnumerator UpdateStatus()
	{
		for (;;)
		{
			this.DoStatusUpdate();
			yield return new WaitForSeconds(5f);
		}
		yield break;
	}

	// Token: 0x06000283 RID: 643 RVA: 0x00014F4C File Offset: 0x0001314C
	public void DoStatusUpdate()
	{
		if (this.discord != null)
		{
			ActivityManager activityManager = this.discord.GetActivityManager();
			Activity activity = default(Activity);
			activity.Timestamps.Start = (long)this.secondsSinceEpoch;
			activity.Assets.LargeImage = "hellhole_icon";
			activity.Assets.LargeText = "Hellhole Island";
			Activity activity2 = activity;
			activityManager.UpdateActivity(activity2, delegate(Result res)
			{
			});
		}
	}

	// Token: 0x0400035B RID: 859
	public static StatusController instance;

	// Token: 0x0400035C RID: 860
	private Discord discord;

	// Token: 0x0400035D RID: 861
	private int secondsSinceEpoch;
}
