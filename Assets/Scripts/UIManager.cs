using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Photon.Pun;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Slider HPSliderPrefab;
    [SerializeField]
    Canvas canvas;
    [SerializeField]
    GameObject FinalPopup;
    [SerializeField]
    Text winnerNameLable;
    [SerializeField]
    Text winnerCollectedCoinsLabel;
    private Dictionary<Slider, Player> playerSliders = new Dictionary<Slider, Player>();
    [SerializeField]
    private Slider coinSlider;
    [SerializeField]
    private Text collectedCoinsCountText;
    
    // Start is called before the first frame update
    private void Awake()
    {
        
    }
    void Start()
    {
        FinalPopup.SetActive(false);
       // MainManager.SpawnPlayer.SpawnNewPlayerEvent += AddPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Slider slider in playerSliders.Keys)
        {
            Vector3 sliderPosition = Camera.main.WorldToScreenPoint(playerSliders[slider].gameObject.transform.position + new Vector3(0, 0.7f, 0));
            sliderPosition.z = 0;
            slider.transform.position = sliderPosition;
            //slider.transform.position = Camera.main.WorldToScreenPoint(playerSliders[slider].gameObject.transform.position + new Vector3(0, 0.7f, 0));

            slider.value = playerSliders[slider].HP;
            //playerHPSlider.value = MainManager.EnemyAndPlayerManager.Player.gameObject.GetComponent<PlayerMovment>().HP;
        }
    }
    public void AddPlayer(object sender, EventArgs args)
    {
        Debug.Log("UI add player");
        Player player = ConvertEventArgsToPLayer(args);
        if (player == null)
            return;

        //Slider slider = PhotonNetwork.Instantiate(CanvasScaler.Instantiate(HPSliderPrefab),Vector3.zero,Quaternion.identity) as Slider;
        GameObject sliderObject = PhotonNetwork.Instantiate(HPSliderPrefab.name, Vector3.zero, Quaternion.identity) as GameObject;
        Slider slider = sliderObject.GetComponent<Slider>();
        slider.GetComponent<RectTransform>().SetParent(canvas.transform);
        //Vector3 sliderPosition = Camera.main.WorldToScreenPoint(player.gameObject.transform.position);
        //slider.transform.position = sliderPosition;
        playerSliders.Add(slider, player);
        slider.maxValue = player.maxHP;
        slider.value = player.HP;
        slider.GetComponent<RectTransform>().localScale = HPSliderPrefab.GetComponent<RectTransform>().localScale;
    }
    public void PlayerKilled(object sender, EventArgs args)
    {
        Player player = ConvertEventArgsToPLayer(args);
        if (player == null)
            return;
        // Debug.Log("Killed player" + player.GetHashCode());
        Slider killedPlayerSlider = null;
        foreach (Slider slider in playerSliders.Keys)
        {
            //Debug.Log("Slider collectionplayer" + playerSliders[slider].GetHashCode());
            if (playerSliders[slider] == player)
                killedPlayerSlider = slider;
        }
        if (killedPlayerSlider != null)
            playerSliders.Remove(killedPlayerSlider);
        Destroy(killedPlayerSlider.gameObject);
    }
    public void GameIsFinished(object sender, EventArgs args)
    {
        Player player = ConvertEventArgsToPLayer(args);
        if (player == null)
            return;
        FinalPopup.SetActive(true);
        winnerNameLable.text = "Winner: " + player.gameObject.name;
        winnerCollectedCoinsLabel.text = "He collected " + player.CoinsCollected + " coins";
    }
    public void CoinIsCollected(object sender, EventArgs args)
    {
        Player player = ConvertEventArgsToPLayer(args);
        if (player == null)
            return;
        collectedCoinsCountText.text = player.CoinsCollected.ToString();
        coinSlider.value = player.CoinsCollected % 10;
    }
    private Player ConvertEventArgsToPLayer(EventArgs args)
    {
        SpawnNewPlayerEventArgs spawnNewPlayerEventArgs = args as SpawnNewPlayerEventArgs;
        if (spawnNewPlayerEventArgs == null)
            return null;
        Player player = spawnNewPlayerEventArgs.player;
        return player;
    }
}
