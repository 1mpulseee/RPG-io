using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class networkManager : MonoBehaviour
{
    private PhotonView PV;
    public FirstPersonController fps;
    public GameObject module;
    public GameObject Stats;
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
            Stats.SetActive(false);
            fps.InstanceSet();
        }
    }
    private void FixedUpdate()
    {
        if (!PV.IsMine && FirstPersonController.Instance != null)
        {
            Stats.transform.LookAt(FirstPersonController.Instance.cam);
            Stats.transform.localRotation = new Quaternion(Stats.transform.localRotation.x, 0, Stats.transform.localRotation.z, 0);
        }
    }
}
