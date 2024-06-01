using System;
using GameJolt.API;
using GameJolt.API.Core;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200009A RID: 154
[RequireComponent(typeof(Text))]
public class TextScript : MonoBehaviour
{
	// Token: 0x060002B4 RID: 692 RVA: 0x00015ED6 File Offset: 0x000140D6
	private void Start()
	{
		this.textComp = base.GetComponent<Text>();
	}

	// Token: 0x060002B5 RID: 693 RVA: 0x00015EE4 File Offset: 0x000140E4
	private void Update()
	{
		switch (this.specialTextType)
		{
		case SpecialText.Time:
			this.textComp.text = GameManager.instance.ReturnFormattedGameTimeAmPm(GameManager.instance.EquippedAccesory("watch"));
			return;
		case SpecialText.GameVersion:
			this.textComp.text = (Application.version ?? "");
			return;
		case SpecialText.Defense:
			this.textComp.text = GameManager.instance.defense.ToString();
			return;
		case SpecialText.FPS:
			this.textComp.text = (1f / Time.unscaledDeltaTime).ToString();
			return;
		case SpecialText.Money:
			this.textComp.text = GameManager.instance.money.ToString();
			return;
		case SpecialText.Day:
			this.textComp.text = string.Format("Day {0}", GameManager.instance.ReturnDay());
			return;
		case SpecialText.SelectedWorld:
			if (WorldManager.instance.worldIdToDelete < GameManager.instance.availableLevels.Count)
			{
				this.textComp.text = WorldManager.instance.worldToDelete.name;
				return;
			}
			this.textComp.text = "";
			return;
		case SpecialText.MapName:
			if (GameManager.instance.worldToDownload != null)
			{
				this.textComp.text = GameManager.instance.worldToDownload.name + " by " + GameManager.instance.worldToDownload.mapAuthor;
				return;
			}
			this.textComp.text = "";
			return;
		case SpecialText.MapDesc:
			if (GameManager.instance.worldToDownload == null)
			{
				this.textComp.text = "";
				return;
			}
			if (GameManager.instance.worldToDownload.mapDescription != string.Empty)
			{
				this.textComp.text = GameManager.instance.worldToDownload.mapDescription;
				return;
			}
			this.textComp.text = "(No description included)";
			return;
		case SpecialText.MapId:
			this.textComp.text = "Map Server ID: " + GameManager.instance.downloadedWorldId;
			return;
		case SpecialText.BossBarName:
			if (GameManager.instance.boss != null)
			{
				this.textComp.text = GameManager.instance.boss.enemy.ReturnEnemyName();
				return;
			}
			break;
		case SpecialText.BossBarLife:
			if (GameManager.instance.boss != null)
			{
				this.textComp.text = Mathf.Max(0f, GameManager.instance.boss.enemy.health).ToString() + "/" + GameManager.instance.boss.enemy.ReturnMaxHealth().ToString();
				return;
			}
			break;
		case SpecialText.CompassPosition:
			if (GameManager.instance.isInMines)
			{
				this.textComp.text = string.Format("X: {0}, Y: {1}", (int)GameManager.instance.playerPos.x - 610, (int)GameManager.instance.playerPos.y - 170);
				return;
			}
			this.textComp.text = string.Format("X: {0}, Y: {1}", (int)GameManager.instance.playerPos.x, (int)GameManager.instance.playerPos.y);
			return;
		case SpecialText.NewsTitle:
			this.textComp.text = GameManager.instance.GetNews(GameManager.instance.newsIndex).header;
			return;
		case SpecialText.NewsContent:
			this.textComp.text = GameManager.instance.GetNews(GameManager.instance.newsIndex).article;
			return;
		case SpecialText.Username:
			if (MonoSingleton<GameJoltAPI>.Instance.CurrentUser != null)
			{
				this.textComp.text = MonoSingleton<GameJoltAPI>.Instance.CurrentUser.Name;
				return;
			}
			this.textComp.text = "Sign in";
			return;
		case SpecialText.LevelGenerationProgress:
			if (WorldManager.instance.isNewWorld)
			{
				this.textComp.text = string.Format("{0}\n\n{1}%", GameManager.instance.levelGenerationStep, Mathf.Clamp((float)Mathf.RoundToInt((float)GameManager.instance.levelGenerationProgress / (float)GameManager.instance.levelGenerationStepsNeeded * 100f), 0f, 100f));
				return;
			}
			this.textComp.text = "Loading world...";
			return;
		case SpecialText.DayBigDisplay:
			this.textComp.text = string.Format("Day - {0}", GameManager.instance.ReturnDay());
			return;
		case SpecialText.InvasionBarName:
			this.textComp.text = GameManager.instance.InvasionName(GameManager.instance.invasionEvent);
			return;
		case SpecialText.InvasionBarProgress:
			this.textComp.text = string.Format("Enemies left: {0}", GameManager.instance.invasionEnemiesLeft);
			return;
		case SpecialText.EmoteName:
			this.textComp.text = GameManager.instance.ReturnCurrentEmote().displayName;
			return;
		case SpecialText.CraftingMenuPage:
			this.textComp.text = string.Format("{0}/{1}", GameManager.instance.recipeTab + 1, GameManager.instance.ReturnMaxRecipePage() + 1);
			return;
		case SpecialText.CreativeMenuPage:
			this.textComp.text = string.Format("{0}/{1}", GameManager.instance.creativeTab + 1, GameManager.instance.ReturnMaxCreativePage() + 1);
			return;
		default:
			this.textComp.text = "";
			break;
		}
	}

	// Token: 0x04000393 RID: 915
	private Text textComp;

	// Token: 0x04000394 RID: 916
	public SpecialText specialTextType;
}
