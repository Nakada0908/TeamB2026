//ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
//Playerの移動
//ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
using UnityEngine;

public class testmove : MonoBehaviour
{
    public float radius = 5f;          // 半径
    public float speed = 10f;          // 移動速度
    public float horizontal;　　　     // 入力元
    public Transform centerPoint;      // 回転の中心となるオブジェクト
                                       
    private float rotationSpeed = 360f; // 回転速度
    private float angle;              　// 角度
    private float rayDistance = 0.3f; 　// レイの長さ
    private float rayOffset   = 0.5f;
    private Rigidbody rb;             //Rigidbodyの呼び出し

    private const float minGroundDotProduct = 0.7f;
    private float jumpHeight = 2f;
    private bool isJump;
    private bool onGround;
    private AudioSource audioSource;  //未実装
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        //仮の変数の中に角度を更新していく
        float nextangle = angle;

        horizontal = Input.GetAxisRaw("Horizontal");
        //playerの移動
        if (horizontal > 0)
        {
            //右
            nextangle += speed * Time.fixedDeltaTime;
        }
        else if (horizontal < 0)
        {
            //左
            nextangle -= speed * Time.fixedDeltaTime;

        }

        ///NOTE:playerが中央のオブジェクトの周りを周回する。
        // 三角関数でXとZの円周上座標を計算
        float radian = nextangle * Mathf.Deg2Rad;
        float x = centerPoint.position.x + Mathf.Cos(radian) * radius;
        float z = centerPoint.position.z + Mathf.Sin(radian) * radius;
        Vector3 targetPosition =
            new Vector3(x, rb.position.y, z);

        ///NOTE:playerが移動に合わせて回転する。（自然な円移動に見せるため）
        ///TODO:AIによって出された答え、中身の理解が必要。
        if (horizontal != 0)
        {
            Vector3 moveDirection =
                (targetPosition - rb.position).normalized;

            moveDirection.y = 0f;



            if (moveDirection != Vector3.zero)
            {
                //回転の軸を見る
                Quaternion targetRotation =
                    Quaternion.LookRotation(moveDirection);

                Quaternion nextRotation =
                    Quaternion.RotateTowards(
                        rb.rotation,
                        targetRotation,
                        rotationSpeed * Time.fixedDeltaTime
                    );

                rb.MoveRotation(nextRotation);

                //Rayによる衝突判定
                Vector3 rayposition = transform.position + moveDirection * rayOffset;

                Ray ray = new Ray(rayposition, moveDirection);
                //Rayの描画。作動してるかチェック
                Debug.DrawRay(rayposition, moveDirection * rayDistance, Color.red);

                //衝突時移動ストップ
                RaycastHit hit;
                if (Physics.Raycast(
                      ray,
                      out hit,
                      rayDistance,
                      Physics.DefaultRaycastLayers,
                      QueryTriggerInteraction.Ignore))
                {
                 
                    Debug.Log("何かに当たった" + gameObject.name);
                }
                else
                {
                    //衝突していない時のみ角度を更新する。
                    angle = nextangle;
                    rb.MovePosition(targetPosition);
                }
            }
        }



    }
}
