using UnityEngine;

public class NuiMove : MonoBehaviour
{
    [SerializeField] private bool isLeftRotation = true;
    private float LR;
    [SerializeField] private float rotationSpeed = 7f;
    private bool isOnGround = false;
    private Rigidbody rb;

    private Animator walkAnime;

    private void Start()    
    {
        rb = GetComponent<Rigidbody>();
        LR = isLeftRotation ? 1f : -1f;
        walkAnime = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        if (!isOnGround)
        {
            return;
        }

        Vector3 centerPos;
        centerPos = new Vector3(0f, rb.position.y, 0f);

        Quaternion rotationDelta = Quaternion.Euler(0f, rotationSpeed * LR * Time.fixedDeltaTime, 0f);
        Vector3 currentOffset = rb.position - centerPos;
        Vector3 newPosition = centerPos + (rotationDelta * currentOffset);

        rb.MovePosition(newPosition);
        rb.MoveRotation(rb.rotation * rotationDelta);

        //テストで円移動
        //transform.RotateAround(centerPos, Vector3.up, rotationSpeed * direction * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(TagConsts.Ground))
        {
            isOnGround = true;
            if(isLeftRotation)
            {
                transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                if (walkAnime != null)
                {
                    walkAnime.SetTrigger("walk");
                }            
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                if (walkAnime != null)
                {
                    walkAnime.SetTrigger("walk");
                }
            }
        }
    }
}