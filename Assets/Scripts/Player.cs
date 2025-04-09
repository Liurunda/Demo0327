//Player as a Ball 
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System;
using types;
using UnityEditor.Rendering;

public class Player : MonoBehaviour //角色为直径为1的球体，角色坐标为球体最低点的坐标。

{
    public float jumpForce = 10f;
    public float gravity = 9.81f;
    public float speedXZ = 0.05f;
    public float speedY = 0f;
    int tileX, tileZ;

    public float mouseSensitivity = 200f;

    void Start(){
        tileX = MapGenerator.tileSizeX;
        tileZ = MapGenerator.tileSizeZ;
    }
    
    void Update(){
        HandleMouseLook();
    }

    void FixedUpdate(){
        
        HandleMove();
    }

    void HandleMouseLook()
    {
        // 1. 获取鼠标水平移动量
        // "Mouse X" 是 Unity 默认的鼠标左右移动输入轴
        float mouseX = Input.GetAxis("Mouse X");

        // 2. 计算旋转角度
        // 乘以灵敏度并乘以时间增量，使得旋转速度与帧率无关且可调
        float rotationAmountY = mouseX * mouseSensitivity * Time.deltaTime;

        // 3. 应用旋转
        // Vector3.up 代表世界坐标系的 Y 轴 (垂直轴)
        // 这会使物体绕着世界的垂直轴旋转，即左右转头
        transform.Rotate(Vector3.up * rotationAmountY);
    }

    void HandleMove(){
         //获取before向量
        //水平移动: 根据键盘输入，获取水平速度。简便实现，将方向键直接映射到固定速度。当前水平方向没有碰撞。    
        //垂直移动: 需要考虑重力加速度, 才能做出"跳跃"的动作。
        //根据重力加速度更新当前垂直速度 ，再根据当前垂直速度更新当前高度。
        //随后判断触地: 如果触地，则更新垂直速度为0，更新高度为等于当前层数的地面。
        //判断触地的条件: 前一帧 高度>=地面，下一帧高度 <= 地面
        
        Vector3 before = transform.position;
        Vector3 after = before;
        Vector3 delta = new Vector3(0, 0, 0);
        
        //根据键盘输入获取水平移动方向, 进行位移分解
        float right = Input.GetAxis("Horizontal");
        float front = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(right, 0, front);
        delta = direction.normalized * speedXZ;

        //根据重力加速度更新当前垂直速度 ，再根据当前垂直速度更新当前高度。
        speedY -= gravity*0.002f;
        if(speedY<-0.1){
            speedY=-0.1f;//限制下落速度
        }
        delta.y += speedY;
        after = before + delta;
        if(grounded(before,after)){
            speedY = 0;
            delta.y = (float)Math.Floor(before.y)-before.y;
            if(Input.GetKey(KeyCode.Space)){
                speedY = 0.4f;
                delta.y+=speedY;
            }
        }
        transform.Translate(delta);
    }

    bool grounded(Vector3 before, Vector3 after){//before.y和after.y均使用角色模型的最低y坐标
        //1. 判断当前Player位于哪一层地板(直接遍历, 常数不大) (maybeGrounded)
        //2. 判断当前Player的高度是否穿越了这层地板的y (maybeGrounded)
        //3. 找到对应的地块
        //4. 判断当前Player触碰到的地块是否为alive状态 -> MapTile.touch()进行相应的更新 
        //当某个地块从Dying变为Dead时，如果有玩家位于其上方，需要让玩家开始下落
        
        int layer = MapGenerator.PenetratedLayer(before.y, after.y);
        // -1: no layer of floor is penetrated

        if(layer == -1){
            return false;//高度未穿越地面
        }  

        //接下来找到对应layer的对应x/z坐标的地块, 
        int x = (int)Math.Floor(before.x/tileX);
        int z = (int)Math.Floor(before.z/tileZ);

        if(!MapGenerator.containXZ(x,z)){
            return false;//(x,z)坐标超出地图范围
        }
        
        MapTile tile = MapGenerator.mapTiles[layer,x,z];
        return tile.touch();//根据对应的地块是否alive, 进行处理

    }
}