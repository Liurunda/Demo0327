using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Rendering;
using Unity.VisualScripting;


public class MapGenerator : MonoBehaviour
{
    public GameObject tilePrefab;
    public Material tileMaterial;
    public int width = 30, length = 30, layer = 6, vertical_gap = 10;
    public static List<int> layer_heights = new List<int>();
    public static MapTile[,,] mapTiles;

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
        mapTiles = new MapTile[layer, width, length];
        GenerateMap();
    }

    void GenerateMap()
    {
        for(int y=0;y<layer;y++){
            
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < length; z++)
                {
                    layer_heights.Add(y*vertical_gap+1);//y坐标为地块下表面高度, 地块自身高度为1
                    Vector3 position = new Vector3(x, y*vertical_gap, z);//0,gap,2*gap,3*gap...(layer-1)*gap
                    //使用Prefab初始化和C++使用constructor进行初始化有何不同？
                    GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);
                    tile.name = $"Tile_{y}_{x}_{z}";
                    //(x,y*gap,z)表示地块左下角坐标 地块右上角为(x+1,y*gap+1,z+1)
                    MapTile tileScript = tile.AddComponent<MapTile>(); // 添加 MapTile 组件
                    mapTiles[y, x, z] = tileScript;
                    tileScript.Initialize(x, y, z, tileMaterial); // 让它渲染自己
                }
            }

        }
    }
}


