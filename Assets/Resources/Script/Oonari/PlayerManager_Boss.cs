//ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
//Playerスクリプト（移動、ジャンプ、当たり判定、回転）
//ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
//TODO:停止中にonGroundがtrueにならない。そこを修正 8/26
using UnityEngine;

public class PlayerManager_Boss : MonoBehaviour
{
    //　　-移動に使う変数-

    public float speed = 10f;          // 円上を移動する速さ
    public float horizontal;           // 入力値  
    public Vector3 velocity;
    [SerializeField] private Transform centerPoint;  // 回転の中心となるオブジェクト
    [SerializeField] private float radius = 25f;     // 半径

    //　　-回転の内部変数ー
    
    private float turnSpeed = 10f;      // 回転速度
    private float angle;              　// 角度
    
    //    -障害物判定-
    
    private float rayDistance = 0.3f; 　// レイの長さ
    private float rayOffset = 0.5f;     // レイをplayerの中心から前へずらす
    private Rigidbody rb;

    //    -ジャンプ・離地判定-

    public bool onGround;             // 地面に接してるか。
    private bool isJump;　　　　　　　 // ジャンプ入力を受け取ったか。
    private const float minGroundDotProduct = 0.7f;
    private float jumpHeight = 2f;　　 //ジャンプの高さ
    private AudioSource audioSource;   //未実装

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    
    }

    // Update is called once per frame
    void Update()
    {
        velocity = rb.linearVelocity;
    }
    private void FixedUpdate()
    {
        
        if (Input.GetButtonDown("Jump2"))
        {
            isJump = true;
        }

        //　NOTE:壁に当たっていない場合のみ、円周上の位置を更新
        rb.linearVelocity = velocity;
        float nextangle = angle;
        //playerの移動
        if (horizontal > 0)
        {
            nextangle += speed * Time.fixedDeltaTime;
        }
        else if (horizontal < 0)
        {
            nextangle -= speed * Time.fixedDeltaTime;
        }

        //NOTE:playerが中央のオブジェクトの周りを周回する。
        float radian = nextangle * Mathf.Deg2Rad;
        float x = centerPoint.position.x + Mathf.Cos(radian) * radius;
        float z = centerPoint.position.z + Mathf.Sin(radian) * radius;
        Vector3 targetPosition = new Vector3(x, rb.position.y, z);

        //入力、着地してるときのみ
        if (isJump)
        {
            if (onGround)
            {
                Jump();
            }
            //ジャンプの実行有無に関わらず入力フラグをリセットする
            isJump = false;
        }
        //次フレームの物理演算のために接地フラグをリセットする
        onGround = false;   

        //Player回転処理ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
        //NOTE:playerが移動に合わせて回転する。（自然な円移動に見せるため）
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
                    Quaternion.Slerp(
                        rb.rotation,
                        targetRotation,
                        turnSpeed * Time.fixedDeltaTime
                    );

                rb.MoveRotation(nextRotation);

                //Rayによる衝突判定
                Vector3 rayposition = transform.position + moveDirection * rayOffset;
                Ray ray = new Ray(rayposition, moveDirection);
                
                //Rayの描画。(確認用)
                Debug.DrawRay(rayposition, moveDirection * rayDistance, Color.red);

                RaycastHit hit;
                if (!Physics.Raycast(
                      ray,
                      out hit,
                      rayDistance,
                      Physics.DefaultRaycastLayers,
                      QueryTriggerInteraction.Ignore))
                {
                    //衝突していない時のみ角度を更新する。
                    angle = nextangle;
                    rb.MovePosition(targetPosition);
                }
            }
        }



    }
    //ジャンプ実装ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
private void Jump()
    {
        rb.linearVelocity += Vector3.up * Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
        ///TODO:ジャンプ音用の AudioSourceを設定する
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        EvaluateCollision(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        EvaluateCollision(collision);
    }
    //地面接触判定
    private void EvaluateCollision(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;

            if (normal.y >= minGroundDotProduct)
            {
                onGround = true;
            }
        }
    }

    //セーブによるリセットーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public void ResetAngle()
    {
        angle = 0f;
    }
}
