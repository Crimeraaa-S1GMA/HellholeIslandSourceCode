using System;
using UnityEngine;

// Token: 0x02000044 RID: 68
[CreateAssetMenu(fileName = "NewFloorReg", menuName = "Registries/Floor Registry", order = 3)]
public class FloorRegistry : ScriptableObject
{
	// Token: 0x0600011A RID: 282 RVA: 0x00007DD0 File Offset: 0x00005FD0
	public Floor FindFloorByInternalIdentifier(string identifier)
	{
		Floor result = Array.Find<Floor>(this.floors, (Floor i) => i.floorInternalId == "unloaded_floor");
		Floor floor = Array.Find<Floor>(this.floors, (Floor i) => i.floorInternalId == identifier);
		if (floor != null)
		{
			return floor;
		}
		return result;
	}

	// Token: 0x04000137 RID: 311
	public Floor[] floors;

	// Token: 0x04000138 RID: 312
	public Sprite[] stonePathSelection;
}
