using UnityEngine;

public class BossSaveManager : MonoBehaviour
{
    public static BossSaveManager instance;

    //参照
    [SerializeField] private GameObject playerObject;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private EventManager eventManager;
    [SerializeField] private WallChange wallChange;
    [SerializeField] private FireManager fireManager;

    //保持するもの
    private Vector3 savePlayerPos;
    private Quaternion savePlayerRotate;
    private Vector3 saveCameraPos;
    private Quaternion saveCameraRotate;
    private int saveEventId;
    private int saveLapCount;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        saveLapCount = wallChange.lapCount;

        SavePlayer();
    }

    private void Update()
    {
        //周回数が増えたタイミングでセーブする
        if (wallChange.lapCount != saveLapCount)
        {
            saveLapCount = wallChange.lapCount;
            SavePlayer();
        }
    }

    public void SavePlayer()
    {
        savePlayerPos = playerObject.transform.position;
        savePlayerRotate = playerObject.transform.rotation;

        saveCameraPos = cameraTransform.position;
        saveCameraRotate = cameraTransform.rotation;

        //イベントのID保存
        saveEventId = eventManager.currentEventIndex - 1;
        if (saveEventId < 0)
        {
            saveEventId = 0;
        }

        //発射物が次に何番目か覚えておく
        fireManager.SaveIndex();
    }

    public void ResetToSavePlayer()
    {
        //出現済みオブジェクトの削除
        foreach (var o in FindObjectsByType<DeleteObjects>(FindObjectsSortMode.None))
        {
            Destroy(o.gameObject);
        }

        //ディレイ中のイベントを止める
        eventManager.StopEvents();

        //プレイヤーの位置と回転をリセット
        playerObject.transform.position = savePlayerPos;
        playerObject.transform.rotation = savePlayerRotate;
        //内部で持ってる角度も戻す。これが無いと動いた瞬間に死んだ場所へ戻される
        playerObject.GetComponent<PlayerManager_Boss>().ResetAngle();
        //カメラの位置と回転をリセット
        cameraTransform.position = saveCameraPos;
        cameraTransform.rotation = saveCameraRotate;

        //ジャンプや衝突で残ってた勢いを消す
        Rigidbody playerRigidbody = playerObject.GetComponent<Rigidbody>();
        playerRigidbody.linearVelocity = Vector3.zero;
        playerRigidbody.angularVelocity = Vector3.zero;

        //プレイヤーを動かしてから、周回の計測をやり直す
        //これもないと周回度数が増えたままになってしまう
        wallChange.ResetRotation();

        //イベントIDを渡してコライダーの位置リセット
        eventManager.currentEventIndex = saveEventId;
        eventManager.SetNextEventCollider();

        //発射物を元の位置に戻す
        fireManager.ResetFire();
    }
}