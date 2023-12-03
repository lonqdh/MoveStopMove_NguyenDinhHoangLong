using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    MainMenu = 0,
    Gameplay = 1,
    Finish = 2,
}

public class GameManager : Singleton<GameManager>
{
    private GameState state;
    public UserData UserData;

    private void Awake()
    {
        //setup tong quan game
        //setup data
        ChangeState(GameState.MainMenu);

        if(SaveManager.Instance.HasData<UserData>())
        {
            Debug.Log("Load Data");
            UserData = SaveManager.Instance.LoadData<UserData>();
        }
        else
        {
            Debug.Log("Created Data");
            UserData = new UserData();
            SaveManager.Instance.SaveData(UserData);
        }
    }

    public void ChangeState(GameState gameState)
    {
        state = gameState;
    }

    public bool IsState(GameState gameState)
    {
        return state == gameState;
    }



}
