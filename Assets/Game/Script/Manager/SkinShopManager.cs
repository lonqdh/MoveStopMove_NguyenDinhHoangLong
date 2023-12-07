using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinShopManager : Singleton<SkinShopManager>
{
    public Button hatBtn;
    public Button pantBtn;
    public Button buySkinBtn;
    public Button closeSkinShopBtn;
    public TextMeshProUGUI skinPrice;
    public TextMeshProUGUI skinStats;
    private UserData data;

    private void Start()
    {
        data = GameManager.Instance.UserData;
        hatBtn.onClick.AddListener(ShowHatSkinList);
        //pantBtn.onClick.AddListener(ShowPantSkinList);
        buySkinBtn.onClick.AddListener(BuyHatSkin);
        closeSkinShopBtn.onClick.AddListener(CloseSkinShop);

    }

    private void CloseSkinShop()
    {
        SkinShopContent.Instance.DespawnHat();
        LevelManager.Instance.player.hatInstance.SetActive(true); //bat lai mu~ dang equip
        UIManager.Instance.skinShopUI.SetActive(false);
        UIManager.Instance.mainMenuUI.SetActive(true);
    }

    public void ShowHatSkinAvailability(SkinButton skinBtn)
    {
        if(data.BoughtHats.Contains((int)skinBtn.hatData.hatType))
        {
            skinPrice.SetText("Equip");
        }
        else
        {
            skinPrice.SetText(skinBtn.hatData.hatPrice.ToString());
        }
    }

    private void BuyHatSkin()
    {
        if(!data.BoughtHats.Contains((int)SkinShopContent.Instance.currentHatType) && data.CurrentCoins >= int.Parse(skinPrice.text))
        {
            data.BoughtHats.Add((int)SkinShopContent.Instance.currentHatType);
            data.CurrentCoins = data.CurrentCoins - int.Parse(skinPrice.text);
            //currentCoinLeft.SetText(data.CurrentCoins.ToString());
            UIManager.Instance.coinText.SetText(data.CurrentCoins.ToString());
            SaveManager.Instance.SaveData(data);
            //currentCoinLeft.SetText(data.CurrentCoins.ToString());
        }
        if (skinPrice.text == "Equip")
        {
            data.EquippedHat = (int)SkinShopContent.Instance.currentHatType;
            SaveManager.Instance.SaveData(data);
            LevelManager.Instance.player.ChangeHat();
        }
    }

    private void ShowPantSkinList()
    {
        
    }

    private void ShowHatSkinList()
    {
        
    }
}
