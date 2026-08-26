using UnityEngine;

public class BossSaveManager : MonoBehaviour
{
    public static BossSaveManager instance;

    //参照
    [SerializeField] private GameObject playerObject;
    [SerializeField] private EventManager eventManager;
    [SerializeField] private WallChange wallChange;

    //保持するもの
    private Vector3 playerPos;
    private Quaternion playerRotate;
    private int eventId;

    //周回が増えたかを見張るため、前に見た値を覚えておく
    private int lastLapCount;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //lastLapCount = wallChange.lapCount;

        //スタート地点を0周目のセーブポイントにする
        SavePlayer();
    }

    private void Update()
    {
        //周回数が増えたタイミングでセーブする
        //if (wallChange.lapCount != lastLapCount)
        //{
        //    lastLapCount = wallChange.lapCount;
        //    SavePlayer();
        //}
    }

    public void SavePlayer()
    {
        //周回の切れ目はスタートと同じ場所。高さが変わっても合うように毎回控える
        playerPos = playerObject.transform.position;
        playerRotate = playerObject.transform.rotation;

        //イベントのID保存
        //currentEventIndexは「次に構える番号」なので、今構えてる番号は1つ前
        eventId = Mathf.Max(0, eventManager.currentEventIndex - 1);
    }

    public void ResetToSavePlayer()
    {
        //出現済みオブジェクトの削除
        foreach (var o in FindObjectsByType<DeleteObjects>(FindObjectsSortMode.None))
        {
            Destroy(o.gameObject);
        }

        //プレイヤーの位置と回転をリセット
        playerObject.transform.position = playerPos;
        playerObject.transform.rotation = playerRotate;
        //内部で持ってる角度も戻す。これが無いと動いた瞬間に死んだ場所へ戻される
        //playerObject.GetComponent<PlayerManager_Boss>().ResetAngle();
        //落ちてた勢いを消す
        playerObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;

        //プレイヤーを動かしてから、周回の計測をやり直す
        //wallChange.ResetRotation();

        //イベントIDを渡してコライダーの位置リセット
        eventManager.currentEventIndex = eventId;
        eventManager.SetNextEventCollider();
    }
}
