using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField]
    GameObject PlayerPrefab;
    public List<Player> players;
    [SerializeField]
    private float minX;
    [SerializeField]
    private float minY;
    [SerializeField]
    private float maxX;
    [SerializeField]
    private float maxY;
    PhotonView photonView;
    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        players = new List<Player>();
        SpawnNewPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        List<Player> playersToRemove = new List<Player>();
        foreach(Player player in players)
            if (!player.Alive)
            {
                Debug.Log("Player is killed" + player);
                OnPlayerKilledEvent(player);
                playersToRemove.Add(player);
            }
        foreach(Player player in playersToRemove)
        {
            players.Remove(player);
            PhotonNetwork.Destroy(player.gameObject);
        }
        if (players.Count > 1 && MainManager.GameStatus!=GameStatus.Playing)
        {
            OnGameIsStartedEvent();
        }
        if (players.Count == 1 && MainManager.GameStatus==GameStatus.Playing)
        {
            //GameIsFinished;
            OnGameIsFinishedEvent(players[0]);
        }
    }
    private void SpawnNewPlayer()
    {
        Vector3 randomPosition = new Vector3(UnityEngine.Random.RandomRange(minX, maxX), UnityEngine.Random.RandomRange(minY, maxY), 0);

        GameObject player = PhotonNetwork.Instantiate(PlayerPrefab.name, randomPosition, Quaternion.identity,0,null);
        player.GetComponent<SpriteRenderer>().color = Color.green;
        Debug.Log("Instantiate you in client" + player.name);
        Player playerScript = player.GetComponent<Player>();
        players.Add(playerScript);
        player.name = "Player" + players.Count;
        OnSpawnNewPlayerEvent(playerScript);
        foreach(Player p in players)
        {
            if(p!= playerScript)
            {
                p.gameObject.GetComponent<PhotonView>().RPC("RPCSpawnNewPlayer", RpcTarget.Others, player);
            }
                
        }
    }
    [PunRPC]
    private void RPCSpawnNewPlayer(Player player)
    {
        Debug.Log("Another player has joined the room" + player.name);
        players.Add(player);
        player.name = "Player" + players.Count;
        //OnSpawnNewPlayerEvent(player);
    }
    public event EventHandler SpawnNewPlayerEvent;
    public event EventHandler PlayerKilledEvent;
    public event EventHandler GameIsFinishedEvent;
    public event EventHandler GameIsStartedEvent;

    public void OnSpawnNewPlayerEvent(Player player)
    {
        EventHandler handler = SpawnNewPlayerEvent;
        if (handler != null)
            handler(this, new SpawnNewPlayerEventArgs(player));
    }
    public void OnPlayerKilledEvent(Player player)
    {
        EventHandler handler = PlayerKilledEvent;
        if (handler != null)
            handler(this, new SpawnNewPlayerEventArgs(player));
    }
    public void OnGameIsFinishedEvent(Player player)
    {
        EventHandler handler = GameIsFinishedEvent;
        if (handler != null)
            handler(this, new SpawnNewPlayerEventArgs(player));
    }
    public void OnGameIsStartedEvent()
    {
        EventHandler handler = GameIsStartedEvent;
        if (handler != null)
            handler(this, new EventArgs());
    }
}
public class SpawnNewPlayerEventArgs : EventArgs
{
    public Player player;
    public SpawnNewPlayerEventArgs(Player player) : base()
    {
        this.player = player;
    }
}
