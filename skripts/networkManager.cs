using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class networkManager : MonoBehaviourPunCallbacks
{
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
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.LogWarning("leave" + otherPlayer.NickName);
        Debug.LogWarning(otherPlayer.IsMasterClient);
    } 
}
