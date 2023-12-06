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
    [SerializeField] private Transform ground;

    public int totalPlayers = 50;
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

    private void LoadLevel()
    {
        //player = Instantiate(playerPrefab);
        player = playerPrefab;
        player.OnInit();
        player.transform.position = playerSpawnPoint.position;

        SpawnBotsAtStart();

    }

    public void OnStart()
    {
        GameManager.Instance.ChangeState(GameState.Gameplay);
        //LoadLevel();
    }

    public void SpawnBotsAtStart()
    {
        //foreach (Transform spawnPoint in botSpawnPointList)
        //{
        //    Bot newBot = LeanPool.Spawn(botPrefab, spawnPoint.position, spawnPoint.rotation);
        //    //newBot.gameObject.layer = 7;
        //    newBot.OnInit();
        //    bots.Add(newBot);

        //}


        for(int i = 0; i < 25; i++)
        {
            randomBotSpawnPos = new Vector3(UnityEngine.Random.Range(-200, 200), 1, UnityEngine.Random.Range(-200, 200));
            Bot newBot = LeanPool.Spawn(botPrefab, randomBotSpawnPos, Quaternion.identity);
            newBot.OnInit();
            bots.Add(newBot);
        }
    }

    //public void SpawnSingleBot(Transform spawnPoint)
    //{
    //    Bot newBot = LeanPool.Spawn(botPrefab, spawnPoint.position, spawnPoint.rotation);
    //    newBot.OnInit();
    //    bots.Add(newBot);
    //}

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
        }
        else
        {
            Debug.LogError("Attempting to despawn a null bot.");
        }
    }

    //lam cai spawn bot kphai dua tren diem co san nua, tu random tren plane

}
