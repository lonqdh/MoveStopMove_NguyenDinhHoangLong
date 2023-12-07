using System;
using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        hatBtn.onClick.AddListener(ShowHatSkinList);
        //pantBtn.onClick.AddListener(ShowPantSkinList);
        buySkinBtn.onClick.AddListener(BuySkin);
        closeSkinShopBtn.onClick.AddListener(CloseSkinShop);

    }

    private void CloseSkinShop()
    {
        SkinShopContent.Instance.DespawnHat();
        UIManager.Instance.skinShopUI.SetActive(false);
        UIManager.Instance.mainMenuUI.SetActive(true);

    }

    private void BuySkin()
    {
        
    }

    private void ShowPantSkinList()
    {
        
    }

    private void ShowHatSkinList()
    {
        
    }
}
