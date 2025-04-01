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
        transform.position = target.position + offset;
    }
}