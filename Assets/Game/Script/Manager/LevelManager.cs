using Lean.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] public Bot botPrefab;
    [SerializeField] private Player playerPrefab;
    [NonSerialized] public Level levelPrefab;
    [NonSerialized] public Player player;
    [NonSerialized] public List<Bot> bots = new List<Bot>();

    //[SerializeField] public Transform planeTransform;
    //[SerializeField] public List<Transform> botSpawnPointList;
    //[SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private LevelDataSO levelDataSO;
    public int totalPlayers;
    public FloatingJoystick joystick;
    private Vector3 randomBotSpawnPos;
    private Level currentLevel;
    private int levelCount;
    public bool finishedLevel = false;
    


    // Start is called before the first frame update
    void Start()
    {
        LoadLevel();
    }


    public void LoadLevel()
    {
        List<Level> levels = levelDataSO.listLevels;
        if(currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }
        if(finishedLevel == true)
        {
            currentLevel = Instantiate(levels[1]);
        }
        else
        {
            currentLevel = Instantiate(levels[0]);
        }

        //Debug.Log("Load level " + levelCount);
        //currentLevel = Instantiate(levels[levelCount]);


        //levelPrefab = levelDataSO.listLevels[level];
        //currentLevel = levelPrefab;
        //level = currentLevel.levelId;
        //Instantiate(levelPrefab);
        //LeanPool.Spawn(currentLevel.levelEnvironment);
        //currentLevel = Resources.Load(levelDataSO.listLevels[level]);

        //player = Instantiate(playerPrefab);
        //if (currentLevel != null)
        //{
        //    Destroy(currentLevel.gameObject);
        //    //foreach(var bricks in player.brickList)
        //    //{
        //    //    Destroy(bricks.gameObject);
        //    //}
        //}
        //currentLevel = Instantiate(levels[indexLevel - 1]);



        totalPlayers = 50;
        player = playerPrefab;
        player.OnInit();
        player.transform.position = currentLevel.playerSpawnPoint.position;
        DespawnBots();
        SpawnBotsAtStart();
    }

    public void LevelStart()
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
        for(int i = 0; i < totalPlayers/2; i++)
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
        else if (totalPlayers == 0)
        {
            Debug.Log("Finished Game!");
            finishedLevel = true;
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
