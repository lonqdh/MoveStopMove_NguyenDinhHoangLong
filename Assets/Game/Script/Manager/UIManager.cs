using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public GameObject mainMenuUI;
    public GameObject gameplayUI;
    public GameObject skinShopUI;
    public GameObject weaponShopUI;
    public GameObject finishGameUI;
    public GameObject mainCamera;
    public Button finishBtn;
    public Button returnToMenuBtn;
    public Button playBtn;
    public Button weaponBtn;
    public Button skinBtn;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI playersLeft;
    private int numOfPlayers = 50;
    //private int numOfPlayersLeft;



    private void Start()
    {
        //numOfPlayersLeft = LevelManager.Instance.totalPlayers;
        playBtn.onClick.AddListener(StartGame);
        finishBtn.onClick.AddListener(FinishGame);
        returnToMenuBtn.onClick.AddListener(BackToMenu);
        weaponBtn.onClick.AddListener(OpenWeaponShop);
        skinBtn.onClick.AddListener(OpenSkinShop);
        coinText.text = GameManager.Instance.UserData.CurrentCoins.ToString();
    }

    private void BackToMenu()
    {
        finishGameUI.SetActive(false);
        mainMenuUI.SetActive(true);
        LevelManager.Instance.LoadLevel();
        GameManager.Instance.ChangeState(GameState.MainMenu);
    }

    private void FinishGame()
    {
        
    }

    public void StartGame()
    {
        mainMenuUI.SetActive(false);
        gameplayUI.SetActive(true);
        LevelManager.Instance.OnStart();
        SetTotalPlayersText();
    }

    public void OpenSkinShop()
    {
        mainCamera.GetComponent<AudioListener>().enabled = false;
        mainMenuUI.SetActive(false);
        skinShopUI.SetActive(true);
        Debug.Log(SkinShopManager.Instance.currentSession);
    }

    public void OpenWeaponShop()
    {
        mainCamera.GetComponent<AudioListener>().enabled = false;
        mainMenuUI.SetActive(false);
        weaponShopUI.SetActive(true);
    }

    public void SetTotalPlayersText()
    {
        playersLeft.SetText(LevelManager.Instance.totalPlayers.ToString());
    }

    private void ChangeUI(CanvasGroup on, CanvasGroup off)
    {
        on.interactable = on; 
        off.interactable = off;
        on.gameObject.SetActive(true);
        on.gameObject.SetActive(false);
    }
}
