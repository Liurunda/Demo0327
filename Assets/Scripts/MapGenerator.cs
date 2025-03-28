using UnityEngine;
using System;

public class MapGenerator : MonoBehaviour
{
    public GameObject tilePrefab;
    public Material tileMaterial;
    public int width = 6, height = 6, layer = 4;

    [RuntimeInitializeOnLoadMethod]
    static void InitializeOnStartup()
    {
        // 在程序启动时创建一个 MapGenerator 实例
        GameObject mapGeneratorObject = new GameObject("MapGenerator");
        MapGenerator mapGenerator = mapGeneratorObject.AddComponent<MapGenerator>();
    }


    void Start()
    {
        Console.Write("Aha");
        tilePrefab = Resources.Load<GameObject>("NewPrefab");
        tileMaterial = new Material(Shader.Find("Unlit/Color"));
        //tileMaterial = new Material(Shader.Find("Unlit/Transparent"));

        if (tilePrefab == null)
        {
            Debug.LogError("tilePrefab could not be loaded.");
            return;
        }

        GenerateMap();
    }

    void GenerateMap()
    {
        for(int y=0;y<layer;y++){
            
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    Vector3 position = new Vector3(x, y*3, z);
                    //使用Prefab初始化和C++使用constructor进行初始化有何不同？
                    GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);
                    tile.name = $"Tile_{y}_{x}_{z}";

                    MapTile tileScript = tile.AddComponent<MapTile>(); // 添加 MapTile 组件
                    tileScript.Initialize(x, y, z, tileMaterial); // 让它渲染自己
                }
            }

        }
    }
}


