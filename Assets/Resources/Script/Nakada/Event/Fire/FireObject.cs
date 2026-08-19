using UnityEngine;

public class FireObject : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float deleteTime = 10f;

    private bool isMoving = false;

    public void StartMove()
    {
        isMoving = true;
        Destroy(gameObject, deleteTime);
    }

    private void Update()
    {
        if (!isMoving)
        {
            return;
        }

        //直進
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    //Sceneビューに発射方向の矢印を出す
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 25f);
    }
}
