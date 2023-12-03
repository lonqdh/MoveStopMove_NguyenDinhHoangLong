using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : Singleton<ShopManager>
{
    public WeaponDataSO WeaponDataSO;
    public TextMeshProUGUI weaponName;
    public Button NextWeapInShopBtn;
    public Button PrevWeapInShopBtn;
    private int currentWeapShownIndex = 0;

    void Start()
    {
        NextWeapInShopBtn.onClick.AddListener(NextWeaponInShop);
        PrevWeapInShopBtn.onClick.AddListener(PrevWeaponInShop);
        weaponName.SetText(WeaponDataSO.weaponDataList[currentWeapShownIndex].weaponType.ToString());
    }

    private void Update()
    {
       if(currentWeapShownIndex > 2)
        {
            currentWeapShownIndex = 0;
            weaponName.SetText(WeaponDataSO.weaponDataList[currentWeapShownIndex].weaponType.ToString());
        }
    }

    public void NextWeaponInShop()
    {
        currentWeapShownIndex++;
        weaponName.SetText(WeaponDataSO.weaponDataList[currentWeapShownIndex].weaponType.ToString());
    }

    public void PrevWeaponInShop()
    {
        currentWeapShownIndex--;
        weaponName.SetText(WeaponDataSO.weaponDataList[currentWeapShownIndex].weaponType.ToString());
    }




}
