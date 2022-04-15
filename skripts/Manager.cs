using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public float MinSpawnHight = 117;
    public int maxPlayers = 12;
    public GameObject Bot;
    public Transform Players;    
    public List<GameObject> players;
    private GameObject[] player;
    //singleton
    public static Manager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        InvokeRepeating("AddPlayer", 5, 5);
    }
    public void AddPlayer()
    {
        players = new List<GameObject>();
        //for (int i = 0; i < players.Count; i++)
        //{
        //    players.RemoveAt(i);
        //}
        player = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < player.Length; i++)
        {
            players.Add(player[i]);
        }
        player = GameObject.FindGameObjectsWithTag("Bot");
        for (int i = 0; i < player.Length; i++)
        {
            players.Add(player[i]);
        }
        int[,] biome = TerrainGenerator.Instance.Biomes;
        int size = 512;
        for (int i = players.Count; i < maxPlayers; i++)
        {
            while (true)
            {
                int x = Random.Range(0, size);
                int z = Random.Range(0, size);
                if (biome[x, z] == 0)
                {
                    TerrainData terrainData = TerrainGenerator.Instance.TerrainMain.terrainData;
                    float y = terrainData.GetHeight(z, x);
                    if (y > MinSpawnHight)
                    {
                        GameObject Newplayer = PhotonNetwork.Instantiate(Bot.name, new Vector3(z * 1000 / 512, y + 1, x * 1000 / 512), transform.rotation);
                        Newplayer.transform.SetParent(Players);
                        return;
                    }
                }
            }
        }
        
    }
}
