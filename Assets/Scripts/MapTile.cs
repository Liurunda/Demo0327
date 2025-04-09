using UnityEngine;
using types;
using UnityEngine.Rendering.Universal;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Timeline;

public class MapTile : MonoBehaviour
{
    public int x,y,z; //地块左下角坐标
    public int X,Z;//地块大小
    public int my_color;
    static Color[] colors = {Color.red, Color.blue, Color.yellow};  
    private MeshRenderer meshRenderer; // 用于渲染
    private MeshFilter meshFilter;

    public Material tileMaterial; // 可配置材质

    public TileAlive alive = TileAlive.ALIVE; // 地图块状态
    public MapGenerator mapGenerator;

    public void Initialize(int _x, int _y, int _z, int _X, int _Z, Material material, MapGenerator mapGen)
    {
        x = _x;
        y = _y;
        z = _z;
        X = _X;
        Z = _Z;
        tileMaterial = material;
        mapGenerator = mapGen;
        RenderTile(); // 生成长方体
    }


    private void RenderTile()
    {
        // 添加 MeshFilter 和 MeshRenderer 组件
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();

        // 生成一个立方体
        meshFilter.mesh = CreateCubeMesh();

        // 设置材质
        meshRenderer.material = tileMaterial;
        //将my_color 赋值为 0....colors.size()的随机数： 
        my_color = Random.Range(0, colors.Length);
        meshRenderer.material.color = colors[my_color];
    }

    // 生成立方体地块的 Mesh, 地块大小可调节
    private Mesh CreateCubeMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = new Vector3[]
        {
            new Vector3(0, 0, 0), new Vector3(X, 0, 0),
            new Vector3(X, 1, 0), new Vector3(0, 1, 0),
            new Vector3(0, 0, Z), new Vector3(X, 0, Z),
            new Vector3(X, 1, Z), new Vector3(0, 1, Z)
        };
        mesh.triangles = new int[]
        {
            0, 2, 1, 0, 3, 2, // 前
            1, 2, 6, 1, 6, 5, // 右
            4, 5, 6, 4, 6, 7, // 后
            0, 7, 3, 0, 4, 7, // 左
            3, 6, 2, 3, 7, 6, // 上
            0, 1, 5, 0, 5, 4  // 下
        };
        mesh.RecalculateNormals();
        return mesh;
    }

    //ALIVE -> (touched) -> DYING -> (2s) -> DEAD
    //  如果Alive，则触发触地状态, 更新当前地块的alive状态为Dying 
    //  如果Dying, 则触发触地状态
    //  如果Dead，则不触发触地状态，继续下落
    public bool touch(){
        if(alive==TileAlive.DEAD){
            return false;
        }else{
            if(alive==TileAlive.ALIVE){
                die_start();
            }
            return true;
        }
    }
    void die_start() {
        meshRenderer.material.color = Color.gray;
        alive = TileAlive.DYING;
        mapGenerator.UpdateScore(my_color);
        StartCoroutine(DelayDieFinish()); 
    }

    IEnumerator DelayDieFinish() {
        yield return new WaitForSeconds(2f);
        die_finish(); 
    }

    //called 2 seconds after die_start()
    void die_finish(){
        alive = TileAlive.DEAD;
        meshRenderer.enabled = false;
    }
}

