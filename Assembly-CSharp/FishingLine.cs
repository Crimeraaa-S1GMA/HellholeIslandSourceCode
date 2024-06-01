using System;
using UnityEngine;

// Token: 0x02000041 RID: 65
public class FishingLine : MonoBehaviour
{
	// Token: 0x06000112 RID: 274 RVA: 0x00007B38 File Offset: 0x00005D38
	private void Update()
	{
		this.line.SetPosition(0, base.transform.position + new Vector2(this.GetLineStartDirection() * GameManager.instance.ReturnSelectedItem().size, GameManager.instance.ReturnSelectedItem().size * 0.5f));
		if (GameManager.instance.lake == null)
		{
			this.lerpToLine = base.transform.position + new Vector2(this.GetLineStartDirection() * GameManager.instance.ReturnSelectedItem().size, GameManager.instance.ReturnSelectedItem().size * 0.5f);
		}
		else if (Mathf.Abs(Mathf.Sin(GameManager.instance.fishingHitTime)) < 0.1f)
		{
			this.lerpToLine = Vector2.Lerp(this.lerpToLine, GameManager.instance.lake.transform.position + Vector2.up * (Mathf.Sin(Time.time * 4f) * 0.25f) + Vector2.down * 0.5f, 0.3f);
		}
		else
		{
			this.lerpToLine = Vector2.Lerp(this.lerpToLine, GameManager.instance.lake.transform.position + Vector2.down * 0.5f, 0.3f);
		}
		this.line.SetPosition(1, this.lerpToLine);
	}

	// Token: 0x06000113 RID: 275 RVA: 0x00007CDC File Offset: 0x00005EDC
	private float GetLineStartDirection()
	{
		if (!(GameManager.instance.playerMovement != null))
		{
			return 0f;
		}
		if (GameManager.instance.playerMovement.Flip())
		{
			return -0.5f;
		}
		return 0.5f;
	}

	// Token: 0x04000132 RID: 306
	[SerializeField]
	private LineRenderer line;

	// Token: 0x04000133 RID: 307
	private Vector2 lerpToLine;
}
