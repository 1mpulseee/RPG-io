using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Lobby : MonoBehaviourPunCallbacks
{
    [System.Serializable] public enum DropDown { MainScene, test }

    [SerializeField] public DropDown scene;
    void Start()
    {
        PhotonNetwork.NickName = "Player" + Random.Range(1000, 9999); //�������
        //Log("Player name: " + PhotonNetwork.NickName);
        //Log("Loading server...");
        //Log("Please wait");
        PhotonNetwork.AutomaticallySyncScene = true; //���������������� �����
        PhotonNetwork.GameVersion = "1"; //������ ����
        PhotonNetwork.ConnectUsingSettings();
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    public void CreateRoom()
    {
        string RoomName = "room" + Random.Range(0, 10000);
        PhotonNetwork.CreateRoom(RoomName, new Photon.Realtime.RoomOptions { IsVisible = true, MaxPlayers = 12 }, Photon.Realtime.TypedLobby.Default);
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Server loaded");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(scene.ToString());
    }
}
