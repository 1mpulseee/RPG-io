using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class TerrainGenerator : MonoBehaviour
{
    public GameObject test;

    private PhotonView PV;
    public Terrain TerrainMain;
    public int size = 513;
    public float[,] heights;
    public int[,] Biomes = new int[1000, 1000] ;
    
    public biomeGen[] BiomeGen;
    [System.Serializable]
    public class biomeGen
    {
        public float zoom;
        public float pow;
        public SplatHeights[] splatHeights;
        [System.Serializable]
        public class SplatHeights
        {
            public int textureIndex;
            public int startingHeight;
            public Biome1[] resourses;
            [System.Serializable]
            public class Biome1
            {
                public int chance;
                public GameObject obj;
            }
        }
    }
    public float zoomH;
    public float zoomH2;
    public float powH;
    public float powH2;
    public float Hsize;
    public float Hsize2;
    public int ColorGen;
    public List<int> ColorGenMix;
    private int HeightGen = 0;

    public NavMeshSurface[] surfaces;
    public static TerrainGenerator Instance { get; private set; } // static singleton

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PhotonNetwork.IsMasterClient)
        {
            if (HeightGen < 1)
            {
                HeightGen = Random.Range(1, 1000);
                ColorGen = Random.Range(1, 1000);
                Invoke("CreateWorld", 0.1f);
            }
        }
        else
        {
            if (HeightGen < 1)
            {
                PV.RPC("GetInfo", RpcTarget.MasterClient);
                Invoke("Start", 0);
            }
            else
            {
                Invoke("CreateWorld", 0.1f);
            }
        }  
    }
    public void CreateWorld()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            CreateTexure();
            //SetTerrainHeights();
            //PaintTerrain();
            //CreateTrees();
            //generateNavMesh();
        }
        else
        {
            if (HeightGen != 0)
            {
                CreateTexure();
                //SetTerrainHeights();
                //PaintTerrain();
                //generateNavMesh();
            }
            else
            {
                PV.RPC("GetInfo", RpcTarget.MasterClient);
                Invoke("CreateWorld", 0);
            }
        }
    }
    public void SetTerrainHeights()
    {  
        heights = TerrainMain.terrainData.GetHeights(0, 0, size, size);
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                float H = (Mathf.Pow(Mathf.PerlinNoise((x + HeightGen * 100) / zoomH, (y) / zoomH), powH) * Hsize)
                         + Mathf.Pow(Mathf.PerlinNoise((x) / zoomH2, (y + HeightGen * 100) / zoomH2), powH2) * Hsize2;
                heights[x, y] = H;
            }
        }
        TerrainMain.terrainData.SetHeights(0, 0, heights);
        GameManager.Instance.UpdateLoadingScreen(50);
        Invoke("PaintTerrain", 0.1f);
    }
    public void PaintTerrain()
    {
        TerrainData terrainData = TerrainMain.terrainData;
        float[,,] splatmapData = new float[terrainData.alphamapWidth,
                                           terrainData.alphamapHeight,
                                           terrainData.alphamapLayers];
        for (int y = 0; y < terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < terrainData.alphamapWidth; x++)
            {
                float terrainHeight = terrainData.GetHeight(y, x);

                int splats = 0;
                for (int i = 0; i < BiomeGen.Length; i++)
                {
                    splats += BiomeGen[i].splatHeights.Length;
                }
                float[] splat = new float[splats];

                int index = Biomes[x, y];
                for (int i = 0; i < BiomeGen[index].splatHeights.Length; i++)
                {
                    if (i == BiomeGen[index].splatHeights.Length - 1 && terrainHeight >= BiomeGen[index].splatHeights[i].startingHeight)
                    {
                        splat[BiomeGen[index].splatHeights[i].textureIndex] = 1;
                    }
                    else if (terrainHeight >= BiomeGen[index].splatHeights[i].startingHeight && terrainHeight <= BiomeGen[index].splatHeights[i + 1].startingHeight)
                    {
                        splat[BiomeGen[index].splatHeights[i].textureIndex] = 1;
                    }
                }
                for (int j = 0; j < BiomeGen[index].splatHeights.Length; j++)
                {
                    splatmapData[x, y, BiomeGen[index].splatHeights[j].textureIndex] = splat[BiomeGen[index].splatHeights[j].textureIndex];
                }
            }
        }
        terrainData.SetAlphamaps(0, 0, splatmapData);
        GameManager.Instance.UpdateLoadingScreen(75);
        if (PhotonNetwork.IsMasterClient)
        {
            CreateResourses();
        }
        Invoke("generateNavMesh", 0.1f);
    }
    public void CreateTexure()
    {
        GameManager.Instance.UpdateLoadingScreen(0);
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                int index = 0;
                float indexInfo = 0;
                for (int i = 0; i < BiomeGen.Length; i++)
                {
                    if (Mathf.Pow(Mathf.PerlinNoise((x + (ColorGenMix[i] + ColorGen) * 100) / BiomeGen[i].zoom, y / BiomeGen[i].zoom), BiomeGen[i].pow) > indexInfo)
                    {
                        indexInfo = Mathf.Pow(Mathf.PerlinNoise((x + (ColorGenMix[i] + ColorGen) * 100) / BiomeGen[i].zoom, y / BiomeGen[i].zoom), BiomeGen[i].pow);
                        index = i;
                    }
                }
                Biomes[x, y] = index;
            }
        }
        GameManager.Instance.UpdateLoadingScreen(25);
        Invoke("SetTerrainHeights", 0.1f);
    }
    public void CreateResourses()
    {
        TerrainData terrainData = TerrainMain.terrainData;
        for (int y = 0; y < terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < terrainData.alphamapWidth; x++)
            {
                int index = Biomes[y, x];
                float terrainHeight = terrainData.GetHeight(x, y);

                for (int i = 0; i < BiomeGen[index].splatHeights.Length ; i++)
                {
                    if (BiomeGen[index].splatHeights[i].resourses.Length > 0)
                    {
                        if (i > 0 && terrainHeight >= BiomeGen[index].splatHeights[i].startingHeight)
                        {
                            if (BiomeGen[index].splatHeights.Length > i + 1)
                            {
                                if (terrainHeight <= BiomeGen[index].splatHeights[i + 1].startingHeight)
                                {
                                    for (int r = 0; r < BiomeGen[index].splatHeights[i].resourses.Length; r++)
                                    {
                                        if (Random.Range(0, BiomeGen[index].splatHeights[i].resourses[r].chance) == 0)
                                        {
                                            PhotonNetwork.Instantiate(BiomeGen[index].splatHeights[i].resourses[r].obj.name, new Vector3(x * 1000 / 512, terrainHeight, y * 1000 / 512), Quaternion.identity);
                                        }
                                    }
                                }
                            }
                            else
                            {

                                for (int r = 0; r < BiomeGen[index].splatHeights[i].resourses.Length; r++)
                                {
                                    if (Random.Range(0, BiomeGen[index].splatHeights[i].resourses[r].chance) == 0)
                                    {
                                        if (test == null)
                                        {
                                            test = PhotonNetwork.Instantiate(BiomeGen[index].splatHeights[i].resourses[r].obj.name, new Vector3(x * 1000 / 512, terrainHeight, y * 1000 / 512), Quaternion.identity);
                                        }
                                        else
                                        {
                                            PhotonNetwork.Instantiate(BiomeGen[index].splatHeights[i].resourses[r].obj.name, new Vector3(x * 1000 / 512, terrainHeight, y * 1000 / 512), Quaternion.identity);
                                        }
                                    }
                                }
                            }
                        }
                        //else if (terrainHeight >= BiomeGen[index].splatHeights[i].startingHeight && terrainHeight <= BiomeGen[index].splatHeights[i + 1].startingHeight)
                        //{
                        //    for (int r = 0; r < BiomeGen[index].splatHeights[i].resourses.Length; r++)
                        //    {
                        //        if (Random.Range(0, BiomeGen[index].splatHeights[i].resourses[r].chance) == 0)
                        //        {
                        //            PhotonNetwork.Instantiate(BiomeGen[index].splatHeights[i].resourses[r].obj.name, new Vector3(x * 1000 / 512, terrainHeight, y * 1000 / 512), Quaternion.identity);
                        //        }
                        //    }
                        //}
                    } 
                }
            }
        }
    }
    public void generateNavMesh()
    {
        for (int i = 0; i < surfaces.Length; i++)
        {
            surfaces[i].BuildNavMesh();
        }
        GameManager.Instance.UpdateLoadingScreen(100);
        GameManager.Instance.SpawnPlayer();
    }
    [PunRPC]
    public void GetInfo()
    {
        if (HeightGen != 0)
        {
            PV.RPC("SetInfo", RpcTarget.Others, HeightGen, ColorGen);
        }
    }
    [PunRPC]
    public void SetInfo(int H, int C)
    {
        HeightGen = H;
        ColorGen = C;
    }
}
