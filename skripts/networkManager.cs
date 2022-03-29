using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class networkManager : MonoBehaviour
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
}
