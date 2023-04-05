using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private InputField joinRoomName;
    [SerializeField]
    private InputField createRoomName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void JoinRoom()
    {
        if (String.IsNullOrEmpty(joinRoomName.text))
            return;
        PhotonNetwork.JoinRoom(joinRoomName.text);
    }
    public void CreateRoom()
    {
        if (String.IsNullOrEmpty(createRoomName.text))
            return;
        PhotonNetwork.CreateRoom(createRoomName.text);
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Join room");
        PhotonNetwork.LoadLevel("Room");
    }
}
