using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
public class CoinManager : MonoBehaviour
{
    [SerializeField]
    private int maxCoinCount;
    [SerializeField]
    GameObject coinPrefab;
    public int currentCoinCount = 0;
    [SerializeField]
    private float minX;
    [SerializeField]
    private float minY;
    [SerializeField]
    private float maxX;
    [SerializeField]
    private float maxY;
    PhotonView photonView;
    //public List<Coin> coins;
    // Start is called before the first frame update
    void Start()
    {
        //coins = new List<Coin>();
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        //Debug.Log("Master client coins count: " + currentCoinCount);
        while (currentCoinCount < maxCoinCount)
        {
            SpawnCoin();
            currentCoinCount++;

        }
        //photonView.RPC("UpdateCoinsCount", RpcTarget.All, currentCoinCount);
    }
    private void SpawnCoin()
    {
        Vector3 pos = new Vector3(UnityEngine.Random.RandomRange(minX, maxX), UnityEngine.Random.RandomRange(minY, maxY), 0);
        GameObject newCoin = PhotonNetwork.Instantiate(coinPrefab.name, pos,Quaternion.identity) as GameObject;
        Coin coinScript = newCoin.gameObject.GetComponent<Coin>();
        coinScript.CoinIsCollected += CoinIsCollected;
        
        //coins.Add(coinScript);
        //photonView.RPC("AddNewCoin", RpcTarget.All, null);
    }
    //[PunRPC]
    //public void UpdateCoinsCount(int coinsCount)
    //{
    //    Debug.Log("Coins count RPC update received" + coinsCount);
    //    currentCoinCount = coinsCount;
    //}
    //[PunRPC]
    //public void AddNewCoin()
    //{
    //    Debug.Log("Receive RPC to add coin");
    //    GameObject[] existingCoins = GameObject.FindGameObjectsWithTag("Coin");
    //    foreach (GameObject c in existingCoins)
    //    {
    //        Coin coinScript = c.gameObject.GetComponent<Coin>();
    //        if (!coins.Contains(coinScript))
    //        {
    //            coins.Add(coinScript);
    //            coinScript.CoinIsCollected += CoinIsCollected;
    //        }
    //    }
    //    Debug.Log("Now i have coins" + coins.Count);
    //}
    public void CoinIsCollected(object sender, EventArgs args)
    {
        CoinIsCollectedEventArgs coinIsCollectedEventArgs = args as CoinIsCollectedEventArgs;
        if (coinIsCollectedEventArgs == null)
            return;
        Player playerCllected = coinIsCollectedEventArgs.Player;
        Coin coin = coinIsCollectedEventArgs.Coin;
        PhotonNetwork.Destroy(coin.gameObject);
        currentCoinCount--;
        //photonView.RPC("UpdateCoinsCount", RpcTarget.All, currentCoinCount);
        SpawnNewPlayerEventArgs arg = new SpawnNewPlayerEventArgs(playerCllected);
        OnCoinIsCollected(arg);
    }
    public event EventHandler CoinIsCollectedEvent;
    private void OnCoinIsCollected(EventArgs args)
    {
        EventHandler handler = CoinIsCollectedEvent;
        if (handler != null)
            handler(this, args);
    }
}
