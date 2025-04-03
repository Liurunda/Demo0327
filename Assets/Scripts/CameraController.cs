// manage camera&player orientation 


using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour {
    Transform target;      // 绑定的目标物体
    Vector3 offset = new Vector3(0, 2, -5); // 摄像机相对目标的偏移量

    public void Initialize() {
        target = GameObject.Find("Player").transform;
        transform.position = target.position + offset;
    }
    void LateUpdate() {
        // 摄像机位置 = 目标位置 + 偏移量
        //从玩家那里获得朝向，按照玩家朝向计算相机偏移量, 然后相机lookat玩家
    
        Vector3 worldOffset = target.rotation * offset;

        transform.position = target.position + worldOffset;

        transform.LookAt(target.position);

    }
}