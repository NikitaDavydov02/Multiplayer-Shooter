using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainManager : MonoBehaviour
{
    public static bool GameIsFinished { get; private set; } = false;
    public static UIManager UIManager;
    public static SpawnPlayer SpawnPlayer;
    //public static CoinManager CoinManager;

    // Start is called before the first frame update
    private void Awake()
    {
        UIManager = GetComponent<UIManager>();
        SpawnPlayer = GetComponent<SpawnPlayer>();
        //CoinManager = GetComponent<CoinManager>();
        MainManager.SpawnPlayer.SpawnNewPlayerEvent += UIManager.AddPlayer;
        MainManager.SpawnPlayer.PlayerKilledEvent += UIManager.PlayerKilled;
        MainManager.SpawnPlayer.GameIsFinishedEvent += UIManager.GameIsFinished;
    }
    void Start()
    {
        //MainManager.SpawnPlayer.SpawnNewPlayerEvent += UIManager.AddPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
