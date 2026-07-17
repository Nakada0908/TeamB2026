using UnityEngine;

public class testmove : MonoBehaviour
{
    public Transform centerPoint; // 回転の中心となるオブジェクト
    public float radius = 5f;     // 中心からの距離
    public float speed = 10f;     // 回転速度（度/秒）
    public float horizontal;

    private Rigidbody rb;         //Rigidbodyの呼び出し
    private float angle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 角度を計算
        horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal > 0)
        {
            //右
            angle += speed * Time.fixedDeltaTime;
        }
        else if (horizontal < 0)
        {
            //左
            angle -= speed * Time.fixedDeltaTime;
        }

        float radian = angle * Mathf.Deg2Rad;

        // 三角関数でXとZ（またはY）の円周上座標を計算
        float x = centerPoint.position.x + Mathf.Cos(radian) * radius;
        float z = centerPoint.position.z + Mathf.Sin(radian) * radius;
        Vector3 targetPosition = new Vector3(x, transform.position.y, z);

        ///FIXME:オブジェクトがすり抜けてしまう。一旦除外
        ///NOTE:AI曰く、毎フレーム強制的に移動し続けてしまったため
        //rb.MovePosition(targetPosition);


        ///HACK：すり抜けバグは解決したが、動作の理解が必要。
        // 現在地から目標座標への「移動ベクトル」を計算
        Vector3 moveVelocity = (targetPosition - transform.position) / Time.fixedDeltaTime;

        // Y軸（高さ）のブレを防ぐため、現在のY速度を維持（または0にする）
        moveVelocity.y = rb.linearVelocity.y; 

        // 速度を直接 Rigidbody に与える
        rb.linearVelocity = moveVelocity;
    }
}
