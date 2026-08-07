//ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
//Playerの動き、回転
//ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
using UnityEngine;

public class PlayerAnimation_Boss : MonoBehaviour
{
    public Animator animator;
    public testmove TestMove;
    public PlayerManager_Boss playermanager_boss;
    Quaternion targetRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float animVelocityX =playermanager_boss.horizontal;
        animator.SetFloat("velocityX", animVelocityX);
        animator.SetFloat("velocityY", playermanager_boss.velocity.y); //これもおそらくジャンプ部分。ｙの値を変更しないと。
        animator.SetFloat("speedX", Mathf.Abs(playermanager_boss.horizontal));
        animator.SetBool("onGround", playermanager_boss.onGround);
        playermanager_boss.horizontal = Input.GetAxisRaw("Horizontal");
        //進行方向に合わせて回転する。
        /// HACK:Playerごと回転してしまう、Playerの方向を常に右に
        if (playermanager_boss.horizontal > 0)
        {
            //右
            gameObject.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);

        }
        else if (playermanager_boss.horizontal < 0)
        {
            //左

            gameObject.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        }
    }
}

