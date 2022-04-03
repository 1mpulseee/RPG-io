using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class networkManager : MonoBehaviourPunCallbacks
{
    public TerrainGenerator terrainGenerator;
    private PhotonView PV;
    public FirstPersonController fps;
    public GameObject module;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        if (!PV.IsMine)
        {
            fps.enabled = false;
            module.SetActive(false);
        }
        else
        {
            fps.InstanceSet();
        }
    }
    //public override void OnPlayerLeftRoom(Player otherPlayer)
    //{
    //    var pv = terrainGenerator.test.GetComponent<PhotonView>();
    //    if (pv.Owner == PhotonNetwork.MasterClient)
    //    {
    //        pv.Controller = PhotonNetwork.MasterClient;
    //    }
    //    if (PV.IsMine)
    //    {
    //        Debug.LogWarning(PhotonNetwork.MasterClient + "mine");
    //        Debug.LogWarning("leave" + otherPlayer.NickName + "mine");
    //        Debug.LogWarning(otherPlayer.IsMasterClient + "mine");
    //    }
    //    Debug.LogWarning(PhotonNetwork.MasterClient);
    //    Debug.LogWarning("leave" + otherPlayer.NickName);
    //    Debug.LogWarning(otherPlayer.IsMasterClient);
    //} 
}
