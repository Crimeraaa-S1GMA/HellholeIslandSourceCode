using System;
using UnityEngine;

// Token: 0x02000098 RID: 152
public class Teleporter : InteractableBlock
{
	// Token: 0x060002AF RID: 687 RVA: 0x00015D84 File Offset: 0x00013F84
	private void Start()
	{
		PlacedBlock component = base.GetComponent<PlacedBlock>();
		if (component.blockMetadata.Count <= 0)
		{
			component.blockMetadata.Add("");
		}
		Guid guid = Guid.NewGuid();
		component.blockMetadata[0] = guid.ToString();
	}

	// Token: 0x060002B0 RID: 688 RVA: 0x00015DD8 File Offset: 0x00013FD8
	private void Update()
	{
		if (base.ReturnBlockInteraction() && !GameManager.instance.fullInventoryOpen)
		{
			GameManager.instance.selectedTeleporter = this;
			GameManager.instance.bed = null;
			GameManager.instance.temporaryTeleporterList.Clear();
			foreach (Teleporter item in Object.FindObjectsOfType<Teleporter>())
			{
				GameManager.instance.temporaryTeleporterList.Add(item);
			}
			GameManager.instance.teleporterSelectedId = GameManager.instance.temporaryTeleporterList.IndexOf(this);
		}
	}
}
