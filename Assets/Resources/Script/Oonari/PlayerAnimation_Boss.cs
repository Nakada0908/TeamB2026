//ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
//Playerの動き、回転
//ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerAnimation_Boss : MonoBehaviour
{
    public Animator animator;
    public testmove TestMove;

    Quaternion targetRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float animVelocityX =TestMove.horizontal;
        animator.SetFloat("velocityX", animVelocityX);
        animator.SetFloat("speedX", Mathf.Abs(TestMove.horizontal));

        TestMove.horizontal = Input.GetAxisRaw("Horizontal");
        //進行方向に合わせて回転する。
        /// HACK:Playerごと回転してしまう、Playerの方向を常に右に
        if (TestMove.horizontal > 0)
        {
            //右
            gameObject.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);

        }
        else if (TestMove.horizontal < 0)
        {
            //左

            gameObject.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        }
    }
}

