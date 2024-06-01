using System;
using System.IO;
using UnityEngine;

// Token: 0x0200002F RID: 47
public static class CustomSkinUtility
{
	// Token: 0x060000B5 RID: 181 RVA: 0x00005454 File Offset: 0x00003654
	public static Sprite GetCustomSkinTexture(string fileName)
	{
		string path = Application.dataPath + "/ModData/CustomSkin/" + fileName.Trim();
		if (!File.Exists(path))
		{
			return null;
		}
		byte[] data = File.ReadAllBytes(path);
		Texture2D texture2D = new Texture2D(22, 42);
		texture2D.LoadImage(data);
		if (texture2D.height < 2)
		{
			return null;
		}
		if (texture2D.height % 2 != 0)
		{
			texture2D = CustomSkinUtility.ScaleTexture(texture2D, texture2D.height * 2, texture2D.height * 2);
		}
		Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, 22f, 42f), new Vector2(0.5f, 0.4f), 15f);
		sprite.texture.filterMode = FilterMode.Point;
		sprite.name = fileName + "_CUSTOMSKIN";
		return sprite;
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x0000551C File Offset: 0x0000371C
	private static Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
	{
		Texture2D texture2D = new Texture2D(targetWidth, targetHeight, source.format, false);
		float num = 1f / (float)targetWidth;
		float num2 = 1f / (float)targetHeight;
		for (int i = 0; i < texture2D.height; i++)
		{
			for (int j = 0; j < texture2D.width; j++)
			{
				Color pixelBilinear = source.GetPixelBilinear((float)j / (float)texture2D.width, (float)i / (float)texture2D.height);
				texture2D.SetPixel(j, i, pixelBilinear);
			}
		}
		texture2D.Apply();
		return texture2D;
	}
}
