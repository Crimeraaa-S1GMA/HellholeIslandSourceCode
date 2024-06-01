using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000051 RID: 81
public class HeldItemVisual : MonoBehaviour
{
	// Token: 0x0600019C RID: 412 RVA: 0x0000E914 File Offset: 0x0000CB14
	private void Start()
	{
		this.sprite = base.GetComponent<SpriteRenderer>();
		this.itemHitbox = base.GetComponent<BoxCollider2D>();
		this.emissionModule = this.flamethrowerParticle.emission;
		base.StartCoroutine("FlamethrowerSound");
	}

	// Token: 0x0600019D RID: 413 RVA: 0x0000E94C File Offset: 0x0000CB4C
	private void Update()
	{
		this.flamethrowerTime = Mathf.Max(this.flamethrowerTime - Time.deltaTime, 0f);
		if (this.flamethrowerTime > 0f)
		{
			this.emissionModule.rateOverTimeMultiplier = 90f;
			this.flamethrowerHitbox.enabled = true;
			this.flamethrowerHitbox.transform.position = base.transform.position + new Vector2(Mathf.Cos(this.flamethrowerHitbox.transform.eulerAngles.z * 0.0174532924f), Mathf.Sin(this.flamethrowerHitbox.transform.eulerAngles.z * 0.0174532924f)) * 0.5f;
			this.flamethrowerHitbox.transform.right = Camera.main.ScreenToWorldPoint(Input.mousePosition) - base.transform.position;
			if (GameManager.instance.ReturnSelectedItem().specialUsage != "Flamethrower")
			{
				this.flamethrowerTime = 0f;
			}
		}
		else
		{
			if (this.flamethrowerSound)
			{
				AudioManager.instance.Stop("Flamethrower");
				base.StopCoroutine("FlamethrowerSound");
				base.StartCoroutine("FlamethrowerSound");
				this.flamethrowerSound = false;
			}
			this.emissionModule.rateOverTimeMultiplier = 0f;
			this.flamethrowerHitbox.enabled = false;
		}
		base.transform.localScale = new Vector3(0.7f * GameManager.instance.ReturnSelectedItem().size, 0.7f * GameManager.instance.ReturnSelectedItem().size, 1f);
		this.sprite.flipX = this.flip;
		if (GameManager.instance.ReturnSelectedItem().specialUsage == "Fishing" && GameManager.instance.lake != null)
		{
			this.sprite.sprite = GameManager.instance.ReturnSelectedItem().rodThrownSprite;
		}
		else
		{
			this.sprite.sprite = GameManager.instance.ReturnSelectedItem().sprite;
		}
		this.sprite.material = (GameManager.instance.ReturnSelectedItem().spriteIsUnlit ? GameManager.instance.Unlit : GameManager.instance.Lit);
		if (this.flip)
		{
			base.transform.localEulerAngles = Vector3.back * (this.rotation * -1f) - base.transform.parent.localEulerAngles;
		}
		else
		{
			base.transform.localEulerAngles = Vector3.back * this.rotation - base.transform.parent.localEulerAngles;
		}
		this.sprite.enabled = (GameManager.instance.selectedTeleporter == null);
	}

	// Token: 0x0600019E RID: 414 RVA: 0x0000EC41 File Offset: 0x0000CE41
	public void Animation()
	{
		base.StartCoroutine("RotateAnim");
	}

	// Token: 0x0600019F RID: 415 RVA: 0x0000EC4F File Offset: 0x0000CE4F
	private IEnumerator RotateAnim()
	{
		this.itemHitbox.enabled = true;
		Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - base.transform.position;
		if (GameManager.instance.ReturnSelectedItem().useType != ItemUseType.Spin)
		{
			this.rotation = 0f;
		}
		float num;
		for (float i = 0f; i < 10f * Mathf.Clamp(GameManager.instance.ReturnSelectedItem().animationSpeed, 0.1f, 10f); i = num + 1f)
		{
			switch (GameManager.instance.ReturnSelectedItem().useType)
			{
			case ItemUseType.Regular:
				this.rotation += 9f / Mathf.Clamp(GameManager.instance.ReturnSelectedItem().animationSpeed, 0.1f, 10f);
				break;
			case ItemUseType.Eat:
				this.rotation -= 10f / Mathf.Clamp(GameManager.instance.ReturnSelectedItem().animationSpeed, 0.1f, 10f);
				break;
			case ItemUseType.Hold:
				this.rotation = 0f;
				break;
			case ItemUseType.Aim:
				this.rotation = this.ReturnAim(dir);
				break;
			case ItemUseType.Spin:
				this.rotation += 300f * Time.deltaTime;
				break;
			case ItemUseType.Flute:
				this.rotation = -130f;
				break;
			}
			if (GameManager.instance.ReturnSelectedItem().isWeapon && GameManager.instance.ReturnSelectedItem().meleeHit)
			{
				this.use.IterateEnemies(GameManager.instance.enemyList);
			}
			yield return new WaitForSeconds(0.01f);
			num = i;
		}
		this.itemHitbox.enabled = false;
		yield return new WaitForSeconds(0.01f);
		this.rotation = 0f;
		yield break;
	}

	// Token: 0x060001A0 RID: 416 RVA: 0x0000EC5E File Offset: 0x0000CE5E
	private IEnumerator FlamethrowerSound()
	{
		for (;;)
		{
			this.flamethrowerSound = false;
			while (this.flamethrowerTime <= 0f)
			{
				yield return null;
			}
			this.flamethrowerSound = true;
			AudioManager.instance.Play("Flamethrower");
			yield return new WaitForSeconds(3f);
		}
		yield break;
	}

	// Token: 0x060001A1 RID: 417 RVA: 0x0000EC70 File Offset: 0x0000CE70
	private float ReturnAim(Vector2 dir)
	{
		float num = Mathf.Atan2(dir.y, dir.x) * 57.29578f * -1f;
		if (GameManager.instance.playerMovement != null)
		{
			if (GameManager.instance.playerMovement.Flip())
			{
				num += 180f;
				num *= -1f;
			}
			if (GameManager.instance.bed != null)
			{
				num += (float)(GameManager.instance.playerMovement.Flip() ? -90 : 90);
			}
		}
		return num;
	}

	// Token: 0x0400021A RID: 538
	[HideInInspector]
	public bool flip;

	// Token: 0x0400021B RID: 539
	private SpriteRenderer sprite;

	// Token: 0x0400021C RID: 540
	public float rotation;

	// Token: 0x0400021D RID: 541
	public HeldItemUse use;

	// Token: 0x0400021E RID: 542
	private BoxCollider2D itemHitbox;

	// Token: 0x0400021F RID: 543
	public ParticleSystem flamethrowerParticle;

	// Token: 0x04000220 RID: 544
	private ParticleSystem.EmissionModule emissionModule;

	// Token: 0x04000221 RID: 545
	public BoxCollider2D flamethrowerHitbox;

	// Token: 0x04000222 RID: 546
	public float flamethrowerTime;

	// Token: 0x04000223 RID: 547
	private bool flamethrowerSound;
}
