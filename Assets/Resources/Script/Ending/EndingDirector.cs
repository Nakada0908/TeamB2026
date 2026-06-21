using System.Collections;
using UnityEngine;

public class EndingDirector : MonoBehaviour
{
    [Header("演出設定")]
    public Transform player;
    public Transform walkTarget1;
    public Transform walkTarget2;

    [Header("演出パラメータ (時間設定)")]
    public float firstPauseTime = 1.0f;
    public float firstStareTime = 2.5f;
    public float secondPauseTime = 0.5f;
    public float secondStareTime = 3.0f;
    public float leaveScreeenTime = 4.0f;

    [Header("感情パラメータ")]
    public float cutsceneTurnSpeed = 2.5f;  
    public float cutsceneWalkSpeed = 3.0f;  
    public float waveDuration = 2.5f; 

    private PlayerManager_Rigid playerController;
    private PlayerAnimation_rigid playerAnim;
    private float originalTurnSpeed;
    private float originalWalkSpeed;

    void Start()
    {
        playerController = player.GetComponent<PlayerManager_Rigid>();
        
        playerAnim = player.GetComponentInChildren<PlayerAnimation_rigid>();

        StartCoroutine(PlayEndingCutscene());
    }

    IEnumerator PlayEndingCutscene()
    {
        // 1.
        playerController.isCutsceneMode = true;

        originalTurnSpeed = playerAnim.turnSpeed;
        originalWalkSpeed = playerController.maxSpeed;

        playerAnim.turnSpeed = cutsceneTurnSpeed;      
        playerController.maxSpeed = cutsceneWalkSpeed; 

        // 2.walk to point1
        playerController.simulatedMoveInput = new Vector2(1f, 0f);
        while (player.position.x < walkTarget1.position.x)
        {
            yield return null;
        }

        // 3.first stop
        playerController.simulatedMoveInput = Vector2.zero;
        yield return new WaitForSeconds(firstPauseTime);

        // 4.first look back
        playerController.simulatedMoveInput = new Vector2(-1f, 0f);
        yield return new WaitForSeconds(0.1f);
        playerController.simulatedMoveInput = Vector2.zero;

        // 
        yield return WaitForFacing(269f);

        // 
        yield return new WaitForSeconds(firstStareTime);

        // 5.walk to point2
        playerController.simulatedMoveInput = new Vector2(1f, 0f);
        // 
        while (player.position.x < walkTarget2.position.x)
        {
            yield return null;
        }

        // 6.second stop
        playerController.simulatedMoveInput = Vector2.zero;
        yield return new WaitForSeconds(secondPauseTime);

        // 7.second look back
        playerController.simulatedMoveInput = new Vector2(-1f, 0f);
        yield return new WaitForSeconds(0.1f);
        playerController.simulatedMoveInput = Vector2.zero;

        // 
        yield return WaitForFacing(269f);

        // 
        yield return new WaitForSeconds(secondStareTime);

        Debug.Log("wave animation");
        playerAnim.animator.SetTrigger("wave");

        yield return new WaitForSeconds(waveDuration);

        // 9.leave
        playerController.simulatedMoveInput = new Vector2(1f, 0f);
        yield return new WaitForSeconds(leaveScreeenTime);

        // end
        playerController.simulatedMoveInput = Vector2.zero;
        FadeToBlackAndEnd();
    }
    IEnumerator WaitForFacing(float targetY)
    {
        while (Mathf.Abs(Mathf.DeltaAngle(playerAnim.transform.eulerAngles.y, targetY)) > 2f)
        {
            yield return null;
        }
    }
    private void FadeToBlackAndEnd()
    {
        Debug.Log("end...");
    }
}