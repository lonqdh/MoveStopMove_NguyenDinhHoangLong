using Lean.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] public Bot botPrefab;
    [SerializeField] private Player playerPrefab;
    [NonSerialized] public Player player;
    [NonSerialized] public List<Bot> bots = new List<Bot>();

    [SerializeField] public Transform planeTransform;
    //[SerializeField] public List<Transform> botSpawnPointList;
    [SerializeField] private Transform playerSpawnPoint;

    public int totalPlayers;
    public FloatingJoystick joystick;
    private Vector3 randomBotSpawnPos;
    


    // Start is called before the first frame update
    void Start()
    {
        LoadLevel();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadLevel()
    {
        //player = Instantiate(playerPrefab);
        totalPlayers = 50;
        player = playerPrefab;
        player.OnInit();
        player.transform.position = playerSpawnPoint.position;
        DespawnBots();
        SpawnBotsAtStart();
    }

    public void RetryLevel()
    {
        LoadLevel();
        OnStart();
    }

    public void OnStart()
    {
        GameManager.Instance.ChangeState(GameState.Gameplay);
    }

    public void OnFinish()
    {
        GameManager.Instance.ChangeState(GameState.Finish);
        UIManager.Instance.OpenFinishUI();
    }

    public void SpawnBotsAtStart()
    {
        for(int i = 0; i < 25; i++)
        {
            randomBotSpawnPos = new Vector3(UnityEngine.Random.Range(-200, 200), 1, UnityEngine.Random.Range(-200, 200));
            Bot newBot = LeanPool.Spawn(botPrefab, randomBotSpawnPos, Quaternion.identity);
            newBot.OnInit();
            bots.Add(newBot);
        }
    }

    public void DespawnBots()
    {
        LeanPool.DespawnAll();
    }

    public void SpawnSingleBot()
    {
        randomBotSpawnPos = new Vector3(UnityEngine.Random.Range(-200, 200), 1, UnityEngine.Random.Range(-200, 200));
        Bot newBot = LeanPool.Spawn(botPrefab, randomBotSpawnPos, Quaternion.identity);
        newBot.OnInit();
        bots.Add(newBot);
    }

    public void DespawnBot(Bot bot)
    {
        if (bot != null && bots.Contains(bot) && totalPlayers > 0)
        {
            bots.Remove(bot);
            LeanPool.Despawn(bot);
            totalPlayers--;
            UIManager.Instance.SetTotalPlayersText();
        }
        else if(totalPlayers == 0)
        {
            Debug.Log("Finished Game!");
            OnFinish();
        }
    }
}


//public class LevelManager : Singleton<LevelManager>
//{
//    public List<Level> levels = new List<Level>();
//    public Player player;
//    Level currentLevel;

//    int level = 1;

//    private void Start()
//    {
//        UIManager.Instance.OpenMainMenuUI();
//        LoadLevel();
//    }

//    public void LoadLevel()
//    {
//        LoadLevel(level);
//        OnInit();
//    }

//    public void LoadLevel(int indexLevel)
//    {
//        if (currentLevel != null)
//        {
//            Destroy(currentLevel.gameObject);
//            //foreach(var bricks in player.brickList)
//            //{
//            //    Destroy(bricks.gameObject);
//            //}
//        }

//        currentLevel = Instantiate(levels[indexLevel - 1]);
//    }

//    public void OnInit()
//    {
//        player.transform.position = currentLevel.startPoint.position;
//        player.OnInit();
//    }

//    public void OnStart()
//    {
//        GameManager.Instance.ChangeState(GameState.Gameplay);
//    }

//    public void OnFinish()
//    {
//        UIManager.Instance.OpenFinishUI();
//        GameManager.Instance.ChangeState(GameState.Finish);
//    }

//    public void NextLevel()
//    {
//        level++;
//        LoadLevel();
//    }


//}
