using UnityEngine;

public class MapTile : MonoBehaviour
{
    public int x,y,z; // 坐标
    private MeshRenderer meshRenderer; // 用于渲染
    private MeshFilter meshFilter;

    public Material tileMaterial; // 可配置材质

    public void Initialize(int _x, int _y, int _z, Material material)
    {
        x = _x;
        y = _y;
        z = _z;
        tileMaterial = material;
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
            new Vector3(-0.5f, 0, -0.5f), new Vector3(0.5f, 0, -0.5f),
            new Vector3(0.5f, 1, -0.5f), new Vector3(-0.5f, 1, -0.5f),
            new Vector3(-0.5f, 0, 0.5f), new Vector3(0.5f, 0, 0.5f),
            new Vector3(0.5f, 1, 0.5f), new Vector3(-0.5f, 1, 0.5f)
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
}
