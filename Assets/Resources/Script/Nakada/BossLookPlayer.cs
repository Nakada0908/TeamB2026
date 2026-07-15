using UnityEngine;
using UnityEngine.UIElements;

public class BossLookPlayer : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    void Update()
    {
        transform.LookAt(playerTransform);
    }
}
