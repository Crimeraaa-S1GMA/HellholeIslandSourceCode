using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000026 RID: 38
public class ButtonEffect : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	// Token: 0x06000093 RID: 147 RVA: 0x00004805 File Offset: 0x00002A05
	private void Awake()
	{
		this.button = base.GetComponent<Button>();
	}

	// Token: 0x06000094 RID: 148 RVA: 0x00004813 File Offset: 0x00002A13
	private void Update()
	{
		if (this.isSelected && this.button.interactable)
		{
			this.buttonText.color = this.selectColor;
			return;
		}
		this.buttonText.color = this.normalColor;
	}

	// Token: 0x06000095 RID: 149 RVA: 0x0000484D File Offset: 0x00002A4D
	public void OnPointerClick(PointerEventData pointerEventData)
	{
		this.isSelected = false;
	}

	// Token: 0x06000096 RID: 150 RVA: 0x00004856 File Offset: 0x00002A56
	public void OnPointerEnter(PointerEventData pointerEventData)
	{
		this.isSelected = true;
	}

	// Token: 0x06000097 RID: 151 RVA: 0x0000485F File Offset: 0x00002A5F
	public void OnPointerExit(PointerEventData pointerEventData)
	{
		this.isSelected = false;
	}

	// Token: 0x040000AC RID: 172
	public Text buttonText;

	// Token: 0x040000AD RID: 173
	public Color selectColor;

	// Token: 0x040000AE RID: 174
	public Color normalColor;

	// Token: 0x040000AF RID: 175
	private bool isSelected;

	// Token: 0x040000B0 RID: 176
	private Button button;
}
