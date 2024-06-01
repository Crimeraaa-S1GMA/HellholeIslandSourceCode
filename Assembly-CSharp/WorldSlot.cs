using System;
using System.Collections;
using TrashTake.BuildInfo;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000B3 RID: 179
public class WorldSlot : MonoBehaviour
{
	// Token: 0x0600036F RID: 879 RVA: 0x00019ED1 File Offset: 0x000180D1
	private void Start()
	{
		this.button = base.GetComponent<Button>();
	}

	// Token: 0x06000370 RID: 880 RVA: 0x00019EE0 File Offset: 0x000180E0
	private void Update()
	{
		if (this.index + GameManager.instance.worldsPage * 20 < GameManager.instance.availableLevels.Count && this.world != null)
		{
			if (this.world.versionId < TrashTakeBuildInfo.GAME_VERSION_ID)
			{
				this.worldNameTarget.text = "<size=30>" + this.world.name + "</size>" + string.Format("<size=22>\n{0} | {1} | {2} HP | {3} Hellbux | Day {4} | <color=red>Old version</color></size>", new object[]
				{
					this.WorldSizeDescription(),
					this.GameModeName(),
					20 + this.world.extraHealth,
					this.world.money,
					Mathf.FloorToInt(this.world.time / 1440f) + 1
				});
			}
			else
			{
				this.worldNameTarget.text = "<size=30>" + this.world.name + "</size>" + string.Format("<size=22>\n{0} | {1} | {2} HP | {3} Hellbux | Day {4}</size>", new object[]
				{
					this.WorldSizeDescription(),
					this.GameModeName(),
					20 + this.world.extraHealth,
					this.world.money,
					Mathf.FloorToInt(this.world.time / 1440f) + 1
				});
			}
			this.button.interactable = true;
			this.worldIcon.sprite = GameManager.instance.worldIcons[Mathf.Clamp(this.world.worldIconId, 0, GameManager.instance.worldIcons.Length - 1)];
			this.worldIcon.gameObject.SetActive(true);
			this.deleteButton.SetActive(true);
		}
		else
		{
			this.worldNameTarget.text = "";
			this.button.interactable = false;
			this.worldIcon.gameObject.SetActive(false);
			this.deleteButton.SetActive(false);
		}
		if (this.tempPage != GameManager.instance.worldsPage)
		{
			this.UpdateLevelData();
		}
		this.tempPage = GameManager.instance.worldsPage;
	}

	// Token: 0x06000371 RID: 881 RVA: 0x0001A11B File Offset: 0x0001831B
	private void OnEnable()
	{
		this.UpdateLevelData();
	}

	// Token: 0x06000372 RID: 882 RVA: 0x0001A123 File Offset: 0x00018323
	private void OnDisable()
	{
		base.StopCoroutine("AsyncDataUpdate");
	}

	// Token: 0x06000373 RID: 883 RVA: 0x0001A130 File Offset: 0x00018330
	public void UpdateLevelData()
	{
		base.StartCoroutine("AsyncDataUpdate");
	}

	// Token: 0x06000374 RID: 884 RVA: 0x0001A13E File Offset: 0x0001833E
	private IEnumerator AsyncDataUpdate()
	{
		yield return null;
		if (this.index + GameManager.instance.worldsPage * 20 < GameManager.instance.availableLevels.Count)
		{
			this.world = JsonUtility.FromJson<World>(PlayerPrefs.GetString(GameManager.instance.availableLevels[this.index + GameManager.instance.worldsPage * 20]));
		}
		yield break;
	}

	// Token: 0x06000375 RID: 885 RVA: 0x0001A150 File Offset: 0x00018350
	private string GameModeName()
	{
		if (this.world == null)
		{
			return "";
		}
		if (this.world.creative)
		{
			return "Creative Mode";
		}
		if (this.world.trainwreckMode)
		{
			return "<color=#c633f7>Trainwreck Mode</color>";
		}
		if (this.world.hellholeMode)
		{
			return "<color=#f24f09>Hellhole Mode</color>";
		}
		if (!this.world.hardcoreMode)
		{
			return "Survival Mode";
		}
		return "<color=#f76260>Hardcore Mode</color>";
	}

	// Token: 0x06000376 RID: 886 RVA: 0x0001A1BC File Offset: 0x000183BC
	private string WorldSizeDescription()
	{
		if (this.world == null)
		{
			return "Unknown World";
		}
		float worldScale = this.world.worldScale;
		if (worldScale == 1f)
		{
			return "Large World";
		}
		if (worldScale == 0.7f)
		{
			return "Medium World";
		}
		if (worldScale != 0.5f)
		{
			return "Unknown World";
		}
		return "Small World";
	}

	// Token: 0x04000439 RID: 1081
	public Text worldNameTarget;

	// Token: 0x0400043A RID: 1082
	public int index;

	// Token: 0x0400043B RID: 1083
	private Button button;

	// Token: 0x0400043C RID: 1084
	public GameObject deleteButton;

	// Token: 0x0400043D RID: 1085
	public Image worldIcon;

	// Token: 0x0400043E RID: 1086
	private World world;

	// Token: 0x0400043F RID: 1087
	private int tempPage;
}
