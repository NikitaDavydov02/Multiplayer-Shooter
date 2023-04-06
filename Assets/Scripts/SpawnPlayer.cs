using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class SpawnPlayer : MonoBehaviourPunCallbacks
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
        Debug.Log(photonView.Owner.NickName+" : SpawnPlayer script started");
        SpawnNewPlayer();
        Debug.Log(players.Count + " : Players count");
    }

    // Update is called once per frame
    void Update()
    {
        List<Player> playersToRemove = new List<Player>();
        foreach(Player player in players)
            if (!player.Alive)
            {
                Debug.Log("Player is killed" + photonView.Owner.NickName);
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
        Player playerScript = player.GetComponent<Player>();
        players.Add(playerScript);
        photonView.Owner.NickName = "Player" + players.Count;
        Debug.Log("Instantiate you in client" + photonView.Owner.NickName);
        Debug.Log("Send RPC");
        OnSpawnNewPlayerEvent(playerScript);
        photonView.RPC("FindNewPlayer", RpcTarget.All, null);
        
        
    }
    [PunRPC]
    public void FindNewPlayer()
    {
        Debug.Log("Receive RPC to add new plyere");
        GameObject[] existingPlayeres = GameObject.FindGameObjectsWithTag("Player");
       foreach (GameObject p in existingPlayeres)
        {
            Player pS = p.GetComponent<Player>();
            if (!players.Contains(pS))
            {
                players.Add(pS);
                OnSpawnNewPlayerEvent(pS);
            }
        }
        Debug.Log("Now i have"+players.Count);
        photonView.RPC("FindNewPlayersOrExisting", RpcTarget.All, null);
    }
    [PunRPC]
    public void FindNewPlayersOrExisting()
    {
        GameObject[] existingPlayeres = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log("Receive RPC"+ existingPlayeres.Length);
        foreach (GameObject p in existingPlayeres)
        {
            Player pS = p.GetComponent<Player>();
            if (!players.Contains(pS))
            {
                players.Add(pS);
                OnSpawnNewPlayerEvent(pS);
            }
        }
        Debug.Log("Now i have" + players.Count);
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
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
