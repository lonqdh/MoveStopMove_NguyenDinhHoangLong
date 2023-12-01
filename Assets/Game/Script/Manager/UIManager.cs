using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject gameplayUI;
    public GameObject weaponShopUI;
    public Button playBtn;
    public Button weaponBtn;
    public Button skinBtn;



    private void Start()
    {
        playBtn.onClick.AddListener(StartGame);
    }

    public void StartGame()
    {
        mainMenuUI.SetActive(false);
        gameplayUI.SetActive(true);
        LevelManager.Instance.OnStart();
    }


    private void ChangeUI(CanvasGroup on, CanvasGroup off)
    {
        on.interactable = on; 
        off.interactable = off;
        on.gameObject.SetActive(true);
        on.gameObject.SetActive(false);

    }




}
