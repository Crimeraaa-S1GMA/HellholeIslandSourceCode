using System;
using UnityEngine;

// Token: 0x02000083 RID: 131
public class SelectionSquare : MonoBehaviour
{
	// Token: 0x0600025E RID: 606 RVA: 0x00013A2E File Offset: 0x00011C2E
	private void Start()
	{
		GameManager.instance.enemyBlockingBuild = false;
		this.spriteRenderer = base.GetComponent<SpriteRenderer>();
	}

	// Token: 0x0600025F RID: 607 RVA: 0x00013A48 File Offset: 0x00011C48
	private void Update()
	{
		base.transform.position = GameManager.instance.selectionSquare + Vector2.down * 0.5f;
		if (Input.GetMouseButton(0))
		{
			base.transform.localScale = Vector3.Lerp(base.transform.localScale, Vector3.one * 1.4f, 0.3f);
		}
		else
		{
			base.transform.localScale = Vector3.Lerp(base.transform.localScale, Vector3.one * 1f, 0.3f);
		}
		this.spriteRenderer.enabled = false;
		this.interactableBlockTooltip.enabled = false;
		if (GameManager.instance.ReturnSelectedItem().blockId != string.Empty)
		{
			switch (GameManager.instance.ReturnSelectedItem().blockType)
			{
			case BlockType.Solid:
				if (!GameManager.instance.blockPositions.Contains(GameManager.instance.selectionSquare) && !GameManager.instance.treePositions.Contains(GameManager.instance.selectionSquare) && !GameManager.instance.bedHalfPositions.Contains(GameManager.instance.selectionSquare) && !GameManager.instance.enemyBlockingBuild)
				{
					this.spriteRenderer.enabled = true;
				}
				break;
			case BlockType.Floor:
				if (!GameManager.instance.floorPositions.Contains(GameManager.instance.selectionSquare) && !GameManager.instance.treePositions.Contains(GameManager.instance.selectionSquare))
				{
					this.spriteRenderer.enabled = true;
				}
				break;
			case BlockType.Tree:
				if (!GameManager.instance.treePositions.Contains(GameManager.instance.selectionSquare) && !GameManager.instance.floorPositions.Contains(GameManager.instance.selectionSquare) && !GameManager.instance.blockPositions.Contains(GameManager.instance.selectionSquare) && !GameManager.instance.bedHalfPositions.Contains(GameManager.instance.selectionSquare))
				{
					this.spriteRenderer.enabled = true;
				}
				break;
			}
		}
		string specialUsage = GameManager.instance.ReturnSelectedItem().specialUsage;
		if (specialUsage != null)
		{
			if (!(specialUsage == "Pickaxe"))
			{
				if (!(specialUsage == "Axe"))
				{
					if (!(specialUsage == "Hammer"))
					{
						if (specialUsage == "Hoe")
						{
							if (!GameManager.instance.blockPositions.Contains(GameManager.instance.selectionSquare) && !GameManager.instance.floorPositions.Contains(GameManager.instance.selectionSquare) && !GameManager.instance.treePositions.Contains(GameManager.instance.selectionSquare) && !GameManager.instance.bedHalfPositions.Contains(GameManager.instance.selectionSquare))
							{
								this.spriteRenderer.enabled = true;
							}
						}
					}
					else if (GameManager.instance.floorPositions.Contains(GameManager.instance.selectionSquare))
					{
						this.spriteRenderer.enabled = true;
					}
				}
				else if (GameManager.instance.treePositions.Contains(GameManager.instance.selectionSquare))
				{
					this.spriteRenderer.enabled = true;
				}
			}
			else if (GameManager.instance.blockPositions.Contains(GameManager.instance.selectionSquare))
			{
				this.spriteRenderer.enabled = true;
			}
		}
		if (GameManager.instance.highlightableBlocks.Contains(GameManager.instance.selectionSquare))
		{
			this.spriteRenderer.enabled = true;
			if (!GameManager.instance.waterPositions.Contains(GameManager.instance.selectionSquare) && GameManager.instance.interactionTooltips)
			{
				this.interactableBlockTooltip.enabled = true;
			}
			if (GameManager.instance.farmlands.Find((Farmland f) => f.transform.position == GameManager.instance.selectionSquare) != null)
			{
				if (GameManager.instance.farmlands.Find((Farmland f) => f.transform.position == GameManager.instance.selectionSquare).ReturnCropStage() >= 3)
				{
					this.interactableBlockTooltip.enabled = true;
				}
				else
				{
					this.interactableBlockTooltip.enabled = false;
				}
			}
		}
		if (GameManager.instance.hoveringButtonInv && GameManager.instance.tooltipIndex == -1 && GameManager.instance.tooltipCustom == string.Empty)
		{
			this.spriteRenderer.enabled = false;
		}
	}

	// Token: 0x04000342 RID: 834
	private SpriteRenderer spriteRenderer;

	// Token: 0x04000343 RID: 835
	[SerializeField]
	private SpriteRenderer interactableBlockTooltip;
}
