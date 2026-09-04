using UnityEngine;

public class BossHuwahuwa : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float height = 1.0f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        Vector3 pos = transform.position;
        pos.y = startPos.y + Mathf.Sin(Time.time * speed) * height;
        transform.position = pos;
    }
}
