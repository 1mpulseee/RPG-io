using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class networkManager : MonoBehaviour
{
    private PhotonView PV;
    public FirstPersonController fps;
    public GameObject module;
    public GameObject selector;
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (!PV.IsMine)
        {
            fps.enabled = false;
            module.SetActive(false);
            selector.SetActive(false);
        }
    }
}
