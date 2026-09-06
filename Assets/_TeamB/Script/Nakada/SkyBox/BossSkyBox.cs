using UnityEngine;

public class BossSkyBoxManager : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float smoothing = 3.0f;//遅れる度合い

    void Start()
    {
        //初めにプレイヤーの方に向ける
        Vector3 targetPosition = playerTransform.position;
        targetPosition.y = transform.position.y;
        transform.LookAt(targetPosition);
    }

    //プレイヤーとカメラが FixedUpdate で動くので、それに合わせる
    void FixedUpdate()
    {
        //Y軸のみを回転させる
        Vector3 targetPosition = playerTransform.position;
        targetPosition.y = transform.position.y;
        Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);

        //ワンテンポ遅れて追従させる
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * smoothing
        );
    }
}
