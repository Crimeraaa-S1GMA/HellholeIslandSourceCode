using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200001A RID: 26
public class BestiaryDrop : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x06000074 RID: 116 RVA: 0x00003D1B File Offset: 0x00001F1B
	private void Awake()
	{
		this.image = base.GetComponent<Image>();
	}

	// Token: 0x06000075 RID: 117 RVA: 0x00003D2C File Offset: 0x00001F2C
	private void Update()
	{
		if (this.dropIndex >= GameManager.instance.bestiaryRegistryReference.bestiaryEntries[this.gui.bestiaryPage].drops.Count)
		{
			this.image.enabled = false;
			this.dropText.text = string.Empty;
			return;
		}
		this.image.enabled = true;
		this.image.sprite = GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(GameManager.instance.bestiaryRegistryReference.bestiaryEntries[this.gui.bestiaryPage].drops[this.dropIndex].DROP_ID).sprite;
		if (GameManager.instance.bestiaryRegistryReference.bestiaryEntries[this.gui.bestiaryPage].drops[this.dropIndex].bestiaryIsHellholeModeDrop)
		{
			this.dropText.color = this.hellholeDropColor;
		}
		else
		{
			this.dropText.color = this.normalDropColor;
		}
		if (GameManager.instance.bestiaryRegistryReference.bestiaryEntries[this.gui.bestiaryPage].drops[this.dropIndex].DROP_STACK_MIN == GameManager.instance.bestiaryRegistryReference.bestiaryEntries[this.gui.bestiaryPage].drops[this.dropIndex].DROP_STACK_MAX - 1)
		{
			this.dropText.text = GameManager.instance.bestiaryRegistryReference.bestiaryEntries[this.gui.bestiaryPage].drops[this.dropIndex].DROP_STACK_MIN.ToString();
			return;
		}
		this.dropText.text = string.Format("{0}-{1}", GameManager.instance.bestiaryRegistryReference.bestiaryEntries[this.gui.bestiaryPage].drops[this.dropIndex].DROP_STACK_MIN, GameManager.instance.bestiaryRegistryReference.bestiaryEntries[this.gui.bestiaryPage].drops[this.dropIndex].DROP_STACK_MAX - 1);
	}

	// Token: 0x06000076 RID: 118 RVA: 0x00003F60 File Offset: 0x00002160
	public void OnPointerEnter(PointerEventData pointerEventData)
	{
		if (this.dropIndex >= GameManager.instance.bestiaryRegistryReference.bestiaryEntries[this.gui.bestiaryPage].drops.Count)
		{
			GameManager.instance.tooltipCustom = string.Empty;
			return;
		}
		if (GameManager.instance.bestiaryRegistryReference.bestiaryEntries[this.gui.bestiaryPage].drops[this.dropIndex].bestiaryIsHellholeModeDrop)
		{
			GameManager.instance.tooltipCustom = string.Format("<size=30>{0}</size>\n{1}% Chance\n(<color=#F24F09>Hellhole Mode Only</color>)", GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(GameManager.instance.bestiaryRegistryReference.bestiaryEntries[this.gui.bestiaryPage].drops[this.dropIndex].DROP_ID).name, 1f / (GameManager.instance.bestiaryRegistryReference.bestiaryEntries[this.gui.bestiaryPage].drops[this.dropIndex].DROP_CHANCE - 1f) * 100f);
			return;
		}
		GameManager.instance.tooltipCustom = string.Format("<size=30>{0}</size>\n{1}% Chance", GameManager.instance.itemRegistryReference.FindItemByInternalIdentifier(GameManager.instance.bestiaryRegistryReference.bestiaryEntries[this.gui.bestiaryPage].drops[this.dropIndex].DROP_ID).name, 1f / (GameManager.instance.bestiaryRegistryReference.bestiaryEntries[this.gui.bestiaryPage].drops[this.dropIndex].DROP_CHANCE - 1f) * 100f);
	}

	// Token: 0x06000077 RID: 119 RVA: 0x00004123 File Offset: 0x00002323
	public void OnPointerExit(PointerEventData pointerEventData)
	{
		GameManager.instance.tooltipCustom = string.Empty;
	}

	// Token: 0x04000050 RID: 80
	[SerializeField]
	private int dropIndex;

	// Token: 0x04000051 RID: 81
	[SerializeField]
	private Text dropText;

	// Token: 0x04000052 RID: 82
	[SerializeField]
	private BestiaryGui gui;

	// Token: 0x04000053 RID: 83
	[SerializeField]
	private Color normalDropColor;

	// Token: 0x04000054 RID: 84
	[SerializeField]
	private Color hellholeDropColor;

	// Token: 0x04000055 RID: 85
	private Image image;
}
