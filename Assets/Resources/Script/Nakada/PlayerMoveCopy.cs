using UnityEngine;

public class PlayerMoveCopy : MonoBehaviour
{
    //～～～～～～～～仮のプレイヤー移動～～～～～～～～　



    //変更:Startのコメントを削除せず維持しますが、このコードには元々存在しなかったためそのままにします。
    public GameObject Boss;
    //追加:物理演算用のRigidbodyコンポーネント
    public Rigidbody rb;
    //追加:ジャンプ力の設定値
    public float jumpForce = 5f;

    //追加:初期化時にRigidbodyを取得
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //追加:ジャンプ処理(スペースキー押下時)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //追加:上方向へ瞬間的な力を加える
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    //変更:物理演算に関わる移動処理のため、UpdateからFixedUpdateへ移動
    void FixedUpdate()
    {
        //追加:計算後の次フレーム座標と回転を保持する変数
        Vector3 nextPosition = rb.position;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            //削除:transform.RotateAround(Boss.transform.position, -Vector3.up, 0.5f);
            //変更:RotateAroundの代わりにクォータニオンを用いて回転後の座標を計算
            Quaternion rotation = Quaternion.AngleAxis(-0.5f, Vector3.up);
            Vector3 direction = nextPosition - Boss.transform.position;
            nextPosition = Boss.transform.position + rotation * direction;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //削除:transform.RotateAround(Boss.transform.position, Vector3.up, 0.5f);
            //変更:同様に左方向の回転座標を計算
            Quaternion rotation = Quaternion.AngleAxis(0.5f, Vector3.up);
            Vector3 direction = nextPosition - Boss.transform.position;
            nextPosition = Boss.transform.position + rotation * direction;
        }

        //追加:計算結果をRigidbody経由で適用することで障害物での貫通を防止
        rb.MovePosition(nextPosition);
    }
}