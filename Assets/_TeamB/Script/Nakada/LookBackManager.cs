using UnityEngine;
using UnityEngine.SceneManagement;

public class LookBackManager : MonoBehaviour
{
    [Header("演出が終わってから次のシーンへ")]
    [SerializeField] private float returnSceneTime = 17f;
    [SerializeField] private string returnSceneName = "Stage1Color";

    private PlayerManager_Rigid playerCntrol;

    private void Start()
    {
        SoundManager.instance.PlayBGMSound("Stage1");
    }

    void OnTriggerEnter(Collider other)
    {
        if (playerCntrol != null) return;

        if (other.CompareTag(TagConsts.Player))
        {
            playerCntrol = other.GetComponent<PlayerManager_Rigid>();

            //演出が終わったらステージ1へ戻す
            Invoke("ReturnScene", returnSceneTime);
        }
    }

    private void LateUpdate()
    {
        if (playerCntrol == null) return;

        //ジャンプが押された時だけ、接地していない事にして不発にする
        //FixedUpdateの直前に書き換えたいのでLateUpdate
        if (Input.GetButtonDown("Jump2"))
        {
            playerCntrol.onGround = false;
        }
    }

    private void ReturnScene()
    {
        SceneManager.LoadScene(returnSceneName);
    }
}
