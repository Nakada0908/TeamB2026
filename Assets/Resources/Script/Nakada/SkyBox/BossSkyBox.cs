using UnityEngine;

public class BossSkyBoxManager : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    void Update()
    {
        Vector3 targetPosition = playerTransform.position;
        targetPosition.y = transform.position.y;
        transform.LookAt(targetPosition);
    }
}