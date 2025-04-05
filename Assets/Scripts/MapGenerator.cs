using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Rendering;
using Unity.VisualScripting;


public class MapGenerator : MonoBehaviour
{
    public GameObject tilePrefab,playerPrefab;
    public Material tileMaterial;
    public static int width = 30, length = 30, layer = 6, vertical_gap = 10;

    public static int tileSizeX = 2, tileSizeZ = 2;
    public static List<int> layer_heights = new List<int>();
    public static MapTile[,,] mapTiles;

    // Add MapGenerator as a game object in the scene
    // [RuntimeInitializeOnLoadMethod]
    // static void InitializeOnStartup()
    // {
    //     // 在程序启动时创建一个 MapGenerator 实例
    //     GameObject mapGeneratorObject = new GameObject("MapGenerator");
    //     MapGenerator mapGenerator = mapGeneratorObject.AddComponent<MapGenerator>();
    // }

    static public bool contain(int x, int z){
        return x>=0 && z>=0 && x<width && z < length;
    }

    void Start()
    {
        Console.Write("Aha");
        tilePrefab = Resources.Load<GameObject>("NewPrefab");
        playerPrefab = Resources.Load<GameObject>("PlayerPrefab");

        tileMaterial = new Material(Shader.Find("Unlit/Color"));

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
            layer_heights.Add(y*vertical_gap+1);//y坐标为地块下表面高度, 地块自身高度为1 //这一句之前放到内层循环, 结果整了个大小2700的数组出来

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < length; z++)
                {
                    Vector3 position = new Vector3(x*tileSizeX, y*vertical_gap, z*tileSizeZ);//0,gap,2*gap,3*gap...(layer-1)*gap
                    //使用Prefab初始化和C++使用constructor进行初始化有何不同？
                    GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);
                    tile.name = $"Tile_{y}_{x}_{z}";
                    //(x*tileSizeX,y*gap,z*tileSizeZ)表示地块左下角坐标
                    //地块右上角为(x*tileSizeX+tileSizeX,y*gap+1,z*tileSizeZ+tileSizeZ)
                    MapTile tileScript = tile.AddComponent<MapTile>(); // 添加 MapTile 组件
                    mapTiles[y, x, z] = tileScript;
                    tileScript.Initialize(x, y, z, tileSizeX, tileSizeZ, tileMaterial); // 让它渲染自己
                }
            }
        } 

        Vector3 playerInitialPosition = new Vector3(3, 35, 3);
        GameObject player = Instantiate(playerPrefab, playerInitialPosition, Quaternion.identity);
        player.name = "Player";
        Player playerScript = player.AddComponent<Player>();
        GameObject mainCam = GameObject.Find("Main Camera");
        CameraController cameraController = mainCam.AddComponent<CameraController>();
        cameraController.Initialize();
    }
}


