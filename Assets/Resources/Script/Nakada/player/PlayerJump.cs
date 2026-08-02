using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private Rigidbody rb;
    private const float minGroundDotProduct = 0.7f;
    private float jumpHeight = 2f;
    private bool isJump;
    private bool onGround;
    private AudioSource audioSource;//音は未実装

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJump = true;
        }
    }

    private void FixedUpdate()
    {
        //入力、着地してるときのみ
        if (isJump)
        {
            if (onGround)
            {
                Jump();
            }
            //ジャンプの実行有無に関わらず入力フラグをリセットする
            isJump = false;
        }

        //次フレームの物理演算のために接地フラグをリセットする
        onGround = false;
    }

    private void Jump()
    {
        rb.linearVelocity += Vector3.up * Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);

        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        EvaluateCollision(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        EvaluateCollision(collision);
    }

    private void EvaluateCollision(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;

            if (normal.y >= minGroundDotProduct)
            {
                onGround = true;
            }
        }
    }
}