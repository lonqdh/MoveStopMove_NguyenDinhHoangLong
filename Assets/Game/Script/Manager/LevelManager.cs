using Lean.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private Player playerPrefab;
    [SerializeField] private LevelDataSO levelDataSO;
    [SerializeField] public Bot botPrefab;
    [NonSerialized] public Level levelPrefab;
    [NonSerialized] public Player player;
    [NonSerialized] public List<Bot> bots = new List<Bot>();
    
    public int totalPlayers;
    public FloatingJoystick joystick;
    private Vector3 randomBotSpawnPos;
    private Level currentLevel;
    public int levelCount;
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
        
        currentLevel = Instantiate(levels[levelCount]);

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
        if (bot != null && bots.Contains(bot) && totalPlayers >= 1)
        {
            bots.Remove(bot);
            LeanPool.Despawn(bot);
            totalPlayers--;
            Debug.Log(totalPlayers);
            UIManager.Instance.SetTotalPlayersText();
        }
        else
        {
            Debug.Log("Finished Game!");
            finishedLevel = true;
            OnFinish();
        }
    }
}
