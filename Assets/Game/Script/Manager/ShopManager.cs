using Lean.Pool;
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
    [SerializeField] private Transform weaponShopPosition;
    private Weapon weaponModel;
    private UserData data;
    private int currentWeapShownIndex = 0;

    void Start()
    {
        data = GameManager.Instance.UserData;
        NextWeapInShopBtn.onClick.AddListener(NextWeaponInShop);
        PrevWeapInShopBtn.onClick.AddListener(PrevWeaponInShop);
        CloseWeaponShopBtn.onClick.AddListener(CloseShop);
        BuyWeaponBtn.onClick.AddListener(BuyWeapon);
        showWeapon(currentWeapShownIndex);
        currentCoinLeft.SetText(data.CurrentCoins.ToString());
    }

    private void Update()
    {
        SetWeaponsAvailability(currentWeapShownIndex);
    }

    private void showWeapon(int weaponIndex)
    {
        if(weaponModel == null)
        {
            weaponModel = Instantiate(WeaponDataSO.weaponDataList[weaponIndex].weapon, weaponShopPosition.position, weaponShopPosition.rotation);
            weaponModel.transform.parent = weaponShopPosition;
            weaponName.SetText(WeaponDataSO.weaponDataList[weaponIndex].weaponType.ToString());
            weaponPrice.SetText(WeaponDataSO.weaponDataList[weaponIndex].price.ToString());
        }
        else
        {
            Destroy(weaponModel.gameObject);
            weaponModel = Instantiate(WeaponDataSO.weaponDataList[weaponIndex].weapon, weaponShopPosition.position, weaponShopPosition.rotation);
            weaponModel.transform.parent = weaponShopPosition;
            weaponName.SetText(WeaponDataSO.weaponDataList[weaponIndex].weaponType.ToString());
            weaponPrice.SetText(WeaponDataSO.weaponDataList[weaponIndex].price.ToString());
        }
        
    }

    private void SetWeaponsAvailability(int currentWeapIndex)
    {
        if (data.BoughtWeapons.Contains(currentWeapIndex))
        {
            weaponAvailability.SetText("Owned");
            weaponPrice.SetText("Equip");
        }
        else
        {
            weaponAvailability.SetText("Locked");
        }
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
        if (weaponPrice.text == "Equip")
        {
            data.EquippedWeapon = currentWeapShownIndex;
            SaveManager.Instance.SaveData(data);
            LevelManager.Instance.player.OnInit();
        }
    }

    public void CloseShop()
    {
        UIManager.Instance.weaponShopUI.SetActive(false);
        //UIManager.Instance.mainCamera.SetActive(true);
        UIManager.Instance.mainCamera.GetComponent<AudioListener>().enabled = true;
        UIManager.Instance.mainMenuUI.SetActive(true);
    }

    public void NextWeaponInShop()
    {
        if (currentWeapShownIndex == 2)
        {
            currentWeapShownIndex = 0;

            //weaponName.SetText(WeaponDataSO.weaponDataList[currentWeapShownIndex].weaponType.ToString());
            //weaponPrice.SetText(WeaponDataSO.weaponDataList[currentWeapShownIndex].price.ToString());

            showWeapon(currentWeapShownIndex);

        }
        else
        {
            currentWeapShownIndex++;
            showWeapon(currentWeapShownIndex);
        }

    }

    public void PrevWeaponInShop()
    {
        if (currentWeapShownIndex == 0)
        {
            currentWeapShownIndex = 2;
            showWeapon(currentWeapShownIndex);
        }
        else
        {
            currentWeapShownIndex--;
            showWeapon(currentWeapShownIndex);
        }
    }




}
