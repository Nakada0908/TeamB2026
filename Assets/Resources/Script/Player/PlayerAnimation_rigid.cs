using UnityEngine;
using UnityEngine.Animations;
public class PlayerAnimation_rigid : MonoBehaviour
{
    public Animator animator;
    public PlayerManager_Rigid playerManager;

    Quaternion targetRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        float animVelocityX = playerManager.velocity.x / playerManager.maxSpeed;
        animator.SetFloat("velocityX", animVelocityX);
        animator.SetFloat("velocityY", playerManager.velocity.y);
        animator.SetFloat("speedX", Mathf.Abs(playerManager.velocity.x / 1.5f));
        animator.SetBool("onGround", playerManager.onGround);
        animator.SetBool("push", playerManager.push);
        animator.SetBool("pull", playerManager.pull);
        if (playerManager.currentStates == PlayerManager_Rigid.EPlayerStates.MoveObject)
        {
            animator.SetBool("moveObject", true);
        }
        else
        {
            animator.SetBool("moveObject", false);
        }
        if (playerManager.currentStates == PlayerManager_Rigid.EPlayerStates.Wallhold)
        {
            animator.SetBool("wallHold", true);
        }
        else
        {
            animator.SetBool("wallHold", false);
        }





        if (playerManager.currentStates != PlayerManager_Rigid.EPlayerStates.MoveObject &&
            playerManager.currentStates != PlayerManager_Rigid.EPlayerStates.Wallhold)
        {
            //左向くやつ
            if (playerManager.velocity.x < 0f)
            {
                targetRotation = Quaternion.Euler(0, -91f, 0);
            }        //右向くやつ
            else if (playerManager.velocity.x > 0f)
            {
                targetRotation = Quaternion.Euler(0, 91f, 0);
            }
        }
        else
        {
            targetRotation = playerManager.targetRotation;
        }
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
    }


}
