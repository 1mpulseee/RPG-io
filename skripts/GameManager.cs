using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject PlayerPrefab;
    void Start()
    {
        PhotonNetwork.Instantiate(PlayerPrefab.name, transform.position, transform.rotation);
    }
}
