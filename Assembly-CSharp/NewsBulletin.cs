using System;
using UnityEngine;

// Token: 0x02000066 RID: 102
[Serializable]
public class NewsBulletin
{
	// Token: 0x040002C0 RID: 704
	public string header = "Hello World";

	// Token: 0x040002C1 RID: 705
	[TextArea]
	public string article = "Hello!\nThis is a test article";

	// Token: 0x040002C2 RID: 706
	public string link = string.Empty;
}
