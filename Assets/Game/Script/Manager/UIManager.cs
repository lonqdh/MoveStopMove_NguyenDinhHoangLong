using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public GameObject mainMenuUI;
    public GameObject gameplayUI;
    public GameObject weaponShopUI;
    public GameObject mainCamera;
    public Button playBtn;
    public Button weaponBtn;
    public Button skinBtn;
    public TextMeshProUGUI coinText;



    private void Start()
    {
        playBtn.onClick.AddListener(StartGame);
        weaponBtn.onClick.AddListener(OpenWeaponShop);
        coinText.text = GameManager.Instance.UserData.CurrentCoins.ToString();
    }

    public void StartGame()
    {
        mainMenuUI.SetActive(false);
        gameplayUI.SetActive(true);
        LevelManager.Instance.OnStart();
    }

    public void OpenWeaponShop()
    {
        mainCamera.GetComponent<AudioListener>().enabled = false;
        mainMenuUI.SetActive(false);
        weaponShopUI.SetActive(true);

    }


    private void ChangeUI(CanvasGroup on, CanvasGroup off)
    {
        on.interactable = on; 
        off.interactable = off;
        on.gameObject.SetActive(true);
        on.gameObject.SetActive(false);

    }




}
