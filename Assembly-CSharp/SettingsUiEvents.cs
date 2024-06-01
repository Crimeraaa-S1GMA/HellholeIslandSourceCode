using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000A0 RID: 160
public class SettingsUiEvents : MonoBehaviour
{
	// Token: 0x060002EA RID: 746 RVA: 0x00016DD8 File Offset: 0x00014FD8
	public void SetResolution(Dropdown change)
	{
		Screen.SetResolution(GameManager.instance.resolutions[change.value].width, GameManager.instance.resolutions[change.value].height, GameManager.instance.fullscreen, GameManager.instance.resolutions[change.value].refreshRate);
		GameManager.instance.SaveSettings();
	}

	// Token: 0x060002EB RID: 747 RVA: 0x00016E55 File Offset: 0x00015055
	private int ReturnScaleId(float guiScale)
	{
		if (guiScale == 1150f)
		{
			return 0;
		}
		if (guiScale == 1350f)
		{
			return 1;
		}
		if (guiScale != 1600f)
		{
			GameManager.instance.guiScale = 1300f;
			return 0;
		}
		return 2;
	}

	// Token: 0x060002EC RID: 748 RVA: 0x00016E87 File Offset: 0x00015087
	private float ReturnGuiScale(int scaleId)
	{
		switch (scaleId)
		{
		case 0:
			return 1150f;
		case 1:
			return 1350f;
		case 2:
			return 1600f;
		default:
			return 1300f;
		}
	}

	// Token: 0x060002ED RID: 749 RVA: 0x00016EB4 File Offset: 0x000150B4
	public void SetGuiScale(Dropdown change)
	{
		float guiScale = this.ReturnGuiScale(change.value);
		GameManager.instance.guiScale = guiScale;
		this.scaler.referenceResolution = Vector2.one * GameManager.instance.guiScale;
		GameManager.instance.SaveSettings();
	}

	// Token: 0x060002EE RID: 750 RVA: 0x00016F04 File Offset: 0x00015104
	private void Start()
	{
		this.scaler = base.GetComponent<CanvasScaler>();
		this.resDropdown.ClearOptions();
		foreach (Resolution resolution in GameManager.instance.resolutions)
		{
			this.resDropdown.options.Add(new Dropdown.OptionData(string.Format("{0}x{1} {2}Hz", resolution.width, resolution.height, resolution.refreshRate)));
			if (resolution.width == Screen.currentResolution.width && resolution.height == Screen.currentResolution.height && resolution.refreshRate == Screen.currentResolution.refreshRate)
			{
				this.resDropdown.SetValueWithoutNotify(this.resDropdown.options.Count - 1);
			}
		}
		this.guiScaleDropdown.SetValueWithoutNotify(this.ReturnScaleId(GameManager.instance.guiScale));
		this.resDropdown.onValueChanged.AddListener(delegate(int <p0>)
		{
			this.SetResolution(this.resDropdown);
		});
		this.guiScaleDropdown.onValueChanged.AddListener(delegate(int <p0>)
		{
			this.SetGuiScale(this.guiScaleDropdown);
		});
	}

	// Token: 0x060002EF RID: 751 RVA: 0x00017068 File Offset: 0x00015268
	private void Update()
	{
		this.scaler.referenceResolution = Vector2.one * GameManager.instance.guiScale;
	}

	// Token: 0x060002F0 RID: 752 RVA: 0x00017089 File Offset: 0x00015289
	public void SetMusic(bool setting)
	{
		GameManager.instance.music = setting;
		GameManager.instance.SaveSettings();
	}

	// Token: 0x060002F1 RID: 753 RVA: 0x000170A0 File Offset: 0x000152A0
	public void SetSound(bool setting)
	{
		GameManager.instance.sound = setting;
		GameManager.instance.SaveSettings();
	}

	// Token: 0x060002F2 RID: 754 RVA: 0x000170B7 File Offset: 0x000152B7
	public void SetAmbience(bool setting)
	{
		GameManager.instance.ambience = setting;
		GameManager.instance.SaveSettings();
	}

	// Token: 0x060002F3 RID: 755 RVA: 0x000170CE File Offset: 0x000152CE
	public void SetBlockAnimations(bool setting)
	{
		GameManager.instance.blockAnimations = setting;
		GameManager.instance.SaveSettings();
	}

	// Token: 0x060002F4 RID: 756 RVA: 0x000170E5 File Offset: 0x000152E5
	public void SetEnemyCorpses(bool setting)
	{
		GameManager.instance.enemyCorpses = setting;
		GameManager.instance.SaveSettings();
	}

	// Token: 0x060002F5 RID: 757 RVA: 0x000170FC File Offset: 0x000152FC
	public void SetTreeTransparency(bool setting)
	{
		GameManager.instance.treeTransparency = setting;
		GameManager.instance.SaveSettings();
	}

	// Token: 0x060002F6 RID: 758 RVA: 0x00017113 File Offset: 0x00015313
	public void SetAutosave(bool setting)
	{
		GameManager.instance.autosave = setting;
		GameManager.instance.SaveSettings();
	}

	// Token: 0x060002F7 RID: 759 RVA: 0x0001712A File Offset: 0x0001532A
	public void SetInteractionTooltips(bool setting)
	{
		GameManager.instance.interactionTooltips = setting;
		GameManager.instance.SaveSettings();
	}

	// Token: 0x060002F8 RID: 760 RVA: 0x00017141 File Offset: 0x00015341
	public void SetEnemyHealthBars(bool setting)
	{
		GameManager.instance.enemyHealthBars = setting;
		GameManager.instance.SaveSettings();
	}

	// Token: 0x060002F9 RID: 761 RVA: 0x00017158 File Offset: 0x00015358
	public void SetBossHealthBars(bool setting)
	{
		GameManager.instance.bossHealthBars = setting;
		GameManager.instance.SaveSettings();
	}

	// Token: 0x060002FA RID: 762 RVA: 0x0001716F File Offset: 0x0001536F
	public void SetFullscreen(bool setting)
	{
		GameManager.instance.fullscreen = setting;
		GameManager.instance.SaveSettings();
		Screen.fullScreen = GameManager.instance.fullscreen;
	}

	// Token: 0x060002FB RID: 763 RVA: 0x00017195 File Offset: 0x00015395
	public void SetPostProcessing(bool setting)
	{
		GameManager.instance.postProcessing = setting;
		GameManager.instance.SaveSettings();
	}

	// Token: 0x060002FC RID: 764 RVA: 0x000171AC File Offset: 0x000153AC
	public void ReloadCustomSkin()
	{
		GameManager.instance.ReloadCustomSkin();
	}

	// Token: 0x040003BE RID: 958
	private CanvasScaler scaler;

	// Token: 0x040003BF RID: 959
	[SerializeField]
	private Dropdown resDropdown;

	// Token: 0x040003C0 RID: 960
	[SerializeField]
	private Dropdown guiScaleDropdown;
}
