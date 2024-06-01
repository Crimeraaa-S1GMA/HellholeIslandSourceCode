using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000092 RID: 146
public class StructureBuilder : MonoBehaviour
{
	// Token: 0x06000290 RID: 656 RVA: 0x000151B4 File Offset: 0x000133B4
	private void Awake()
	{
		this.structureBuilderGui = Object.FindObjectOfType<StructureBuilderGui>();
		this.blockPrevSpriteComp = this.blockPrev.GetComponent<SpriteRenderer>();
		if (GameManager.instance != null)
		{
			Object.Destroy(GameManager.instance.gameObject);
		}
		if (AudioManager.instance != null)
		{
			Object.Destroy(AudioManager.instance.gameObject);
		}
		if (WorldManager.instance != null)
		{
			Object.Destroy(WorldManager.instance.gameObject);
		}
		if (ParticleManager.instance != null)
		{
			Object.Destroy(ParticleManager.instance.gameObject);
		}
		if (AchievementManager.instance != null)
		{
			Object.Destroy(AchievementManager.instance.gameObject);
		}
		if (GameBackground.instance != null)
		{
			Object.Destroy(GameBackground.instance.gameObject);
		}
		if (WorldNameGenerator.instance != null)
		{
			Object.Destroy(WorldNameGenerator.instance.gameObject);
		}
		this.LoadStructure();
	}

	// Token: 0x06000291 RID: 657 RVA: 0x000152A8 File Offset: 0x000134A8
	private void LoadStructure()
	{
		foreach (GameObject obj in this.allBlocks)
		{
			Object.Destroy(obj);
		}
		this.allBlocks.Clear();
		foreach (StructureBlock structureBlock in this.structure.structureBlocks)
		{
			this.InstantiateBlockInBuilder(structureBlock.key, structureBlock.pos, structureBlock.blockType);
		}
	}

	// Token: 0x06000292 RID: 658 RVA: 0x0001535C File Offset: 0x0001355C
	public void LoadBuildFromJson(string json)
	{
		base.StartCoroutine(this.LoadBuildFromJsonAsync(json));
	}

	// Token: 0x06000293 RID: 659 RVA: 0x0001536C File Offset: 0x0001356C
	private IEnumerator LoadBuildFromJsonAsync(string json)
	{
		this.gui = BuilderGui.LoadingProgress;
		try
		{
			Structure structureLoad = JsonUtility.FromJson<Structure>(json);
			this.structure.structureName = structureLoad.structureName;
			this.structure.structureBlocks.Clear();
			yield return null;
			foreach (StructureBlock item in structureLoad.structureBlocks)
			{
				this.structure.structureBlocks.Add(item);
			}
			this.LoadStructure();
			structureLoad = null;
		}
		finally
		{
			this.gui = BuilderGui.NoGui;
		}
		yield break;
		yield break;
	}

	// Token: 0x06000294 RID: 660 RVA: 0x00015384 File Offset: 0x00013584
	private void Update()
	{
		if (this.gui == BuilderGui.NoGui)
		{
			Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - Input.mouseScrollDelta.y, 3f, 20f);
			Camera.main.transform.Translate(new Vector2(Input.GetAxis("Horizontal") * (4f * Time.deltaTime), Input.GetAxis("Vertical") * (4f * Time.deltaTime)));
			this.cursor = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.up;
			this.cursor.x = Mathf.Round(this.cursor.x);
			this.cursor.y = Mathf.Round(this.cursor.y);
			this.blockPrev.transform.position = this.cursor;
			this.blockPrevSpriteComp.sprite = this.ReturnSprite(this.selectedBlock.key, this.selectedBlock.blockType);
			if (Input.GetMouseButton(0))
			{
				this.SpawnBlock(this.selectedBlock.key, this.cursor, this.selectedBlock.blockType);
			}
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				SceneManager.LoadScene("GameLoad");
			}
			if (Input.GetKeyDown(KeyCode.Tab))
			{
				this.structureBuilderGui.PrepareSelectBlockGui();
				this.gui = BuilderGui.SelectBlock;
			}
			if (Input.GetKeyDown(KeyCode.Alpha0))
			{
				this.gui = BuilderGui.SaveLoad;
				return;
			}
		}
		else if (Input.GetKeyDown(KeyCode.Escape))
		{
			this.gui = BuilderGui.NoGui;
		}
	}

	// Token: 0x06000295 RID: 661 RVA: 0x00015524 File Offset: 0x00013724
	public Vector2 ReturnCursor()
	{
		return this.cursor;
	}

	// Token: 0x06000296 RID: 662 RVA: 0x0001552C File Offset: 0x0001372C
	public void SpawnBlock(string id, Vector2 pos, StructureBlockType type)
	{
		switch (type)
		{
		case StructureBlockType.Block:
			if (this.structure.FindBlock(pos, StructureBlockType.Block) == null && this.structure.FindBlock(pos, StructureBlockType.Tree) == null)
			{
				this.structure.structureBlocks.Add(new StructureBlock
				{
					key = id,
					pos = pos,
					blockType = StructureBlockType.Block
				});
				this.InstantiateBlockInBuilder(id, pos, StructureBlockType.Block);
				return;
			}
			break;
		case StructureBlockType.Floor:
			if (this.structure.FindBlock(pos, StructureBlockType.Floor) == null && this.structure.FindBlock(pos, StructureBlockType.Tree) == null)
			{
				this.structure.structureBlocks.Add(new StructureBlock
				{
					key = id,
					pos = pos,
					blockType = StructureBlockType.Floor
				});
				this.InstantiateBlockInBuilder(id, pos, StructureBlockType.Floor);
				return;
			}
			break;
		case StructureBlockType.Tree:
			if (this.structure.FindBlock(pos, StructureBlockType.Block) == null && this.structure.FindBlock(pos, StructureBlockType.Floor) == null && this.structure.FindBlock(pos, StructureBlockType.Tree) == null)
			{
				this.structure.structureBlocks.Add(new StructureBlock
				{
					key = id,
					pos = pos,
					blockType = StructureBlockType.Tree
				});
				this.InstantiateBlockInBuilder(id, pos, StructureBlockType.Tree);
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06000297 RID: 663 RVA: 0x0001565C File Offset: 0x0001385C
	public void InstantiateBlockInBuilder(string id, Vector2 pos, StructureBlockType type)
	{
		GameObject gameObject = Object.Instantiate<GameObject>(this.blockPrev, pos, Quaternion.identity);
		gameObject.GetComponent<SpriteRenderer>().sprite = this.ReturnSprite(id, type);
		gameObject.GetComponent<SpriteRenderer>().sortingOrder = this.ReturnSortingOrder(id, type);
		StructureBuilderBlock structureBuilderBlock = gameObject.AddComponent<StructureBuilderBlock>();
		structureBuilderBlock.structureBuilder = this;
		structureBuilderBlock.type = type;
		this.allBlocks.Add(gameObject);
	}

	// Token: 0x06000298 RID: 664 RVA: 0x000156C8 File Offset: 0x000138C8
	private Sprite ReturnSprite(string id, StructureBlockType type)
	{
		switch (type)
		{
		case StructureBlockType.Block:
			return this.blockRegistryReference.FindBlockByInternalIdentifier(id).spriteFrames[0];
		case StructureBlockType.Floor:
			return this.floorRegistryReference.FindFloorByInternalIdentifier(id).floorSprite;
		case StructureBlockType.Tree:
			return this.treeRegistryReference.FindTreeByInternalIdentifier(id).treeSprite;
		default:
			return this.blockRegistryReference.FindBlockByInternalIdentifier(id).spriteFrames[0];
		}
	}

	// Token: 0x06000299 RID: 665 RVA: 0x00015734 File Offset: 0x00013934
	private int ReturnSortingOrder(string id, StructureBlockType type)
	{
		switch (type)
		{
		case StructureBlockType.Block:
			return this.blockRegistryReference.FindBlockByInternalIdentifier(id).sortingLayer;
		case StructureBlockType.Floor:
			return -5;
		case StructureBlockType.Tree:
			return 0;
		default:
			return this.blockRegistryReference.FindBlockByInternalIdentifier(id).sortingLayer;
		}
	}

	// Token: 0x04000372 RID: 882
	public BlockRegistry blockRegistryReference;

	// Token: 0x04000373 RID: 883
	public FloorRegistry floorRegistryReference;

	// Token: 0x04000374 RID: 884
	public TreeRegistry treeRegistryReference;

	// Token: 0x04000375 RID: 885
	public Structure structure;

	// Token: 0x04000376 RID: 886
	public GameObject blockPrev;

	// Token: 0x04000377 RID: 887
	private SpriteRenderer blockPrevSpriteComp;

	// Token: 0x04000378 RID: 888
	public StructureBlock selectedBlock;

	// Token: 0x04000379 RID: 889
	public BuilderGui gui;

	// Token: 0x0400037A RID: 890
	private Vector2 cursor;

	// Token: 0x0400037B RID: 891
	private StructureBuilderGui structureBuilderGui;

	// Token: 0x0400037C RID: 892
	private List<GameObject> allBlocks = new List<GameObject>();
}
