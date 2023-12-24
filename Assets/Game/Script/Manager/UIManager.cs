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
    public TextMeshProUGUI finishLabel;
    public TextMeshProUGUI finishBtnText;
    public TextMeshProUGUI playersLeft;
    public TextMeshProUGUI killerName;
    public TextMeshProUGUI rankLabel;
    public GameObject lostPanel;
    public GameObject wonPanel;



    private int numOfPlayers = 50;
    private bool finishGame;
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

    public void OpenFinishUI()
    {
        //SetTotalPlayersText();
        SetFinishGameState();
        gameplayUI.SetActive(false);
        finishGameUI.SetActive(true);
    }

    public void SetFinishGameState()
    {
        if(LevelManager.Instance.finishedLevel != true)
        {
            finishLabel.SetText(Constant.GAMEOVER);
            finishBtnText.SetText(Constant.RETRY);
            rankLabel.SetText("#" + LevelManager.Instance.totalPlayers.ToString());
            killerName.SetText(LevelManager.Instance.player.killer.name);
            wonPanel.SetActive(false);
            lostPanel.SetActive(true);
            //finishGame = false;
        }
        else
        {
            finishLabel.SetText(Constant.WIN);
            finishBtnText.SetText(Constant.NEXT_LEVEL);
            lostPanel.SetActive(false);
            wonPanel.SetActive(true);
            //finishGame = true;
        }
    }

    private void FinishGame()
    {
        if(LevelManager.Instance.finishedLevel == true)
        {
            LevelManager.Instance.levelCount++;
            LevelManager.Instance.finishedLevel = false;
            finishGameUI.SetActive(false);
            gameplayUI.SetActive(true);
            Debug.Log("You Finished And Moved To Next Level!");
            LevelManager.Instance.LevelStart();
            SetTotalPlayersText();
        }
        else
        {
            Debug.Log("You Chose To Retry");
            finishGameUI.SetActive(false);
            gameplayUI.SetActive(true);
            LevelManager.Instance.LevelStart();
            SetTotalPlayersText();
        }
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
