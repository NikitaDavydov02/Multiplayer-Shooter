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
    private int currentCoinCount = 0;
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
    }

    // Update is called once per frame
    void Update()
    {
        return;
        while (currentCoinCount < maxCoinCount)
        {
            SpawnCoin();
        }
    }
    private void SpawnCoin()
    {
        Vector3 pos = new Vector3(UnityEngine.Random.RandomRange(minX, maxX), UnityEngine.Random.RandomRange(minY, maxY), 0);
        GameObject newCoin = PhotonNetwork.Instantiate(coinPrefab.name, pos,Quaternion.identity) as GameObject;
        
        newCoin.transform.position = pos;
        currentCoinCount++;
        Coin coinScript = newCoin.gameObject.GetComponent<Coin>();
        coinScript.CoinIsCollected += CoinIsCollected;
    }
    public void CoinIsCollected(object sender, EventArgs args)
    {
        currentCoinCount--;
        OnCoinIsCollected(args);
    }
    public event EventHandler CoinIsCollectedEvent;
    private void OnCoinIsCollected(EventArgs args)
    {
        EventHandler handler = CoinIsCollectedEvent;
        if (handler != null)
            handler(this, args);
    }
}
