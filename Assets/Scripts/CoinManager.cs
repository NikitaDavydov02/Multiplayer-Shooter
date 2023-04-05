using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        while (currentCoinCount < maxCoinCount)
        {
            SpawnCoin();
        }
    }
    private void SpawnCoin()
    {
        GameObject newCoin = Instantiate(coinPrefab) as GameObject;
        Vector3 pos = new Vector3(UnityEngine.Random.RandomRange(minX, maxX), UnityEngine.Random.RandomRange(minY, maxY), 0);
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
