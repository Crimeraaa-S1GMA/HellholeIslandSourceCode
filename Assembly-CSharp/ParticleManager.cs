using System;
using UnityEngine;

// Token: 0x0200006D RID: 109
public class ParticleManager : MonoBehaviour
{
	// Token: 0x060001F7 RID: 503 RVA: 0x00010C71 File Offset: 0x0000EE71
	private void Awake()
	{
		if (ParticleManager.instance == null)
		{
			ParticleManager.instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
			return;
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060001F8 RID: 504 RVA: 0x00010CA0 File Offset: 0x0000EEA0
	public GameObject SpawnParticle(string key, Vector2 position)
	{
		GameObject gameObject = Object.Instantiate<GameObject>(Array.Find<ParticleKeyPair>(this.particleKeyPairs, (ParticleKeyPair p) => p.key == key).particle);
		gameObject.transform.position = position;
		Object.Destroy(gameObject, Array.Find<ParticleKeyPair>(this.particleKeyPairs, (ParticleKeyPair p) => p.key == key).duration);
		return gameObject;
	}

	// Token: 0x040002D4 RID: 724
	public static ParticleManager instance;

	// Token: 0x040002D5 RID: 725
	public ParticleKeyPair[] particleKeyPairs;
}
