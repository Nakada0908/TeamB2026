using System.Collections;
using UnityEngine;

public class EndingDirector : MonoBehaviour
{
    [Header("演出設定")]
    public Transform player;
    public Transform walkTarget1;
    public Transform walkTarget2;

    [Header("演出パラメータ")]
    public float firstPauseTime = 1.0f;
    public float firstStareTime = 2.5f;
    public float secondPauseTime = 0.5f;
    public float secondStareTime = 3.0f;
    public float leaveScreeenTime = 4.0f;

    private PlayerManager_Rigid playerController;

    void Start()
    {
        playerController = player.GetComponent<PlayerManager_Rigid>();
        StartCoroutine(PlayEndingCutscene());
    }

    IEnumerator PlayEndingCutscene()
    {
        playerController.isCutsceneMode = true;

        playerController.simulatedMoveInput = new Vector2(1f, 0f);
        while (player.position.x < walkTarget1.position.x)
        {
            yield return null;
        }

        playerController.simulatedMoveInput = Vector2.zero;
        yield return new WaitForSeconds(firstPauseTime);

        playerController.simulatedMoveInput = new Vector2(-1f, 0f);
        yield return new WaitForSeconds(0.1f);
        playerController.simulatedMoveInput = Vector2.zero;
        yield return new WaitForSeconds(firstStareTime);

        playerController.simulatedMoveInput = new Vector2(1f, 0f);
        yield return new WaitForSeconds(0.2f);

        while (player.position.x < walkTarget2.position.x)
        {
            yield return null;
        }

        playerController.simulatedMoveInput = Vector2.zero;
        yield return new WaitForSeconds(secondPauseTime);

        playerController.simulatedMoveInput = new Vector2(-1f, 0f);
        yield return new WaitForSeconds(0.1f);
        playerController.simulatedMoveInput = Vector2.zero;
        yield return new WaitForSeconds(secondStareTime);

        playerController.simulatedMoveInput = new Vector2(1f, 0f);
        yield return new WaitForSeconds(leaveScreeenTime);

        playerController.simulatedMoveInput = Vector2.zero;
        FadeToBlackAndEnd();
    }

    private void FadeToBlackAndEnd()
    {
        Debug.Log("演出終了");
    }
}
