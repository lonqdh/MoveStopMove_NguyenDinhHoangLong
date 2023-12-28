using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : Singleton<ShopManager>
{
    public WeaponDataSO WeaponDataSO;
    public TextMeshProUGUI weaponName;
    public TextMeshProUGUI weaponPrice;
    public TextMeshProUGUI weaponAvailability;
    public TextMeshProUGUI weaponStats;
    public TextMeshProUGUI currentCoinLeft;
    public Button NextWeapInShopBtn;
    public Button PrevWeapInShopBtn;
    public Button CloseWeaponShopBtn;
    public Button BuyWeaponBtn;
    [SerializeField] private Transform weaponShopPosition;
    private Weapon weaponModel;
    private UserData data;
    //private int currentWeapShownIndex = 0;
    private WeaponType currentWeapShownIndex = (WeaponType)0;


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

    private void showWeapon(WeaponType currentWeaponType)
    {
        if(weaponModel == null)
        {
            weaponModel = Instantiate(WeaponDataSO.weaponDataList[(int)currentWeaponType].weapon, weaponShopPosition.position, weaponShopPosition.rotation);
            weaponModel.transform.parent = weaponShopPosition;
            weaponName.SetText(WeaponDataSO.weaponDataList[(int)currentWeaponType].weaponType.ToString());
            weaponPrice.SetText(WeaponDataSO.weaponDataList[(int)currentWeaponType].price.ToString());
            weaponStats.SetText("+" + WeaponDataSO.weaponDataList[(int)currentWeaponType].attackSpeed.ToString() + " AtkSpeed" + " +" + WeaponDataSO.weaponDataList[(int)currentWeaponType].autoAttackRange.ToString() + "AtkRange");
        }
        else
        {
            Destroy(weaponModel.gameObject);
            weaponModel = Instantiate(WeaponDataSO.weaponDataList[(int)currentWeaponType].weapon, weaponShopPosition.position, weaponShopPosition.rotation);
            weaponModel.transform.parent = weaponShopPosition;
            weaponName.SetText(WeaponDataSO.weaponDataList[(int)currentWeaponType].weaponType.ToString());
            weaponPrice.SetText(WeaponDataSO.weaponDataList[(int)currentWeaponType].price.ToString());
            weaponStats.SetText("+" + WeaponDataSO.weaponDataList[(int)currentWeaponType].attackSpeed.ToString() + " AtkSpeed" + " +" + WeaponDataSO.weaponDataList[(int)currentWeaponType].autoAttackRange.ToString() + "AtkRange");
        }
        
    }

    private void SetWeaponsAvailability(/*int currentWeapIndex*/ WeaponType currentWeaponType)
    {
        if (data.BoughtWeapons.Contains((int)currentWeaponType))
        {
            weaponAvailability.SetText(Constant.OWNED);
            weaponPrice.SetText(Constant.EQUIP_SKIN);
        }
        else
        {
            weaponAvailability.SetText(Constant.LOCKED);
        }
    }

    public void BuyWeapon()
    {
        if (!data.BoughtWeapons.Contains((int)currentWeapShownIndex) && data.CurrentCoins >= WeaponDataSO.weaponDataList[(int)currentWeapShownIndex].price)
        {
            data.BoughtWeapons.Add((int)currentWeapShownIndex);
            data.CurrentCoins = data.CurrentCoins - WeaponDataSO.weaponDataList[(int)currentWeapShownIndex].price;
            currentCoinLeft.SetText(data.CurrentCoins.ToString());
            UIManager.Instance.coinText.SetText(data.CurrentCoins.ToString());
            SaveManager.Instance.SaveData(data);
            currentCoinLeft.SetText(data.CurrentCoins.ToString());
        }
        if (weaponPrice.text == Constant.EQUIP_SKIN)
        {
            data.EquippedWeapon = (int)currentWeapShownIndex;
            SaveManager.Instance.SaveData(data);
            LevelManager.Instance.player.ChangeWeapon();
            LevelManager.Instance.player.SetAttackRange();
            LevelManager.Instance.player.ScaleAttackRangeCircle();
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
        if ((int)currentWeapShownIndex == 2)
        {
            currentWeapShownIndex = (WeaponType)0;
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
            currentWeapShownIndex = (WeaponType)2;

            showWeapon(currentWeapShownIndex);
        }
        else
        {
            currentWeapShownIndex--;
            showWeapon(currentWeapShownIndex);
        }
    }




}
