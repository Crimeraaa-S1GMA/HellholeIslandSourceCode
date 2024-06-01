using System;
using GameJolt.API;
using GameJolt.API.Core;
using UnityEngine;

// Token: 0x02000002 RID: 2
public class AchievementManager : MonoBehaviour
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	private void Awake()
	{
		if (AchievementManager.instance == null)
		{
			AchievementManager.instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
			return;
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06000002 RID: 2 RVA: 0x0000207C File Offset: 0x0000027C
	public void AddAchievement(int achievementServerId)
	{
		if (MonoSingleton<GameJoltAPI>.Instance.CurrentUser != null)
		{
			Trophies.TryUnlock(achievementServerId, delegate(TryUnlockResult result)
			{
				if (result == TryUnlockResult.Failure)
				{
					Debug.LogWarning("Trophy could not be unlocked, for whatever reason");
				}
			});
		}
	}

	// Token: 0x04000001 RID: 1
	public static AchievementManager instance;
}
