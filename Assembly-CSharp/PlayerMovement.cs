using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000077 RID: 119
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
	// Token: 0x0600022D RID: 557 RVA: 0x000123DC File Offset: 0x000105DC
	private void Start()
	{
		this.playerBody = base.GetComponent<Rigidbody2D>();
		base.StartCoroutine("StepSounds");
	}

	// Token: 0x0600022E RID: 558 RVA: 0x000123F8 File Offset: 0x000105F8
	private void Update()
	{
		this.dashCooldown = Mathf.Max(this.dashCooldown - Time.deltaTime, 0f);
		if (this.dashCooldown <= 0f)
		{
			this.dashTimes = 0;
		}
		this.vampireKeyTime = Mathf.Max(this.vampireKeyTime - Time.deltaTime, 0f);
		if (this.vampireKeyTime <= 0f)
		{
			this.vampireKeyPresses = 0;
		}
		this.dash = Vector2.SmoothDamp(this.dash, Vector2.zero, ref this.dashVel, 0.5f);
		if (GameManager.instance.sign == null && GameManager.instance.bed == null && GameManager.instance.chair == null && GameManager.instance.openedChest == null)
		{
			this.playerBody.velocity = new Vector2(this.ReturnMovement(false).x * (3f + GameManager.instance.extraMovementSpeed - (this.isOnSlimeFloor ? 2.5f : 0f)), this.ReturnMovement(false).y * (3f + GameManager.instance.extraMovementSpeed - (this.isOnSlimeFloor ? 2.5f : 0f))) + this.dash;
			if (!this.IsDashing() && Input.GetKeyDown(KeyCode.Space) && GameManager.instance.extraDashLength > 0f && this.dashCooldown <= 0f && (!GameManager.instance.hoveringButtonInv || !GameManager.instance.selectingButtonInv))
			{
				this.dashCooldown = 3f;
				AudioManager.instance.Play("Dash");
				this.dash = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - base.transform.position).normalized * (GameManager.instance.extraDashLength * 0.7f);
			}
			if (Input.GetKeyDown(KeyCode.Z) && this.vampire != null)
			{
				if (this.vampireKeyPresses > 9)
				{
					this.vampire.Death();
				}
				else
				{
					this.vampireKeyPresses++;
					this.vampireKeyTime = 1.4f;
				}
			}
			if (Input.GetKeyUp(KeyCode.Z) && this.vampire == null)
			{
				this.PlayEmote(GameManager.instance.ReturnCurrentEmote());
			}
			if (Mathf.Abs(this.ReturnMovement(true).x) > 0f || Mathf.Abs(this.ReturnMovement(true).y) > 0f)
			{
				this.playerAnimator.SetBool("WALKING", true);
			}
			else
			{
				this.playerAnimator.SetBool("WALKING", false);
			}
		}
		else
		{
			this.playerBody.velocity = this.dash;
			this.playerAnimator.SetBool("WALKING", false);
		}
		this.playerAnimator.SetBool("DASHING", this.IsDashing());
		this.playerHandAnimator.SetBool("HOLDING", GameManager.instance.ReturnSelectedSlot().ITEM_ID != "null");
		if (!GameManager.instance.isInMines)
		{
			base.transform.position = new Vector2(Mathf.Clamp(base.transform.position.x, (float)(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(-115) - 18), (float)(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(115) + 18)), Mathf.Min(base.transform.position.y, (float)(WorldManager.instance.loadedWorld.ScaleIntBasedOnWorldScale(110) + 11)));
		}
		else
		{
			base.transform.position = new Vector2(Mathf.Clamp(base.transform.position.x, 555f, 650f), Mathf.Clamp(base.transform.position.y, 119f, 221f));
		}
		if (GameManager.instance.bed != null)
		{
			base.transform.position = GameManager.instance.bed.transform.position + new Vector2(0.8f, -0.3f);
			base.transform.eulerAngles = Vector3.forward * 90f;
		}
		else
		{
			base.transform.eulerAngles = Vector3.zero;
		}
		if (GameManager.instance.chair != null)
		{
			base.transform.position = GameManager.instance.chair.transform.position + Vector2.down * 0.2f;
		}
		this.FlipPlayerBodyParts();
		this.CheckHealth();
		this.CameraMovement();
	}

	// Token: 0x0600022F RID: 559 RVA: 0x000128ED File Offset: 0x00010AED
	public void PlayEmote(Emote emote)
	{
		if (!this.isPlayingEmote)
		{
			base.StartCoroutine(this.PlayEmoteCoroutine(emote));
		}
	}

	// Token: 0x06000230 RID: 560 RVA: 0x00012905 File Offset: 0x00010B05
	private IEnumerator PlayEmoteCoroutine(Emote emote)
	{
		this.isPlayingEmote = true;
		this.playerAnimator.Play(emote.animationClipId, 0);
		yield return new WaitForSeconds(emote.length);
		this.playerAnimator.Play("Idle", 0);
		this.isPlayingEmote = false;
		yield break;
	}

	// Token: 0x06000231 RID: 561 RVA: 0x0001291B File Offset: 0x00010B1B
	public bool IsDashing()
	{
		return this.dash.magnitude > 0.1f;
	}

	// Token: 0x06000232 RID: 562 RVA: 0x0001292F File Offset: 0x00010B2F
	public bool TryDashAttack()
	{
		if (this.dashTimes <= 2)
		{
			this.dashTimes++;
			return true;
		}
		return false;
	}

	// Token: 0x06000233 RID: 563 RVA: 0x0001294B File Offset: 0x00010B4B
	public bool Flip()
	{
		return Camera.main.ScreenToWorldPoint(Input.mousePosition).x < base.transform.position.x;
	}

	// Token: 0x06000234 RID: 564 RVA: 0x00012973 File Offset: 0x00010B73
	public void BounceOffDash()
	{
		this.dash = this.dash.normalized * -10f;
	}

	// Token: 0x06000235 RID: 565 RVA: 0x00012990 File Offset: 0x00010B90
	private Vector2 ReturnMovement(bool raw)
	{
		if (GameManager.instance.hoveringButtonInv && GameManager.instance.selectingButtonInv)
		{
			return Vector2.zero;
		}
		if (raw)
		{
			return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		}
		return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
	}

	// Token: 0x06000236 RID: 566 RVA: 0x000129F1 File Offset: 0x00010BF1
	private void CheckHealth()
	{
		if (GameManager.instance.health <= 0)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06000237 RID: 567 RVA: 0x00012A0C File Offset: 0x00010C0C
	private void CameraMovement()
	{
		Vector3 position = base.transform.position;
		position.z = -10f;
		if (!GameManager.instance.isInMines)
		{
			position.x = Mathf.Clamp(position.x, WorldManager.instance.loadedWorld.ScaleFloatBasedOnWorldScale(-115f) - 18f + Camera.main.aspect * Camera.main.orthographicSize, WorldManager.instance.loadedWorld.ScaleFloatBasedOnWorldScale(115f) + 18f - Camera.main.aspect * Camera.main.orthographicSize);
			position.y = Mathf.Clamp(position.y, WorldManager.instance.loadedWorld.ScaleFloatBasedOnWorldScale(-148f), WorldManager.instance.loadedWorld.ScaleFloatBasedOnWorldScale(110f));
		}
		else
		{
			position.x = Mathf.Clamp(position.x, 555f + Camera.main.aspect * Camera.main.orthographicSize, 650f - Camera.main.aspect * Camera.main.orthographicSize);
			position.y = Mathf.Clamp(position.y, 130f, 210f);
		}
		if (GameManager.instance.rotativeCameraFeature)
		{
			Camera.main.transform.eulerAngles = new Vector3(Mathf.Clamp((Camera.main.ScreenToWorldPoint(Input.mousePosition) - base.transform.position).y, -10f, 10f) * 3f, Mathf.Clamp((Camera.main.ScreenToWorldPoint(Input.mousePosition) - base.transform.position).x, -10f, 10f) * 3f, 0f);
		}
		else
		{
			Camera.main.transform.eulerAngles = Vector3.zero;
		}
		Camera.main.transform.position = position;
	}

	// Token: 0x06000238 RID: 568 RVA: 0x00012C14 File Offset: 0x00010E14
	private void FlipPlayerBodyParts()
	{
		this.playerChar.localEulerAngles = (this.Flip() ? (Vector3.up * -180f) : Vector3.zero);
		SpriteRenderer[] array = this.playerBodyParts;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = (GameManager.instance.selectedTeleporter == null && !GameManager.instance.HasStatusEffect("invisiblity"));
		}
	}

	// Token: 0x06000239 RID: 569 RVA: 0x00012C8E File Offset: 0x00010E8E
	private IEnumerator StepSounds()
	{
		for (;;)
		{
			if ((Mathf.Abs(this.ReturnMovement(true).x) > 0f || Mathf.Abs(this.ReturnMovement(true).y) > 0f || this.IsDashing()) && GameManager.instance.sign == null && GameManager.instance.bed == null && GameManager.instance.selectedTeleporter == null && GameManager.instance.chair == null && GameManager.instance.openedChest == null)
			{
				AudioManager.instance.Play("Step");
			}
			yield return new WaitForSeconds(0.7f);
		}
		yield break;
	}

	// Token: 0x0600023A RID: 570 RVA: 0x00012C9D File Offset: 0x00010E9D
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("SelSquare"))
		{
			GameManager.instance.enemyBlockingBuild = true;
		}
		if (collision.CompareTag("SlimeFloor"))
		{
			this.isOnSlimeFloor = true;
		}
	}

	// Token: 0x0600023B RID: 571 RVA: 0x00012CCB File Offset: 0x00010ECB
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("SelSquare"))
		{
			GameManager.instance.enemyBlockingBuild = true;
		}
		if (collision.CompareTag("SlimeFloor"))
		{
			this.isOnSlimeFloor = true;
		}
	}

	// Token: 0x0600023C RID: 572 RVA: 0x00012CF9 File Offset: 0x00010EF9
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("SelSquare"))
		{
			GameManager.instance.enemyBlockingBuild = false;
		}
		if (collision.CompareTag("SlimeFloor"))
		{
			this.isOnSlimeFloor = false;
		}
	}

	// Token: 0x04000302 RID: 770
	private Rigidbody2D playerBody;

	// Token: 0x04000303 RID: 771
	public Transform playerChar;

	// Token: 0x04000304 RID: 772
	public SpriteRenderer[] playerBodyParts;

	// Token: 0x04000305 RID: 773
	public Animator playerAnimator;

	// Token: 0x04000306 RID: 774
	public Animator playerHandAnimator;

	// Token: 0x04000307 RID: 775
	public HeldItemVisual heldItemVisual;

	// Token: 0x04000308 RID: 776
	public Vector2 dash = Vector2.zero;

	// Token: 0x04000309 RID: 777
	public Vampire vampire;

	// Token: 0x0400030A RID: 778
	private Vector2 dashVel;

	// Token: 0x0400030B RID: 779
	private int dashTimes;

	// Token: 0x0400030C RID: 780
	private float dashCooldown;

	// Token: 0x0400030D RID: 781
	private bool isPlayingEmote;

	// Token: 0x0400030E RID: 782
	private int vampireKeyPresses;

	// Token: 0x0400030F RID: 783
	private float vampireKeyTime;

	// Token: 0x04000310 RID: 784
	private bool isOnSlimeFloor;
}
