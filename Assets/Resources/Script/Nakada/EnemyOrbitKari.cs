using UnityEngine;

public class EnemyOrbitKari : MonoBehaviour
{
    //中心となるオブジェクトのTransform
    public Transform center;
    //1秒間あたりの回転角度
    public float orbitSpeed = 50f;
    //右回りかどうかの判定フラグ
    public bool isClockwise = true;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (center == null)
        {
            return;
        }

        if (rb == null)
        {
            return;
        }

        //フレームレートに依存しないようTime.fixedDeltaTimeを乗算
        float step = orbitSpeed * Time.fixedDeltaTime;

        //左回りの場合は角度をマイナスに反転
        if (!isClockwise)
        {
            step = -step;
        }

        //クォータニオンを用いた回転座標の計算
        Quaternion rotation = Quaternion.AngleAxis(step, Vector3.up);
        Vector3 direction = rb.position - center.position;
        Vector3 nextPosition = center.position + rotation * direction;

        //物理演算を伴う移動の適用
        rb.MovePosition(nextPosition);
    }
}