//Player as a Ball 
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using types;
public class Player : MonoBehaviour //角色模型为倒四棱锥或球体，总之下方和地面接触的是一个点，省去一些麻烦
{
    public float speed = 10f;
    public float jumpForce = 10f;
    public float gravity = 9.81f;
    public float maxVelocity = 10f;
    public float maxVelocityY = 10f;
    List<int> Layers;

    void Start(){
        List<int> Layers = MapGenerator.layer_heights;
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
    void Update(){
        //判断触地的条件: 前一帧 高度>=地面，下一帧高度 <= 地面，则: 更新垂直速度为0，更新高度为等于地面。
        //判断下落
        //根据键盘输入，获取水平速度。简便实现，将方向键直接映射到固定速度。
    }

}