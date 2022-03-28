using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Lobby : MonoBehaviourPunCallbacks
{
    [System.Serializable] public enum DropDown { MainScene, test }
    [SerializeField] List<GameObject> Buttons = new List<GameObject>();

    [SerializeField] public DropDown scene;
    private void Awake()
    {
        for (int i = 0; i < Buttons.Count; i++) { Buttons[i].SetActive(false); }
    }
    void Start()
    {
        
        //Debug.Log(PhotonNetwork.MAX_VIEW_IDS);
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
        for (int i = 0; i < Buttons.Count; i++) {Buttons[i].SetActive(true);}
        

        
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(scene.ToString());
    }
}
