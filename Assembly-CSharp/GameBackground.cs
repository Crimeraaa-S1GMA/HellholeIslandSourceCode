using System;
using UnityEngine;

// Token: 0x02000046 RID: 70
public class GameBackground : MonoBehaviour
{
	// Token: 0x0600011D RID: 285 RVA: 0x00007E59 File Offset: 0x00006059
	private void Awake()
	{
		if (GameBackground.instance == null)
		{
			GameBackground.instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
			return;
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x0600011E RID: 286 RVA: 0x00007E85 File Offset: 0x00006085
	private void Start()
	{
		GameManager.instance.MainMenuCameraZoomStart();
	}

	// Token: 0x0600011F RID: 287 RVA: 0x00007E94 File Offset: 0x00006094
	private void Update()
	{
		if (GameManager.instance.gameState == GameState.MainMenu || GameManager.instance.generatingWorld)
		{
			Camera.main.transform.position = Vector2.down * 900f + new Vector2(Mathf.Sin(Time.time), Mathf.Cos(Time.time)) * 0.5f + Vector3.back * 10f;
		}
		this.background.SetActive(GameManager.instance.gameState == GameState.MainMenu || GameManager.instance.generatingWorld);
	}

	// Token: 0x04000140 RID: 320
	public static GameBackground instance;

	// Token: 0x04000141 RID: 321
	public GameObject background;
}
