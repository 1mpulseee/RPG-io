using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject PlayerPrefab;
    public GameObject cam;
    public Image LoadImg;
    public Text text;
    public float MinSpawnHight;
    public static GameManager Instance { get; private set; } // static singleton

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }
    public void UpdateLoadingScreen(int procent)
    {
        text.text = "Loading " + procent.ToString() + "%";
    }
    public void SpawnPlayer()
    {
        cam.SetActive(false);
        int[,] biome = TerrainGenerator.Instance.Biomes;
        int size = 512;
        while(true)
        {
            int x = Random.Range(0, size);
            int z = Random.Range(0, size);
            if (biome[x,z] == 0)
            {
                TerrainData terrainData = TerrainGenerator.Instance.TerrainMain.terrainData;
                float y = terrainData.GetHeight(z, x);
                if (y > MinSpawnHight)
                {
                    PhotonNetwork.Instantiate(PlayerPrefab.name, new Vector3(z * 1000 / 512, y + 1, x * 1000 / 512), transform.rotation);
                    return;
                }
            }
        } 
    }
}
