using UnityEngine;

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

        //進行方向に合わせて回転する。
        ///HACK:Playerごと回転してしまう、Playerの方向を常に右に
        if (TestMove.horizontal < 0)
        {
            //右
            targetRotation = Quaternion.Euler(0, 170f, 0);
            
        }
        else if (TestMove.horizontal > 0)
        {
            //左
            targetRotation = Quaternion.Euler(0, 0f, 0);
            
        }
        
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
    }
}

