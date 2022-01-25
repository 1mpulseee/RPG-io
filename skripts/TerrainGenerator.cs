using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TerrainGenerator : MonoBehaviour
{ 
    public Terrain TerrainMain;
    public int size = 513;
    public float[,] heights;
    public SplatHeights1[] splatHeights1;
    [System.Serializable]
    public class SplatHeights1
    {
        public int textureIndex;
        public int startingHeight;
    }
    public SplatHeights2[] splatHeights2;
    [System.Serializable]
    public class SplatHeights2
    {
        public int textureIndex;
        public int startingHeight;
    }
    public SplatHeights3[] splatHeights3;
    [System.Serializable]
    public class SplatHeights3
    {
        public int textureIndex;
        public int startingHeight;
    }
    private Texture2D tex;
    public float zoomR;
    public float zoomG;
    public float zoomB;
    public float zoomH;
    public float zoomH2;
    public float powR;
    public float powG;
    public float powB;
    public float powH;
    public float powH2;
    public float Hsize;
    public float Hsize2;
    private int ColorGen;
    private int HeightGen;


    public Biome1[] biome1;
    [System.Serializable]
    public class Biome1
    {
        public int Hight;
        public GameObject[] obj;
    }

    public NavMeshSurface[] surfaces;
    void Start()
    {
        CreateTexure();
        SetTerrainHeights();
        PaintTerrain();
        CreateTrees();
        generateNavMesh();
    }
    public void SetTerrainHeights()
    {
        HeightGen = Random.Range(1, 1000);
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
                float[] splat = new float[splatHeights1.Length + splatHeights2.Length + splatHeights3.Length];
                if (tex.GetPixel(x,y).r > 0.9)
                {
                    for (int i = 0; i < splatHeights1.Length; i++)
                    {
                            if (i == splatHeights1.Length - 1 && terrainHeight >= splatHeights1[i].startingHeight)
                            {
                                splat[splatHeights1[i].textureIndex] = 1;
                            }
                            else if (terrainHeight >= splatHeights1[i].startingHeight && terrainHeight <= splatHeights1[i + 1].startingHeight)
                            {
                                splat[splatHeights1[i].textureIndex] = 1;
                            }
                    }
                    for (int j = 0; j < splatHeights1.Length; j++)
                    {
                        splatmapData[x, y, splatHeights1[j].textureIndex] = splat[splatHeights1[j].textureIndex];
                    }
                }
                else if (tex.GetPixel(x, y).g > 0.9)
                {
                    for (int i = 0; i < splatHeights2.Length; i++)
                    {
                        if (i == splatHeights2.Length - 1 && terrainHeight >= splatHeights2[i].startingHeight)
                        {
                            splat[splatHeights2[i].textureIndex] = 1;
                        }
                        else if (terrainHeight >= splatHeights2[i].startingHeight && terrainHeight <= splatHeights2[i + 1].startingHeight)
                        {
                            splat[splatHeights2[i].textureIndex] = 1;
                        }
                    }
                    for (int j = 0; j < splatHeights2.Length; j++)
                    {
                        splatmapData[x, y, splatHeights2[j].textureIndex] = splat[splatHeights2[j].textureIndex];
                    }
                }
                else if (tex.GetPixel(x, y).b > 0.9)
                {
                    for (int i = 0; i < splatHeights3.Length; i++)
                    {
                        if (i == splatHeights3.Length - 1 && terrainHeight >= splatHeights3[i].startingHeight)
                        {
                            splat[splatHeights3[i].textureIndex] = 1;
                        }
                        else if (terrainHeight >= splatHeights3[i].startingHeight && terrainHeight <= splatHeights3[i + 1].startingHeight)
                        {
                            splat[splatHeights3[i].textureIndex] = 1;
                        }
                    }
                    for (int j = 0; j < splatHeights3.Length; j++)
                    {
                        splatmapData[x, y, splatHeights3[j].textureIndex] = splat[splatHeights3[j].textureIndex];
                    }
                }
            }
        }
        terrainData.SetAlphamaps(0, 0, splatmapData);
    }
    public void CreateTexure()
    {
        ColorGen = Random.Range(1, 1000);
        tex = new Texture2D(size, size);

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                Color c = Color.white;
                c.r = Mathf.Pow(Mathf.PerlinNoise((x + ColorGen * 100) / zoomR, y / zoomR), powR);
                c.g = Mathf.Pow(Mathf.PerlinNoise(x / zoomG, (y + ColorGen * 100) / zoomG), powG);
                c.b = Mathf.Pow(Mathf.PerlinNoise((x + ColorGen * 100) / zoomB, (y + ColorGen * 100) / zoomB), powB);
                if (c.r > c.g && c.r > c.b)
                {
                    c = Color.red;
                }
                else if (c.g > c.r && c.g > c.b)
                {
                    c = Color.green;
                }
                else if (c.b > c.r && c.b > c.g)
                {
                    c = Color.blue;
                }
                tex.SetPixel(x, y, c);
            }
        }
        tex.Apply();
    }
    public void CreateTrees()
    {
        TerrainData terrainData = TerrainMain.terrainData;
        for (int y = 0; y < terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < terrainData.alphamapWidth; x++)
            {
                Color pixel = tex.GetPixel(y, x);
                float terrainHeight = terrainData.GetHeight(x, y);
                if (pixel.r > 0.8f)
                {
                    for (int i = 0; i < biome1.Length; i++)
                    {
                        if (biome1[i].obj.Length != 0)
                        {
                            if (i == biome1.Length - 1 && terrainHeight >= biome1[i].Hight)
                            {
                                if (Random.Range(0, 25) == 0)
                                {
                                    Instantiate(biome1[i].obj[Random.Range(0, biome1[i].obj.Length)], new Vector3(x * 1000 / 512, terrainHeight, y * 1000 / 512), Quaternion.identity);
                                }
                            }
                            else if (terrainHeight >= biome1[i].Hight && terrainHeight <= biome1[i + 1].Hight)
                            {
                                if (Random.Range(0, 25) == 0)
                                {
                                    Instantiate(biome1[i].obj[Random.Range(0, biome1[i].obj.Length)], new Vector3(x * 1000 / 512, terrainHeight, y * 1000 / 512), Quaternion.identity);
                                }
                            }
                        }
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
    }
}
