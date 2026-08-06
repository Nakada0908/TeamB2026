using UnityEngine;

public class PlayerCamera_Boss : MonoBehaviour
{

    public Transform CenterPoint;
    public Transform target;
    public testmove TestMove;
    public PlayerManager_Boss playermanager_boss;

    Vector3 defalutCameraOffset;    //デフォルトカメラ位置
    Quaternion defalutCameraDir;    //デフォルトカメラ方向
    
   
    // Update is called once per frame
    void FixedUpdate()
    {
        //敵とプレイヤーのベクトルを調べる
        Vector3 enemyOffset = target.position - CenterPoint.position;
        //正規化する。距離を取りやすいように
        enemyOffset.Normalize();
        //プレイヤーとカメラの距離の位置
        transform.position = target.position + (enemyOffset * 5f);
        //カメラが見つめる位置
        Vector3 Target = target.position + new Vector3(0, 1.5f, 0);
        //lookatでプレイヤー座標を見る
        transform.LookAt(Target);


        if (playermanager_boss.horizontal > 0)
        {
            transform.RotateAround(CenterPoint.position, Vector3.up, -playermanager_boss.speed * Time.deltaTime);
        }
        else if (playermanager_boss.horizontal < 0)
        {
            transform.RotateAround(CenterPoint.position, Vector3.up, playermanager_boss.speed * Time.deltaTime);
        }

    }
}
