using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSyns : MonoBehaviour
{
    PhotonView PV;
    Animator animator;
    void Start()
    {
        PV = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
    }

    public void SetAnimTrigger(string animName)
    {
        PV.RPC("SetAnimTriggerRPC", RpcTarget.All, animName);
    }
    [PunRPC]
    public void SetAnimTriggerRPC(string animName)
    {
        animator.SetTrigger(animName);
    }
}
