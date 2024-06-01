using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000099 RID: 153
public class TestOptimizationSystem : MonoBehaviour
{
	// Token: 0x060002B2 RID: 690 RVA: 0x00015E68 File Offset: 0x00014068
	private void Update()
	{
		this.blocks.FindAll((GameObject b) => Vector2.Distance(b.transform.position, GameManager.instance.playerPos) <= 9f).ForEach(delegate(GameObject o)
		{
			o.SetActive(true);
		});
	}

	// Token: 0x04000392 RID: 914
	[HideInInspector]
	public List<GameObject> blocks = new List<GameObject>();
}
