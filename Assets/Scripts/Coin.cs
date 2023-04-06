using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Coin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.CoinIsCollected();
            OnCoinIsCollected(player);
        }
    }
    public event EventHandler CoinIsCollected;
    private void OnCoinIsCollected(Player player)
    {
        EventHandler handler = CoinIsCollected;
        if (handler != null)
            handler(this, new CoinIsCollectedEventArgs(this, player));
    }
}
public class CoinIsCollectedEventArgs : EventArgs
{
    public Coin Coin { get; private set; }
    public Player Player{ get; private set; }
    public CoinIsCollectedEventArgs(Coin coin, Player player)
    {
        Coin = coin;
        Player = player;
    }
}
