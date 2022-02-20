using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class resource : MonoBehaviour
{
    private float colliderPos;
    [HideInInspector]
    public bool ready = true;
    public TreeInfo treeInfo;
    public float resourceRecoveryTime = 5f;
    public ParticleSystem effect;
    public AudioSource sound;
    [System.Serializable]
    public class TreeInfo
    {
        [System.Serializable] public enum DropDown { tree, stone}

        [SerializeField] public DropDown type;
        public int ToolLvl;
        [HideInInspector]
        public int stage = 0;
        public GameObject[] stages;
    }
    private PhotonView PV;
    private void Start()
    {
        colliderPos = GetComponent<BoxCollider>().center.y;
        PV = GetComponent<PhotonView>();
        if (treeInfo.stages.Length > 1)
        {
            for (int i = 1; i < treeInfo.stages.Length; i++)
            {
                treeInfo.stages[i].SetActive(false);
            }
        }
    }
    public void felling(int lvl)
    {
        if (lvl >= treeInfo.ToolLvl)
        {
            treeInfo.stage++;
            if (treeInfo.stage > treeInfo.stages.Length - 1)
            {
                treeInfo.stage = treeInfo.stages.Length - 1;
            }
            Invoke("resourceRecovery", resourceRecoveryTime);
            PV.RPC("changeResource", RpcTarget.All, treeInfo.stage);
            PV.RPC("effects", RpcTarget.All);
        }
        else
        {
            Debug.Log("нужен более крутой инструмент");
        }
    }
    public void FixAttack()
    {
        Invoke("FixAttack2", 1f);
    }
    public void FixAttack2()
    {
        ready = true;
    }
    public void resourceRecovery()
    {
        //treeInfo.stage--;
        //if (treeInfo.stage < 0)
        //{
            treeInfo.stage = 0;
        //}
        PV.RPC("changeResource", RpcTarget.All, treeInfo.stage);
        CancelInvoke();
    }
    [PunRPC]
    public void changeResource(int stage)
    {
        for (int i = 0; i < treeInfo.stages.Length; i++)
        {
            treeInfo.stages[i].SetActive(false);
        }
        treeInfo.stages[stage].SetActive(true);
        if (stage == treeInfo.stages.Length - 1)
        {
            GetComponent<BoxCollider>().center = new Vector3(0, 100, 0);
        }
        else
        {
            GetComponent<BoxCollider>().center = new Vector3(0, colliderPos, 0);
        }
    }
    [PunRPC]
    public void effects()
    {
        effect.Play();
        sound.Play();
    }
}
