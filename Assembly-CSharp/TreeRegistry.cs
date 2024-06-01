using System;
using UnityEngine;

// Token: 0x0200009C RID: 156
[CreateAssetMenu(fileName = "NewTreeReg", menuName = "Registries/Tree Registry", order = 4)]
public class TreeRegistry : ScriptableObject
{
	// Token: 0x060002B7 RID: 695 RVA: 0x00016474 File Offset: 0x00014674
	public Tree FindTreeByInternalIdentifier(string identifier)
	{
		Tree result = Array.Find<Tree>(this.trees, (Tree i) => i.treeInternalId == "standardtree");
		Tree tree = Array.Find<Tree>(this.trees, (Tree i) => i.treeInternalId == identifier);
		if (tree != null)
		{
			return tree;
		}
		return result;
	}

	// Token: 0x040003AD RID: 941
	public Tree[] trees;
}
