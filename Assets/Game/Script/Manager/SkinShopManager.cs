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
