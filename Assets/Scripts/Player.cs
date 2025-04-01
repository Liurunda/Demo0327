//Player as a Ball 
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using types;

public class Player : MonoBehaviour //角色模型为倒四棱锥或球体，总之下方和地面接触的是一个点，省去一些麻烦
{
    public float jumpForce = 10f;
    public float gravity = 9.81f;
    public float speedXZ = 0.02f;
    public float speedY = 0f;
    List<int> Layers;

    void Start(){
        Layers = MapGenerator.layer_heights;
    }
    bool grounded(Vector3 before, Vector3 after){
        //当不考虑地块碰撞时，角色发生垂直方向位移时，检测是否触发下落到地面的碰撞事件
        //before.y和after.y均使用角色模型的最低y坐标
        //1. 判断当前Player位于哪一个Layer(直接遍历, 常数不大)
        //2. 判断当前Player在高度上是否能够触地（before高于当前Layer的地面高度，after低于当前Layer的地面高度）
        //3. 判断当前Player触碰到的地块是否为alive状态————> 
        //  如果Alive，则更新当前地块的alive状态为Dying 
        //  如果Dying, 则触发触地状态
        //  如果Dead，则不触发触地状态，继续下落
        //另外，当某个地块从Dying变为Dead时，如果有玩家位于其上方，则开始下落
        if(before.y<Layers.First() || after.y>Layers.Last()){
            return false;//高度超出地图垂直范围
        }
        int l = 0;
        while(after.y>Layers[l]){
            l++;
        }
        //assert: after.y<=Layers[l]
        if(before.y<Layers[l]){
            return false;//高度未穿越地面
        }
        //在高度意义上触地, 判断地块是否alive
        //接下来找到对应layer的对应x/z坐标的地块, 进行查询和处理
        int x = (int)Math.Floor(before.x);
        int z = (int)Math.Floor(before.z);
        Debug.Log("before: "+before+" after: "+after+" l: "+l+" x: "+x+" z: "+z);
        MapTile tile = MapGenerator.mapTiles[l,x,z];
        if(tile.alive==TileAlive.DEAD){
            return false;
        }else{
            if(tile.alive==TileAlive.ALIVE){
                tile.die();
            }
            return true;
        }
    }
    void FixedUpdate(){
        //获取before向量
        //水平移动: 根据键盘输入，获取水平速度。简便实现，将方向键直接映射到固定速度。当前水平方向没有碰撞。    
        //垂直移动: 需要考虑重力加速度, 才能做出"跳跃"的动作。
        //根据重力加速度更新当前垂直速度 ，再根据当前垂直速度更新当前高度。
        //随后判断触地: 如果触地，则更新垂直速度为0，更新高度为等于当前层数的地面。
        //判断触地的条件: 前一帧 高度>=地面，下一帧高度 <= 地面
        Vector3 before = transform.position;
        Vector3 after = before;
        
        //暂时不考虑摄像头的位置移动，只考虑玩家自身的移动，之后再添加摄像头追随玩家移动和改变朝向
        
        //根据键盘输入获取水平速度  
        float right = Input.GetAxis("Horizontal");
        float front = Input.GetAxis("Vertical");
        after.x += right*speedXZ;
        after.z += front*speedXZ;//更精确的实现: 将speedXZ按照sin/cos分解到x，z方向上
        //根据重力加速度更新当前垂直速度 ，再根据当前垂直速度更新当前高度。
        speedY -= gravity*0.02f;
        if(speedY<-0.1)speedY=-0.1f;//限制下落速度
        after.y += speedY;
        if(grounded(before,after)){
            speedY = 0;
            after.y = (float)Math.Floor(before.y);
        }
        transform.position = after;//好了，现在可以球体下落碰到地板了,但依然无法水平移动，而且碰到地板之后就开始大量的数组越界Exception。调试一下。
    }

}