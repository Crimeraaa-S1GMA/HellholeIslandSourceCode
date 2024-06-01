using System;
using UnityEngine;

// Token: 0x02000020 RID: 32
[CreateAssetMenu(fileName = "NewBlockReg", menuName = "Registries/Block Registry", order = 2)]
public class BlockRegistry : ScriptableObject
{
	// Token: 0x06000082 RID: 130 RVA: 0x000043FC File Offset: 0x000025FC
	public Block FindBlockByInternalIdentifier(string identifier)
	{
		Block result = Array.Find<Block>(this.blocks, (Block i) => i.blockInternalId == "unloaded_block");
		Block block = Array.Find<Block>(this.blocks, (Block i) => i.blockInternalId == identifier);
		if (block != null)
		{
			return block;
		}
		return result;
	}

	// Token: 0x04000073 RID: 115
	public Block[] blocks;

	// Token: 0x04000074 RID: 116
	[Header("Resources for Special Blocks")]
	public WireSprite[] wireSprites;

	// Token: 0x04000075 RID: 117
	public Sprite[] wheatStages;

	// Token: 0x04000076 RID: 118
	public Sprite[] tomatoStages;

	// Token: 0x04000077 RID: 119
	public Sprite[] carrotStages;
}
