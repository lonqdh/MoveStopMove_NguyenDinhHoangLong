using Lean.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public FloatingJoystick joystick;
    [SerializeField] public Bot botPrefab;
    [SerializeField] private Player playerPrefab;
    [NonSerialized] public Player player;
    [NonSerialized] public List<Bot> bots = new List<Bot>();

    [SerializeField] public Transform planeTransform;
    [SerializeField] public List<Transform> botSpawnPointList;
    [SerializeField] private Transform playerSpawnPoint;




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
        player = Instantiate(playerPrefab);
        player.OnInit();
        player.transform.position = playerSpawnPoint.position;

        SpawnBots();

    }

    public void SpawnBots()
    {
        foreach (Transform spawnPoint in botSpawnPointList)
        {
            Bot newBot = LeanPool.Spawn(botPrefab, spawnPoint.position, spawnPoint.rotation);
            bots.Add(newBot);
        }
    }

    public void SpawnBot(Transform spawnPoint)
    {
        Bot newBot = LeanPool.Spawn(botPrefab, spawnPoint.position, spawnPoint.rotation);
        bots.Add(newBot);
    }

    public void DespawnBots(Bot bot)
    {
        LeanPool.Despawn(bot);
        bots.Remove(bot);
    }


}
