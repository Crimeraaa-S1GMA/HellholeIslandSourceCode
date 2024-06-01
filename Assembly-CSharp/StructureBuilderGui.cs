using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000095 RID: 149
public class StructureBuilderGui : MonoBehaviour
{
	// Token: 0x0600029D RID: 669 RVA: 0x0001581B File Offset: 0x00013A1B
	private void Awake()
	{
		this.structureBuilder = Object.FindObjectOfType<StructureBuilder>();
		this.selectBlockType.onValueChanged.AddListener(delegate(int <p0>)
		{
			this.UpdateSelectBlockId(this.selectBlockType);
		});
	}

	// Token: 0x0600029E RID: 670 RVA: 0x00015844 File Offset: 0x00013A44
	private void Update()
	{
		this.selectBlock.SetActive(this.structureBuilder.gui == BuilderGui.SelectBlock);
		this.saveLoadMenu.SetActive(this.structureBuilder.gui == BuilderGui.SaveLoad);
		this.saveBuild.SetActive(this.structureBuilder.gui == BuilderGui.SaveBuild);
		this.loadBuild.SetActive(this.structureBuilder.gui == BuilderGui.LoadBuild);
		this.loadingProgress.SetActive(this.structureBuilder.gui == BuilderGui.LoadingProgress);
	}

	// Token: 0x0600029F RID: 671 RVA: 0x000158D0 File Offset: 0x00013AD0
	private void UpdateSelectBlockId(Dropdown dropdown)
	{
		this.selectBlockId.ClearOptions();
		List<Dropdown.OptionData> list = new List<Dropdown.OptionData>();
		switch (dropdown.value)
		{
		case 0:
			foreach (Block block in GameManager.instance.blockRegistryReference.blocks)
			{
				list.Add(new Dropdown.OptionData
				{
					text = block.blockInternalId
				});
			}
			this.selectBlockId.AddOptions(list);
			break;
		case 1:
			foreach (Floor floor in GameManager.instance.floorRegistryReference.floors)
			{
				list.Add(new Dropdown.OptionData
				{
					text = floor.floorInternalId
				});
			}
			this.selectBlockId.AddOptions(list);
			break;
		case 2:
			foreach (Tree tree in GameManager.instance.treeRegistryReference.trees)
			{
				list.Add(new Dropdown.OptionData
				{
					text = tree.treeInternalId
				});
			}
			this.selectBlockId.AddOptions(list);
			break;
		}
		this.selectBlockId.value = this.selectBlockId.options.FindIndex((Dropdown.OptionData i) => i.text == this.structureBuilder.selectedBlock.key);
	}

	// Token: 0x060002A0 RID: 672 RVA: 0x00015A15 File Offset: 0x00013C15
	public void PrepareSelectBlockGui()
	{
		this.UpdateSelectBlockId(this.selectBlockType);
		this.selectBlockType.value = (int)this.structureBuilder.selectedBlock.blockType;
	}

	// Token: 0x060002A1 RID: 673 RVA: 0x00015A3E File Offset: 0x00013C3E
	public void SaveBuildGui()
	{
		this.saveBuildId.text = this.structureBuilder.structure.structureName;
		this.structureBuilder.gui = BuilderGui.SaveBuild;
	}

	// Token: 0x060002A2 RID: 674 RVA: 0x00015A68 File Offset: 0x00013C68
	public void CopyStructureJsonToClipboard()
	{
		if (this.saveBuildId.text.Length > 0)
		{
			this.structureBuilder.structure.structureName = this.saveBuildId.text;
			GUIUtility.systemCopyBuffer = JsonUtility.ToJson(this.structureBuilder.structure);
			this.structureBuilder.gui = BuilderGui.SaveLoad;
		}
	}

	// Token: 0x060002A3 RID: 675 RVA: 0x00015AC4 File Offset: 0x00013CC4
	public void LoadBuildGui()
	{
		this.loadBuildJson.text = string.Empty;
		this.structureBuilder.gui = BuilderGui.LoadBuild;
	}

	// Token: 0x060002A4 RID: 676 RVA: 0x00015AE2 File Offset: 0x00013CE2
	public void LoadBuildJson()
	{
		if (this.loadBuildJson.text.Length > 0)
		{
			this.structureBuilder.LoadBuildFromJson(this.loadBuildJson.text);
		}
	}

	// Token: 0x060002A5 RID: 677 RVA: 0x00015B10 File Offset: 0x00013D10
	public void SaveSelectBlock()
	{
		this.structureBuilder.selectedBlock.key = this.selectBlockId.options[this.selectBlockId.value].text;
		this.structureBuilder.selectedBlock.blockType = (StructureBlockType)this.selectBlockType.value;
		this.structureBuilder.gui = BuilderGui.NoGui;
	}

	// Token: 0x060002A6 RID: 678 RVA: 0x00015B74 File Offset: 0x00013D74
	public void CloseMenus()
	{
		this.structureBuilder.gui = BuilderGui.NoGui;
	}

	// Token: 0x04000386 RID: 902
	private StructureBuilder structureBuilder;

	// Token: 0x04000387 RID: 903
	public GameObject selectBlock;

	// Token: 0x04000388 RID: 904
	public GameObject saveLoadMenu;

	// Token: 0x04000389 RID: 905
	public GameObject saveBuild;

	// Token: 0x0400038A RID: 906
	public GameObject loadBuild;

	// Token: 0x0400038B RID: 907
	public GameObject loadingProgress;

	// Token: 0x0400038C RID: 908
	public Dropdown selectBlockId;

	// Token: 0x0400038D RID: 909
	public Dropdown selectBlockType;

	// Token: 0x0400038E RID: 910
	public InputField saveBuildId;

	// Token: 0x0400038F RID: 911
	public InputField loadBuildJson;
}
