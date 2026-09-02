//ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
//Playerを追従するカメラ
//ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
using UnityEngine;

public class PlayerCamera_Boss : MonoBehaviour
{

    [SerializeField] private Transform CenterPoint;
    [SerializeField] private Transform target;
    public testmove TestMove;
    public PlayerManager_Boss playermanager_boss;
   
    // Update is called once per frame
    void FixedUpdate()
    {

        //敵とプレイヤーのベクトルを調べる
        Vector3 enemyOffset = target.position - CenterPoint.position;

        //正規化する。距離を取りやすいように
        enemyOffset.Normalize();

        //プレイヤーとカメラの距離
        Vector3 cameraPosition = target.position + (enemyOffset * 7f);

        // Y軸はカメラの現在位置のまま固定
        // NOTE:ジャンプ時一緒にy軸が動かないように固定している。
        cameraPosition.y = transform.position.y;
        transform.position = cameraPosition;

        //カメラが見つめる位置
        Vector3 lookTarget = target.position + new Vector3(1.5f, 1.5f, 0);
        


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
