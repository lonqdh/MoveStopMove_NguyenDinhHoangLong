using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : Singleton<ShopManager>
{
    public WeaponDataSO WeaponDataSO;
    public TextMeshProUGUI weaponName;
    public TextMeshProUGUI weaponPrice;
    public TextMeshProUGUI weaponAvailability;
    public TextMeshProUGUI currentCoinLeft;
    public Button NextWeapInShopBtn;
    public Button PrevWeapInShopBtn;
    public Button CloseWeaponShopBtn;
    public Button BuyWeaponBtn;
    private UserData data;
    private int currentWeapShownIndex = 0;

    void Start()
    {
        data = GameManager.Instance.UserData;
        NextWeapInShopBtn.onClick.AddListener(NextWeaponInShop);
        PrevWeapInShopBtn.onClick.AddListener(PrevWeaponInShop);
        CloseWeaponShopBtn.onClick.AddListener(CloseShop);
        BuyWeaponBtn.onClick.AddListener(BuyWeapon);
        weaponName.SetText(WeaponDataSO.weaponDataList[currentWeapShownIndex].weaponType.ToString());
        weaponPrice.SetText(WeaponDataSO.weaponDataList[currentWeapShownIndex].price.ToString());
        currentCoinLeft.SetText(data.CurrentCoins.ToString());
    }

    private void Update()
    {
        //if(currentWeapShownIndex > 2)
        // {
        //     currentWeapShownIndex = 0;
        //     weaponName.SetText(WeaponDataSO.weaponDataList[currentWeapShownIndex].weaponType.ToString());
        //     weaponPrice.SetText(WeaponDataSO.weaponDataList[currentWeapShownIndex].price.ToString());

        // }

        SetWeaponsAvailability(currentWeapShownIndex);
    }

    private void SetWeaponsAvailability(int currentWeapIndex)
    {
        if(data.BoughtWeapons.Contains(currentWeapIndex))
        {
            weaponAvailability.SetText("Owned");
            weaponPrice.SetText("Equip");
        }
        else
        {
            weaponAvailability.SetText("Locked");
        }
        //else if(data.BoughtWeapons.Contains(currentWeapIndex) && data.EquippedWeapon == currentWeapShownIndex)
        //{
        //    weaponAvailability.SetText("Owned");
        //    weaponPrice.SetText("Equipped");
        //}
        

    }

    public void BuyWeapon()
    {
        if (!data.BoughtWeapons.Contains(currentWeapShownIndex) && data.CurrentCoins >= WeaponDataSO.weaponDataList[currentWeapShownIndex].price)
        {
            data.BoughtWeapons.Add(currentWeapShownIndex);
            data.CurrentCoins = data.CurrentCoins - WeaponDataSO.weaponDataList[currentWeapShownIndex].price;
            currentCoinLeft.SetText(data.CurrentCoins.ToString());
            UIManager.Instance.coinText.SetText(data.CurrentCoins.ToString());
            SaveManager.Instance.SaveData(data);
            currentCoinLeft.SetText(data.CurrentCoins.ToString());
        }
        if(weaponPrice.text == "Equip")
        {
            data.EquippedWeapon = currentWeapShownIndex;
            SaveManager.Instance.SaveData(data);
            LevelManager.Instance.player.OnInit();
        }
    }

    public void CloseShop()
    {
        UIManager.Instance.weaponShopUI.SetActive(false);
        UIManager.Instance.mainMenuUI.SetActive(true);
    }

    public void NextWeaponInShop()
    {
        if (currentWeapShownIndex == 2)
        {
            currentWeapShownIndex = 0;

            weaponName.SetText(WeaponDataSO.weaponDataList[currentWeapShownIndex].weaponType.ToString());
            weaponPrice.SetText(WeaponDataSO.weaponDataList[currentWeapShownIndex].price.ToString());

        }
        else
        {
            currentWeapShownIndex++;
            weaponName.SetText(WeaponDataSO.weaponDataList[currentWeapShownIndex].weaponType.ToString());
            weaponPrice.SetText(WeaponDataSO.weaponDataList[currentWeapShownIndex].price.ToString());
        }

    }

    public void PrevWeaponInShop()
    {
        if (currentWeapShownIndex == 0)
        {
            currentWeapShownIndex = 2;
            weaponName.SetText(WeaponDataSO.weaponDataList[currentWeapShownIndex].weaponType.ToString());
            weaponPrice.SetText(WeaponDataSO.weaponDataList[currentWeapShownIndex].price.ToString());
        }
        else
        {
            currentWeapShownIndex--;
            weaponName.SetText(WeaponDataSO.weaponDataList[currentWeapShownIndex].weaponType.ToString());
            weaponPrice.SetText(WeaponDataSO.weaponDataList[currentWeapShownIndex].price.ToString());
        }
    }




}
