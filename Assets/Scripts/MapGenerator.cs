using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Rendering;
using System.Linq;

using Unity.VisualScripting;


public class MapGenerator : MonoBehaviour
{
    public GameObject tilePrefab,playerPrefab;
    public Material tileMaterial;
    public static int width = 10, length = 10, layer = 6, vertical_gap = 10;

    public static int tileSizeX = 2, tileSizeZ = 2;
    public static List<int> layer_heights = new List<int>();
    public static MapTile[,,] mapTiles;

    static public bool containXZ(int x, int z){
        return x>=0 && z>=0 && x<width && z < length;
    }

    //before_y -> after_y 是否穿越了某层地板的y坐标?
    static public int PenetratedLayer(float before_y, float after_y){ 
        if(before_y<layer_heights.First() || after_y>layer_heights.Last()){
            return -1;//高度超出地图垂直范围
        }
        int layer = 0;
        while(after_y>layer_heights[layer]){
            layer++;
        }
        if(before_y<layer_heights[layer]){
            return -1;//高度未穿越地面
        }
        return layer;    
    }

    void Start()
    {
        tilePrefab = Resources.Load<GameObject>("NewPrefab");
        playerPrefab = Resources.Load<GameObject>("PlayerPrefab");

        tileMaterial = new Material(Shader.Find("Unlit/Color"));

        mapTiles = new MapTile[layer, width, length];
        GenerateMap();
        SpawnPlayer();
        SetupCamera();
    }


    void GenerateMap()
    {
        for(int y=0;y<layer;y++){
            layer_heights.Add(y*vertical_gap+1);//y*vertical_gap为地块下表面高度, 地块自身高度为1 

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < length; z++)
                {
                    //地块左下角坐标：(x*tileSizeX,y*gap,z*tileSizeZ)
                    //地块右上角坐标：(x*tileSizeX+tileSizeX,y*gap+1,z*tileSizeZ+tileSizeZ)

                    Vector3 position = new Vector3(x*tileSizeX, y*vertical_gap, z*tileSizeZ);//0,gap,2*gap,3*gap...(layer-1)*gap
                    GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);
                    tile.name = $"Tile_{y}_{x}_{z}";
                    MapTile tileScript = tile.AddComponent<MapTile>(); // 添加 MapTile 组件
                    mapTiles[y, x, z] = tileScript;
                    tileScript.Initialize(x, y, z, tileSizeX, tileSizeZ, tileMaterial); // 让它渲染自己
                }
            }
        } 
    }

    void SpawnPlayer(){
        Vector3 playerInitialPosition = new Vector3(3, 35, 3);
        GameObject player = Instantiate(playerPrefab, playerInitialPosition, Quaternion.identity);
        player.name = "Player";
        Player playerScript = player.AddComponent<Player>();
    }

    void SetupCamera(){
        var mainCam = Camera.main;
        CameraController cameraController = mainCam.AddComponent<CameraController>();
        cameraController.Initialize();
    }

}