using System.Collections;
using UnityEngine;

public class EndingDirector : MonoBehaviour
{
    [Header("演出設定")]
    public Transform player;
    public Transform walkTarget1;
    public Transform walkTarget2;

    [Header("カメラ演出")]
    public Transform cameraTarget;
    public GameObject zoomOutCamera;

    [Header("少女の幻影")]
    public GameObject girlShadow;       // 【新增】少女的影子模型
    public Animator girlAnimator;       // 【新增】少女的动画控制器

    [Header("演出パラメータ (時間設定)")]
    public float firstPauseTime = 1.0f;
    public float firstStareTime = 1.5f;
    public float secondPauseTime = 1f;
    public float secondStareTime = 1.5f;
    public float leaveScreeenTime = 4.5f;
    public float shadowAppearDelay = 2.0f; // 【新增】镜头拉远后，等几秒少女才出现
    public float finalFadeDelay = 3.0f;    // 【新增】少女挥手后，等几秒再黑屏

    [Header("感情パラメータ")]
    public float cutsceneTurnSpeed = 4f;  
    public float cutsceneWalkSpeed = 1.5f;  

    private PlayerManager_Rigid playerController;
    private PlayerAnimation_rigid playerAnim;
    private bool hasTriggered = false;

    void Start()
    {
        playerController = player.GetComponent<PlayerManager_Rigid>();
        playerAnim = player.GetComponentInChildren<PlayerAnimation_rigid>();

        if (girlShadow != null) girlShadow.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered || !other.CompareTag("Player")) return;

        hasTriggered = true;

        StartCoroutine(PlayEndingCutscene());
    }
    IEnumerator PlayEndingCutscene()
    {
        // 1.演出開始
        playerController.isCutsceneMode = true;
        playerController.simulatedMoveInput = Vector2.zero;
        playerAnim.turnSpeed = cutsceneTurnSpeed;      
        playerController.maxSpeed = cutsceneWalkSpeed;

        // 2.ポイント1へ移動
        playerController.simulatedMoveInput = new Vector2(1f, 0f);
        while (player.position.x < walkTarget1.position.x) yield return null;

        // 3.立ち止まる
        playerController.simulatedMoveInput = Vector2.zero;
        yield return new WaitForSeconds(firstPauseTime);

        // 4.振り返る
        playerController.simulatedMoveInput = new Vector2(-1f, 0f);
        yield return new WaitForSeconds(0.1f);
        playerController.simulatedMoveInput = Vector2.zero;

        yield return WaitForFacing(269f);
        yield return new WaitForSeconds(firstStareTime);

        // 5.ポイント2へ移動
        playerController.simulatedMoveInput = new Vector2(1f, 0f);
        while (player.position.x < walkTarget2.position.x) yield return null;

        // 6.立ち止まる
        playerController.simulatedMoveInput = Vector2.zero;
        yield return new WaitForSeconds(secondPauseTime);

        // 7.振り返る
        playerController.simulatedMoveInput = new Vector2(-1f, 0f);
        yield return new WaitForSeconds(0.1f);
        playerController.simulatedMoveInput = Vector2.zero;

        yield return WaitForFacing(269f);
        yield return new WaitForSeconds(secondStareTime);

        // 8.カメラを置き去りにして立ち去る
        if (cameraTarget != null) cameraTarget.SetParent(null);

        playerController.simulatedMoveInput = new Vector2(1f, 0f);
        yield return new WaitForSeconds(leaveScreeenTime);

        //9.カメラ切り替え
        if (zoomOutCamera != null) zoomOutCamera.SetActive(true);

        yield return new WaitForSeconds(shadowAppearDelay);

        //10. 少女の幻影が出現し、手を振る
        if (girlShadow != null && girlAnimator != null)
        {
            girlShadow.SetActive(true);
            girlAnimator.SetTrigger("Wave");
        }

        yield return new WaitForSeconds(finalFadeDelay);

        // 11. 劇終
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