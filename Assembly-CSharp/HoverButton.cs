using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000052 RID: 82
public class HoverButton : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerMoveHandler, ISelectHandler, IDeselectHandler
{
	// Token: 0x060001A3 RID: 419 RVA: 0x0000ED04 File Offset: 0x0000CF04
	private void Update()
	{
		if (GameManager.instance.hoveringButtonInv && this.hoverOnMeActive && !this.hover && !this.selected)
		{
			GameManager.instance.hoveringButtonInv = false;
			GameManager.instance.selectingButtonInv = false;
			this.hoverOnMeActive = false;
			return;
		}
		if (this.hover || this.selected)
		{
			GameManager.instance.hoveringButtonInv = true;
			if (!GameManager.instance.selectingButtonInv)
			{
				GameManager.instance.selectingButtonInv = this.selected;
			}
			this.hoverOnMeActive = true;
		}
	}

	// Token: 0x060001A4 RID: 420 RVA: 0x0000ED91 File Offset: 0x0000CF91
	public void OnPointerEnter(PointerEventData pointerEventData)
	{
		GameManager.instance.tooltipCustom = this.buttonText;
		this.hover = true;
	}

	// Token: 0x060001A5 RID: 421 RVA: 0x0000EDAA File Offset: 0x0000CFAA
	public void OnPointerMove(PointerEventData pointerEventData)
	{
		GameManager.instance.tooltipCustom = this.buttonText;
		this.hover = true;
	}

	// Token: 0x060001A6 RID: 422 RVA: 0x0000EDC3 File Offset: 0x0000CFC3
	public void OnPointerExit(PointerEventData pointerEventData)
	{
		GameManager.instance.tooltipCustom = string.Empty;
		this.hover = false;
	}

	// Token: 0x060001A7 RID: 423 RVA: 0x0000EDDB File Offset: 0x0000CFDB
	public void OnSelect(BaseEventData baseEventData)
	{
		if (this.selectEnabled)
		{
			this.selected = true;
		}
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x0000EDEC File Offset: 0x0000CFEC
	public void OnDeselect(BaseEventData baseEventData)
	{
		if (this.selectEnabled)
		{
			this.selected = false;
		}
	}

	// Token: 0x060001A9 RID: 425 RVA: 0x0000EE00 File Offset: 0x0000D000
	private void OnDisable()
	{
		if (this.hover)
		{
			GameManager.instance.tooltipCustom = string.Empty;
			this.hover = false;
			GameManager.instance.hoveringButtonInv = false;
			if (this.selectEnabled)
			{
				this.selected = false;
				GameManager.instance.selectingButtonInv = false;
			}
		}
	}

	// Token: 0x04000224 RID: 548
	public string buttonText = string.Empty;

	// Token: 0x04000225 RID: 549
	public bool selectEnabled;

	// Token: 0x04000226 RID: 550
	private bool hover;

	// Token: 0x04000227 RID: 551
	private bool selected;

	// Token: 0x04000228 RID: 552
	private bool hoverOnMeActive;
}
