using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class MainManager : MonoBehaviourPunCallbacks
{
    public static GameStatus GameStatus { get; private set; }
    private static UIManager UIManager;
    private static SpawnPlayer SpawnPlayer;
    private static CoinManager CoinManager;
    private static PhotonView photonView;
    //public static CoinManager CoinManager;

    // Start is called before the first frame update
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        UIManager = GetComponent<UIManager>();
        SpawnPlayer = GetComponent<SpawnPlayer>();
        CoinManager = GetComponent<CoinManager>();
        //CoinManager = GetComponent<CoinManager>();
        MainManager.SpawnPlayer.SpawnNewPlayerEvent += UIManager.AddPlayer;
        MainManager.SpawnPlayer.PlayerKilledEvent += UIManager.PlayerKilled;
        MainManager.SpawnPlayer.GameIsFinishedEvent += UIManager.GameIsFinished;
        MainManager.SpawnPlayer.GameIsFinishedEvent += GameIsFinished;
        MainManager.SpawnPlayer.GameIsStartedEvent += GameIsStarted;
        MainManager.SpawnPlayer.CoinIsCollectedEvent += UIManager.CoinIsCollected;
        GameStatus = GameStatus.WaitingForPlayers;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void GameIsFinished(object sender, EventArgs args)
    {
        GameStatus = GameStatus.Finished;
        Debug.Log("GameStatus.Finished;");
    }
    private void GameIsStarted(object sender, EventArgs args)
    {
        GameStatus = GameStatus.Playing;
        Debug.Log("GameStatus.Playing;");
    }
    
}
public enum GameStatus { 
    WaitingForPlayers,
    Playing,
    Finished,

}

