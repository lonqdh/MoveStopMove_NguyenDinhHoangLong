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
    //public bool hatSession;
    //public bool pantSession;
    public int currentSession;


    private void Start()
    {
        currentSession = 3;
        Debug.Log(currentSession);
        data = GameManager.Instance.UserData;
        hatBtn.onClick.AddListener(ShowHatSkinList);
        pantBtn.onClick.AddListener(ShowPantSkinList);
        buySkinBtn.onClick.AddListener(BuySkin);
        closeSkinShopBtn.onClick.AddListener(CloseSkinShop);
    }

    private void CloseSkinShop()
    {
        SkinShopContent.Instance.DespawnHat();
        LevelManager.Instance.player.OnInit();
        LevelManager.Instance.player.hatInstance.SetActive(true); //bat lai mu~ dang equip
        UIManager.Instance.skinShopUI.SetActive(false);
        UIManager.Instance.mainMenuUI.SetActive(true);
        //currentSession = 2;
        Debug.Log(currentSession);
    }

    public void ShowSkinAvailability(SkinButton skinBtn)
    {
        if (currentSession == 1)
        {
            if (data.BoughtHats.Contains((int)skinBtn.hatData.hatType))
            {
                skinPrice.SetText(Constant.EQUIP_SKIN);
            }
            else
            {
                skinPrice.SetText(skinBtn.hatData.hatPrice.ToString());
            }
        }
        else if (currentSession == 0)
        {
            if (data.BoughtPants.Contains((int)skinBtn.pantData.PantType))
            {
                skinPrice.SetText(Constant.EQUIP_SKIN);
            }
            else
            {
                skinPrice.SetText(skinBtn.pantData.PantPrice.ToString());
            }
        }
        else
        {
            return;
        }
    }

    private void BuySkin()
    {
        if (currentSession == 1)
        {
            if (!data.BoughtHats.Contains((int)SkinShopContent.Instance.currentHatType) && data.CurrentCoins >= int.Parse(skinPrice.text))
            {
                data.BoughtHats.Add((int)SkinShopContent.Instance.currentHatType);
                data.CurrentCoins = data.CurrentCoins - int.Parse(skinPrice.text);
                UIManager.Instance.coinText.SetText(data.CurrentCoins.ToString());
                SaveManager.Instance.SaveData(data);
            }
            //OnSkinBought?.Invoke();
            //currentCoinLeft.SetText(data.CurrentCoins.ToString());
            //currentCoinLeft.SetText(data.CurrentCoins.ToString());
        }
        else if (currentSession == 0)
        {
            if (!data.BoughtPants.Contains((int)SkinShopContent.Instance.currentPantType) && data.CurrentCoins >= int.Parse(skinPrice.text))
            {
                data.BoughtPants.Add((int)SkinShopContent.Instance.currentPantType);
                data.CurrentCoins = data.CurrentCoins - int.Parse(skinPrice.text);
                UIManager.Instance.coinText.SetText(data.CurrentCoins.ToString());
                SaveManager.Instance.SaveData(data);
            }
        }

        if (skinPrice.text == Constant.EQUIP_SKIN)
        {
            if (currentSession == 1)
            {
                data.EquippedHat = (int)SkinShopContent.Instance.currentHatType;
                LevelManager.Instance.player.ChangeHat();
            }
            else
            {
                data.EquippedPant = (int)SkinShopContent.Instance.currentPantType;
                LevelManager.Instance.player.ChangePant();
            }

            SaveManager.Instance.SaveData(data);
            LevelManager.Instance.player.SetAttackRange();
            LevelManager.Instance.player.ScaleAttackRangeCircle();
        }


    }

    private void ShowPantSkinList()
    {
        if (currentSession == 0)
        {
            return;
        }

        currentSession = 0;
        Debug.Log(currentSession);
        SkinShopContent.Instance.DespawnHat();
        SkinShopContent.Instance.SpawnSkin();

    }

    private void ShowHatSkinList()
    {
        if (currentSession == 1)
        {
            return;
        }

        currentSession = 1;
        Debug.Log(currentSession);
        SkinShopContent.Instance.SpawnSkin();

    }
}
