using UnityEngine;
using UnityEngine.InputSystem;

public class testmove : MonoBehaviour
{
    public Transform centerPoint; // 回転の中心となるオブジェクト
    public float radius = 5f;     // 半径
    public float speed = 10f;     // 回転速度（度/秒）
    public float horizontal;

    private Rigidbody rb;         //Rigidbodyの呼び出し
    public float angle;           //角度
    
  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public float rotationSpeed = 360f; // 回転速度


    //ジェヒちゃんが作ってくれたAI---------------------------------------------
    // Update is called once per frame
    void FixedUpdate()
    {
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

        // 三角関数でXとZの円周上座標を計算
        float radian = angle * Mathf.Deg2Rad;
        float x = centerPoint.position.x + Mathf.Cos(radian) * radius;
        float z = centerPoint.position.z + Mathf.Sin(radian) * radius;

        Vector3 targetPosition =
            new Vector3(x, transform.position.y, z);
        

        if (horizontal != 0)
        {
            Vector3 moveDirection =
                (targetPosition - rb.position).normalized;

            moveDirection.y = 0f;

            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation =
                    Quaternion.LookRotation(moveDirection);

                Quaternion nextRotation =
                    Quaternion.RotateTowards(
                        rb.rotation,
                        targetRotation,
                        rotationSpeed * Time.fixedDeltaTime
                    );

                rb.MoveRotation(nextRotation);
            }
        }
        rb.MovePosition(targetPosition);

    }

}
