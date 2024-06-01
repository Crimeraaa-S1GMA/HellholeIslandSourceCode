using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200008C RID: 140
[RequireComponent(typeof(Image))]
public class StatusEffectDisplay : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x06000285 RID: 645 RVA: 0x00014FD8 File Offset: 0x000131D8
	private void Awake()
	{
		this.selfImage = base.GetComponent<Image>();
	}

	// Token: 0x06000286 RID: 646 RVA: 0x00014FE8 File Offset: 0x000131E8
	private void Update()
	{
		if (GameManager.instance.StatusEffectIndex(this.statusEffectIndex) != null)
		{
			this.selfImage.sprite = GameManager.instance.StatusEffectIndex(this.statusEffectIndex).ReturnStatusEffectObject().icon;
			this.durationText.text = GameManager.instance.StatusEffectIndex(this.statusEffectIndex).statusTimeLeft.ToString() + "s";
			return;
		}
		this.selfImage.sprite = this.noSprite;
		this.durationText.text = string.Empty;
	}

	// Token: 0x06000287 RID: 647 RVA: 0x00015080 File Offset: 0x00013280
	public void OnPointerEnter(PointerEventData pointerEventData)
	{
		if (GameManager.instance.StatusEffectIndex(this.statusEffectIndex) != null)
		{
			GameManager.instance.tooltipCustom = "<size=30>" + GameManager.instance.StatusEffectIndex(this.statusEffectIndex).ReturnStatusEffectObject().displayName + "</size>\n" + GameManager.instance.StatusEffectIndex(this.statusEffectIndex).ReturnStatusEffectObject().desc;
		}
	}

	// Token: 0x06000288 RID: 648 RVA: 0x000150EC File Offset: 0x000132EC
	public void OnPointerExit(PointerEventData pointerEventData)
	{
		GameManager.instance.tooltipCustom = "";
	}

	// Token: 0x0400035E RID: 862
	private Image selfImage;

	// Token: 0x0400035F RID: 863
	public Text durationText;

	// Token: 0x04000360 RID: 864
	public Sprite noSprite;

	// Token: 0x04000361 RID: 865
	public int statusEffectIndex;
}
