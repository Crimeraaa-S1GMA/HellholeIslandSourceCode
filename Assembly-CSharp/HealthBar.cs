using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200004F RID: 79
public class HealthBar : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x06000193 RID: 403 RVA: 0x0000D8B8 File Offset: 0x0000BAB8
	private void Update()
	{
		this.barFill.localScale = new Vector3(Mathf.Clamp((float)GameManager.instance.health / (float)GameManager.instance.ReturnMaxHealth(), 0f, 1f), 1f, 1f);
	}

	// Token: 0x06000194 RID: 404 RVA: 0x0000D905 File Offset: 0x0000BB05
	public void OnPointerEnter(PointerEventData pointerEventData)
	{
		GameManager.instance.tooltipCustom = string.Format("{0}/{1}", GameManager.instance.health, GameManager.instance.ReturnMaxHealth());
	}

	// Token: 0x06000195 RID: 405 RVA: 0x0000D939 File Offset: 0x0000BB39
	public void OnPointerExit(PointerEventData pointerEventData)
	{
		GameManager.instance.tooltipCustom = "";
	}

	// Token: 0x04000218 RID: 536
	[SerializeField]
	private Transform barFill;
}
