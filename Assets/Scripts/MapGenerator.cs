using UnityEngine;
using System;

public class MapGenerator : MonoBehaviour
{
    public GameObject tilePrefab;
    public Material tileMaterial;
    public int width = 5, height = 5;

    [RuntimeInitializeOnLoadMethod]
    static void InitializeOnStartup()
    {
        // 在程序启动时创建一个 MapGenerator 实例
        GameObject mapGeneratorObject = new GameObject("MapGenerator");
        MapGenerator mapGenerator = mapGeneratorObject.AddComponent<MapGenerator>();
        //mapGenerator.Start();  // 手动调用 Start() 方法
    }


    void Start()
    {
        Console.Write("Aha");
        tilePrefab = Resources.Load<GameObject>("NewPrefab");
        if (tilePrefab == null)
        {
            Debug.LogError("tilePrefab could not be loaded.");
            return;
        }

        GenerateMap();
    }

    void GenerateMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 position = new Vector3(x, 0, z);
                //使用Prefab初始化和C++使用constructor进行初始化有何不同？
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);
                tile.name = $"Tile_{x}_{z}";

                MapTile tileScript = tile.AddComponent<MapTile>(); // 添加 MapTile 组件
                tileScript.Initialize(x, z, tileMaterial); // 让它渲染自己
            }
        }
    }
}


