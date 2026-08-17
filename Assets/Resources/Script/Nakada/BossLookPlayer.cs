using UnityEngine;
using UnityEngine.UIElements;

public class BossLookPlayer : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    void Update()
    {
        //Y軸のみを回転させる
        Vector3 targetPosition = playerTransform.position;
        targetPosition.y = transform.position.y;
        transform.LookAt(targetPosition);
    }
}
