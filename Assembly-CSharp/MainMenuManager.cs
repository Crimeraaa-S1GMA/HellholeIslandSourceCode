using System;
using GameJolt.API;
using GameJolt.API.Core;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000063 RID: 99
public class MainMenuManager : MonoBehaviour
{
	// Token: 0x060001CE RID: 462 RVA: 0x0000FCEC File Offset: 0x0000DEEC
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && GameManager.instance.gameState == GameState.MainMenu)
		{
			if (GameManager.instance.mainMenuSubmenu == 0)
			{
				GameManager.instance.mainMenuSubmenu = 7;
			}
			else if (GameManager.instance.mainMenuSubmenu == 7)
			{
				GameManager.instance.mainMenuSubmenu = 0;
			}
		}
		this.mainMenu.SetActive(GameManager.instance.mainMenuSubmenu == 0);
		this.worldCreate.SetActive(GameManager.instance.mainMenuSubmenu == 1);
		this.worldList.SetActive(GameManager.instance.mainMenuSubmenu == 2);
		this.worldLoadError.SetActive(GameManager.instance.mainMenuSubmenu == 3);
		this.worldCreateError.SetActive(GameManager.instance.mainMenuSubmenu == 4);
		this.about.SetActive(GameManager.instance.mainMenuSubmenu == 5);
		this.worldDeleteConfirmation.SetActive(GameManager.instance.mainMenuSubmenu == 6);
		this.quitConfirmation.SetActive(GameManager.instance.mainMenuSubmenu == 7);
		this.settings.SetActive(GameManager.instance.mainMenuSubmenu == 8);
		this.loading.SetActive(GameManager.instance.mainMenuSubmenu == 9);
		this.successfulUpload.SetActive(GameManager.instance.mainMenuSubmenu == 10);
		this.uploadedMaps.SetActive(GameManager.instance.mainMenuSubmenu == 11);
		this.downloadSuccess.SetActive(GameManager.instance.mainMenuSubmenu == 12);
		this.downloadFail.SetActive(GameManager.instance.mainMenuSubmenu == 13);
		this.uploadScreen.SetActive(GameManager.instance.mainMenuSubmenu == 14);
		this.mapPreview.SetActive(GameManager.instance.mainMenuSubmenu == 15);
		this.uploadFailed.SetActive(GameManager.instance.mainMenuSubmenu == 16);
		this.uploadingProgress.SetActive(GameManager.instance.mainMenuSubmenu == 17);
		this.serverError.SetActive(GameManager.instance.mainMenuSubmenu == 18);
		this.worldIncompatible.SetActive(GameManager.instance.mainMenuSubmenu == 19);
		this.worldsImported.SetActive(GameManager.instance.mainMenuSubmenu == 20);
		this.newsScreen.SetActive(GameManager.instance.mainMenuSubmenu == 21);
		this.newsLoading.SetActive(GameManager.instance.mainMenuSubmenu == 22);
		this.accountOptions.SetActive(GameManager.instance.mainMenuSubmenu == 23);
		this.accountLinked.SetActive(GameManager.instance.mainMenuSubmenu == 24);
		this.signingIn.SetActive(GameManager.instance.mainMenuSubmenu == 25);
		this.signIn.SetActive(GameManager.instance.mainMenuSubmenu == 26);
		this.signInFailed.SetActive(GameManager.instance.mainMenuSubmenu == 27);
		this.delWorld.SetActive(GameManager.instance.mainMenuSubmenu == 28);
		this.renameWorld.SetActive(GameManager.instance.mainMenuSubmenu == 29);
		this.oldWorldWarning.SetActive(GameManager.instance.mainMenuSubmenu == 30);
		this.bestiaryMenu.SetActive(GameManager.instance.mainMenuSubmenu == 31);
		this.confirmSignOut.SetActive(GameManager.instance.mainMenuSubmenu == 32);
		this.createWorldButton.SetActive(GameManager.instance.availableLevels.Count < 200);
		this.advancedCreation.SetActive(GameManager.instance.showAdvancedWorldgen);
		this.advancedButtonText.text = (GameManager.instance.showAdvancedWorldgen ? "Hide" : "Show");
		this.mapsLoadingMessage.SetActive(GameManager.instance.loadingMaps);
		this.mapsRefresh.SetActive(!GameManager.instance.loadingMaps);
		this.mapsPrev.SetActive(!GameManager.instance.loadingMaps);
		this.mapsNext.SetActive(!GameManager.instance.loadingMaps);
		this.mapsSort.SetActive(!GameManager.instance.loadingMaps);
		this.newsLink.SetActive(GameManager.instance.GetNews(GameManager.instance.newsIndex).link != string.Empty);
		if (MonoSingleton<GameJoltAPI>.Instance != null)
		{
			this.loginButton.SetActive(MonoSingleton<GameJoltAPI>.Instance.CurrentUser == null);
			this.accountButton.SetActive(MonoSingleton<GameJoltAPI>.Instance.CurrentUser != null);
		}
		this.playerSkinPreview.sprite = GameManager.instance.playerSkin[0];
	}

	// Token: 0x0400028D RID: 653
	[SerializeField]
	private GameObject mainMenu;

	// Token: 0x0400028E RID: 654
	[SerializeField]
	private GameObject worldCreate;

	// Token: 0x0400028F RID: 655
	[SerializeField]
	private GameObject worldList;

	// Token: 0x04000290 RID: 656
	[SerializeField]
	private GameObject worldLoadError;

	// Token: 0x04000291 RID: 657
	[SerializeField]
	private GameObject worldCreateError;

	// Token: 0x04000292 RID: 658
	[SerializeField]
	private GameObject about;

	// Token: 0x04000293 RID: 659
	[SerializeField]
	private GameObject worldDeleteConfirmation;

	// Token: 0x04000294 RID: 660
	[SerializeField]
	private GameObject quitConfirmation;

	// Token: 0x04000295 RID: 661
	[SerializeField]
	private GameObject settings;

	// Token: 0x04000296 RID: 662
	[SerializeField]
	private GameObject loading;

	// Token: 0x04000297 RID: 663
	[SerializeField]
	private GameObject successfulUpload;

	// Token: 0x04000298 RID: 664
	[SerializeField]
	private GameObject uploadedMaps;

	// Token: 0x04000299 RID: 665
	[SerializeField]
	private GameObject downloadSuccess;

	// Token: 0x0400029A RID: 666
	[SerializeField]
	private GameObject downloadFail;

	// Token: 0x0400029B RID: 667
	[SerializeField]
	private GameObject uploadScreen;

	// Token: 0x0400029C RID: 668
	[SerializeField]
	private GameObject mapPreview;

	// Token: 0x0400029D RID: 669
	[SerializeField]
	private GameObject uploadFailed;

	// Token: 0x0400029E RID: 670
	[SerializeField]
	private GameObject uploadingProgress;

	// Token: 0x0400029F RID: 671
	[SerializeField]
	private GameObject serverError;

	// Token: 0x040002A0 RID: 672
	[SerializeField]
	private GameObject worldIncompatible;

	// Token: 0x040002A1 RID: 673
	[SerializeField]
	private GameObject worldsImported;

	// Token: 0x040002A2 RID: 674
	[SerializeField]
	private GameObject newsScreen;

	// Token: 0x040002A3 RID: 675
	[SerializeField]
	private GameObject newsLoading;

	// Token: 0x040002A4 RID: 676
	[SerializeField]
	private GameObject accountOptions;

	// Token: 0x040002A5 RID: 677
	[SerializeField]
	private GameObject accountLinked;

	// Token: 0x040002A6 RID: 678
	[SerializeField]
	private GameObject signingIn;

	// Token: 0x040002A7 RID: 679
	[SerializeField]
	private GameObject signIn;

	// Token: 0x040002A8 RID: 680
	[SerializeField]
	private GameObject signInFailed;

	// Token: 0x040002A9 RID: 681
	[SerializeField]
	private GameObject delWorld;

	// Token: 0x040002AA RID: 682
	[SerializeField]
	private GameObject renameWorld;

	// Token: 0x040002AB RID: 683
	[SerializeField]
	private GameObject oldWorldWarning;

	// Token: 0x040002AC RID: 684
	[SerializeField]
	private GameObject bestiaryMenu;

	// Token: 0x040002AD RID: 685
	[SerializeField]
	private GameObject confirmSignOut;

	// Token: 0x040002AE RID: 686
	[SerializeField]
	private GameObject createWorldButton;

	// Token: 0x040002AF RID: 687
	[SerializeField]
	private GameObject advancedCreation;

	// Token: 0x040002B0 RID: 688
	[SerializeField]
	private GameObject loginButton;

	// Token: 0x040002B1 RID: 689
	[SerializeField]
	private GameObject accountButton;

	// Token: 0x040002B2 RID: 690
	[SerializeField]
	private GameObject mapsScroll;

	// Token: 0x040002B3 RID: 691
	[SerializeField]
	private GameObject mapsLoadingMessage;

	// Token: 0x040002B4 RID: 692
	[SerializeField]
	private GameObject mapsRefresh;

	// Token: 0x040002B5 RID: 693
	[SerializeField]
	private GameObject mapsPrev;

	// Token: 0x040002B6 RID: 694
	[SerializeField]
	private GameObject mapsNext;

	// Token: 0x040002B7 RID: 695
	[SerializeField]
	private GameObject mapsSort;

	// Token: 0x040002B8 RID: 696
	[SerializeField]
	private GameObject newsLink;

	// Token: 0x040002B9 RID: 697
	[SerializeField]
	private Text advancedButtonText;

	// Token: 0x040002BA RID: 698
	[SerializeField]
	private Image playerSkinPreview;
}
