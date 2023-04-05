using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField]
    GameObject PlayerPrefab;
    private List<Player> players;
    [SerializeField]
    private float minX;
    [SerializeField]
    private float minY;
    [SerializeField]
    private float maxX;
    [SerializeField]
    private float maxY;
    // Start is called before the first frame update
    void Start()
    {
        players = new List<Player>();
        SpawnNewPlayer();
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
            Destroy(player.gameObject);
        }
        if (players.Count == 1)
        {
            //GameIsFinished;
            OnGameIsFinishedEvent(players[0]);
        }
    }
    private void SpawnNewPlayer()
    {
        GameObject player = Instantiate(PlayerPrefab) as GameObject;
        player.transform.position = new Vector3(UnityEngine.Random.RandomRange(minX, maxX), UnityEngine.Random.RandomRange(minY, maxY), 0);
        
        Player playerScript = player.GetComponent<Player>();
        players.Add(playerScript);
        player.name = "Player" + players.Count;
        OnSpawnNewPlayerEvent(playerScript);
    }
    public event EventHandler SpawnNewPlayerEvent;
    public event EventHandler PlayerKilledEvent;
    public event EventHandler GameIsFinishedEvent;

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
}
public class SpawnNewPlayerEventArgs : EventArgs
{
    public Player player;
    public SpawnNewPlayerEventArgs(Player player) : base()
    {
        this.player = player;
    }
}
