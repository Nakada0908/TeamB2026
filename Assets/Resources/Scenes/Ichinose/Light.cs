using UnityEngine;

public class FaceLight : MonoBehaviour
{
    public Transform player;
    public Transform face;     // 顔の位置
    public float height = 1.5f;
    public float front = 0.4f;

    void LateUpdate()
    {
        // ライトをプレイヤーの少し前に配置
        transform.position =
            player.position +
            Vector3.up * height +
            player.forward;

        // ライトから顔への方向
        Vector3 dir = face.position - transform.position;

        // 顔を向く
        transform.rotation = Quaternion.LookRotation(dir);
    }
}