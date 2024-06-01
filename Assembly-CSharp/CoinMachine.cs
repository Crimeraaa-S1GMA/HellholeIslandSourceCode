using System;
using System.IO;
using UnityEngine;

// Token: 0x0200002C RID: 44
public class CoinMachine : InteractableBlock
{
	// Token: 0x060000AC RID: 172 RVA: 0x00004F8C File Offset: 0x0000318C
	private void Update()
	{
		if (base.ReturnBlockInteraction() && GameManager.instance.ReturnSelectedSlot().ITEM_ID == "squarium" && !WorldPolicy.creativeMode && !WorldPolicy.allowSwitchingGamemodes)
		{
			this.AddCoins();
			GameManager.instance.RemoveCurrentHeldItemOne();
		}
	}

	// Token: 0x060000AD RID: 173 RVA: 0x00004FDC File Offset: 0x000031DC
	private void AddCoins()
	{
		string text = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/TrashTake/SS2Coins/";
		CoinData coinData = new CoinData();
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		if (File.Exists(text + "coinData.json"))
		{
			coinData = JsonUtility.FromJson<CoinData>(File.ReadAllText(text + "coinData.json"));
			if (coinData != null)
			{
				coinData.coinAmount += 3;
			}
			else
			{
				coinData = new CoinData();
			}
			File.WriteAllText(text + "coinData.json", JsonUtility.ToJson(coinData));
			return;
		}
		StreamWriter streamWriter = File.CreateText(text + "coinData.json");
		streamWriter.Write(JsonUtility.ToJson(coinData));
		streamWriter.Close();
	}
}
