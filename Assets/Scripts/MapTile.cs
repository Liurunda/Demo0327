using UnityEngine;
using types;
using UnityEngine.Rendering.Universal;
public class MapTile : MonoBehaviour
{
    public int x,y,z; //position of tile
    public int X,Z;//size of tile
    // 一个地图块是1*1*1的正方体，而这里的坐标是正方体中心的坐标。
    //角色设计为直径为1的球体，角色坐标为球心的坐标。当角色坐标高度和正方体地块坐标相差1的时候，就说明发生了碰撞。
    //由于地图十分规则，可以O(1)简单计算出当前坐标可能参与碰撞检测的正方体地块是谁。
    //碰撞检测时，需判断地图地块状态，是否已被破坏
    //已被破坏的地图地块，不参与碰撞检测，不被渲染
    private MeshRenderer meshRenderer; // 用于渲染
    private MeshFilter meshFilter;

    public Material tileMaterial; // 可配置材质

    public TileAlive alive = TileAlive.ALIVE; // 地图块状态


    public void Initialize(int _x, int _y, int _z, int _X, int _Z, Material material)
    {
        x = _x;
        y = _y;
        z = _z;
        X = _X;
        Z = _Z;
        tileMaterial = material;
        RenderTile(); // 生成长方体
    }
    public void die_start(){
        //TODO: 状态 Alive -> Dying, 安排一个固定延迟后的事件将Dying -> Dead
        meshRenderer.material.color = Color.gray;
        alive = TileAlive.DYING;
    }
    public void die_finish(){//called 2 seconds after die_start
        alive = TileAlive.DEAD;
        meshRenderer.enabled = false;
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
        
        // 生成随机颜色 (RGB 值范围 0~1)
        Color randomColor = new Color(Random.value, Random.value, Random.value, 0.5f);

        // 应用到材质的颜色
        meshRenderer.material.color = randomColor;

    }

    // 生成立方体的 Mesh
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

    public bool touch(){
        if(alive==TileAlive.DEAD){
            return false;
        }else{//ALIVE or DYING?
            if(alive==TileAlive.ALIVE){
                die_start();
            }
            return true;
        }
    }
}
