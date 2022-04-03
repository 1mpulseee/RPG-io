using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Lobby : MonoBehaviourPunCallbacks
{
    [System.Serializable] public enum DropDown { MainScene, test }
    [SerializeField] public DropDown scene;
    [SerializeField] List<GameObject> Buttons = new List<GameObject>();
    private List<RoomInfo> roomList = new List<RoomInfo>();
    public InputField NameInput;
    public Text nameText;
    public GameObject TextChangeMenu;
    private void Awake()
    {
        for (int i = 0; i < Buttons.Count; i++) { Buttons[i].SetActive(false); }
    }
    void Start()
    {
        TextChangeMenu.SetActive(false);
        if (PlayerPrefs.HasKey("Name"))
        {
            PhotonNetwork.NickName = PlayerPrefs.GetString("Name");
            nameText.text = PhotonNetwork.NickName;
        }
        else
        {
            PhotonNetwork.NickName = "Player" + Random.Range(1000, 9999); //�������
            PlayerPrefs.SetString("Name", PhotonNetwork.NickName);
            nameText.text = PhotonNetwork.NickName;
        }
        PhotonNetwork.AutomaticallySyncScene = true; //���������������� �����
        PhotonNetwork.GameVersion = "1"; //������ ����
        PhotonNetwork.ConnectUsingSettings();
    }
    public void StartGame()
    {
        if (CheckRooms())
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            string RoomName = "room" + Random.Range(0, 10000);
            PhotonNetwork.CreateRoom(RoomName, new Photon.Realtime.RoomOptions { IsVisible = true, MaxPlayers = 12 , CleanupCacheOnLeave = false}, Photon.Realtime.TypedLobby.Default);
        }
    }
    public void OpenChangeName()
    {
        TextChangeMenu.SetActive(true);
    }
    public void ChangeName()
    {
        if (NameInput.text != string.Empty)
        {
            PhotonNetwork.NickName = NameInput.text;
        }
        PlayerPrefs.SetString("Name", PhotonNetwork.NickName);
        nameText.text = PhotonNetwork.NickName;
        NameInput.text = "";
        TextChangeMenu.SetActive(false);
    }
    public bool CheckRooms()
    {
        if (roomList.Count > 0)
        {
            for (int i = 0; i < roomList.Count; i++)
            {
                if (roomList[i].PlayerCount < roomList[i].MaxPlayers)
                {
                    return true;
                }
            }
            return false;
        }
        else
        {
            return false;
        }
    }
    #region Callbacks
    public override void OnConnectedToMaster()
    {
        Debug.Log("Server loaded");
        for (int i = 0; i < Buttons.Count; i++) { Buttons[i].SetActive(true); }
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(scene.ToString());
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomListUpd)
    {
        roomList = roomListUpd;
    }
    #endregion
}
