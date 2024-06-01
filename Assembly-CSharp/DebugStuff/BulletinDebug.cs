using System;
using UnityEngine;

namespace DebugStuff
{
	// Token: 0x020000B5 RID: 181
	public class BulletinDebug : MonoBehaviour
	{
		// Token: 0x06000379 RID: 889 RVA: 0x0001A228 File Offset: 0x00018428
		private void OnGUI()
		{
			this.articleTitle = GUI.TextField(new Rect(2f, 2f, 600f, 20f), this.articleTitle);
			this.articleContent = GUI.TextArea(new Rect(2f, 24f, 600f, 180f), this.articleContent);
			if (GUI.Button(new Rect(2f, 206f, 80f, 20f), "Add"))
			{
				this.bulletinCollection.news.Add(new NewsBulletin
				{
					header = this.articleTitle,
					article = this.articleContent
				});
			}
			if (GUI.Button(new Rect(84f, 206f, 160f, 20f), "Copy to clipboard"))
			{
				GUIUtility.systemCopyBuffer = JsonUtility.ToJson(this.bulletinCollection);
			}
		}

		// Token: 0x04000441 RID: 1089
		[SerializeField]
		private string articleTitle = "";

		// Token: 0x04000442 RID: 1090
		[SerializeField]
		private string articleContent = "";

		// Token: 0x04000443 RID: 1091
		[SerializeField]
		private BulletinCollection bulletinCollection;
	}
}
