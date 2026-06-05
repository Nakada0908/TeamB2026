using UnityEngine;
using UnityEngine.Animations;

public class PlayerAnimations : MonoBehaviour
{

    public PlayerManager playerManager;
    public Animator playerAnimator;

    public Quaternion targetRotation;

    public bool push;
    public bool pull;

    // Update is called once per frame
    void Update()
    {
        //プレイヤーの方向
        if (playerManager.currentStates != PlayerManager.EPlayerStates.MoveObject)
        {
            if (UnityEngine.Input.GetAxis("Horizontal") < -0.1f)
            { //左
                targetRotation = Quaternion.Euler(0, -90, 0);
            }
            else if (UnityEngine.Input.GetAxis("Horizontal") > 0.1f)
            { //右
                targetRotation = Quaternion.Euler(0, 90, 0);
            }
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,Time.deltaTime * 10f);





        //Animatorパラメーター変更
        playerAnimator.SetFloat("VelocityX", playerManager.characterController.velocity.x);
        playerAnimator.SetFloat("VelocityY", playerManager.characterController.velocity.y);
        playerAnimator.SetFloat("VelocityZ", playerManager.characterController.velocity.z);
        playerAnimator.SetBool("grounded", playerManager.characterController.isGrounded);


        //物の押し引き
        if (playerManager.currentStates == PlayerManager.EPlayerStates.MoveObject)
        {
            playerAnimator.SetBool("pull-idle", true);
        }
        else
        {
            playerAnimator.SetBool("pull-idle", false);
        }
        playerAnimator.SetBool("push", push);
        playerAnimator.SetBool("pull", pull);
        //Debug.Log(push + "," + pull);
    }
}
