using UnityEngine;

public class FireObject : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float deleteTime = 10f;

    private bool isMoving = false;
    private float moveTimer = 0f;

    //最初に置かれていた場所へ戻す
    private Vector3 homePosition;
    private Quaternion homeRotation;

    private void Awake()
    {
        homePosition = transform.position;
        homeRotation = transform.rotation;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void StartMove()
    {
        isMoving = true;
        moveTimer = 0f;
    }

    private void Update()
    {
        if (!isMoving)
        {
            return;
        }

        //直進
        transform.position += transform.forward * speed * Time.deltaTime;

        //時間が来たら、消さずに元の位置に戻す
        moveTimer += Time.deltaTime;
        if (moveTimer >= deleteTime)
        {
            ResetToHome();
        }
    }

    //元の位置に戻して非表示にする
    public void ResetToHome()
    {
        isMoving = false;
        moveTimer = 0f;

        transform.position = homePosition;
        transform.rotation = homeRotation;

        gameObject.SetActive(false);
    }

    //Sceneビューに発射方向の矢印を出す
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 25f);
    }
}
