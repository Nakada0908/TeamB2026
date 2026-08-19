using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EEventType
{
    EventType_None = 0,
    //イベントの種類を定義
    Hasira = 1,

    LeftNui = 100,
    RightNui =101,

    Eye =110,

    LeftMiniYagi=120,
    RightMiniYagi=121,

    Fire =200,
}

//読み込み用のクラス
[System.Serializable]
public class EventData
{
    //自動設定されるもの
    public int eventId;
    //dropTypeStrをEventTypeに変換
    public EEventType eventType;
    //コライダー移動地点、生成地点を配列用に変換
    public int colliderPositionNum;
    public int spawnPositionNum;
    public float dropDelayTime;
}

public class EventManager : MonoBehaviour
{
    //出現位置
    [SerializeField] private SetEventPos eventPos;
    //FireNoticeに渡す
    [SerializeField] private FireManager fireManager;
    //出現オブジェクトをTypeとオブジェクトをセットにして設定
    private Dictionary<EEventType, GameObject> dropObjects = new Dictionary<EEventType, GameObject>();
    //イベントのデータ
    private List<EventData> eventList = new List<EventData>();
    [SerializeField] private TextAsset eventCsvFile;
    private int currentEventIndex = 0;

    private EventData currentEventData;
    private EventPointData currentTargetPoint;

    void Awake()
    {
        LoadCsvList();
    }

    private void Start()
    {
        LoadObjects();
        SetNextEventCollider();
    }

    //次のイベント地点へ移動させる
    private void SetNextEventCollider()
    {
        if (currentEventIndex >= eventList.Count)
        {
            //SetActiveだとイベントごと消えるので、コライダーだけを無効化する
            GetComponent<Collider>().enabled = false;
            return;
        }

        currentEventData = eventList[currentEventIndex];
        currentTargetPoint = eventPos.eventPoints[currentEventData.colliderPositionNum];

        transform.position = currentTargetPoint.position;
        transform.rotation = currentTargetPoint.rotation;

        currentEventIndex++;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagConsts.Player))
        {
            EventPointData spawnPoint = eventPos.eventPoints[currentEventData.spawnPositionNum];
            StartCoroutine(StartEvent(currentEventData, spawnPoint));
            SetNextEventCollider();
        }
    }

    private IEnumerator StartEvent(EventData eventData, EventPointData spawnPoint)
    {
        //イベントの発生を遅延させる
        yield return new WaitForSeconds(eventData.dropDelayTime);
        ActivateEvent(eventData, spawnPoint);
    }

    //共通のイベント発生処理
    private void ActivateEvent(EventData currentEvent, EventPointData targetPoint)
    {
        //イベントタイプがあったらオブジェクトを取得する
        if (dropObjects.TryGetValue(currentEvent.eventType, out GameObject prefab))
        {
            Vector3 pos = targetPoint.position;
            pos.y += 10f;
            GameObject spawnedObject = Instantiate(prefab, pos, targetPoint.rotation);

            //発射通知オブジェクトだった場合は、発射先のマネージャを渡す
            FireNotice notice = spawnedObject.GetComponent<FireNotice>();
            if (notice != null)
            {
                notice.fireManager = fireManager;
            }

            Debug.Log(currentEvent.eventType.ToString() + "がスポーンしたよ");
        }
        else
        {
            Debug.LogError("存在しないイベントを指定してるよ！ID:"+currentEvent.eventId);
        }
    }

    private void LoadCsvList()
    {
        //eventCsvFileを行ごとに分けて、空白があった場合は無視する
        string[] lines = eventCsvFile.text.Split(new char[] { '\n', '\r' },
            System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');

            if (values.Length < 4) continue;

            EventData newData = new EventData();
            newData.eventId = i - 1;

            //読み込んだ文字列をEnumに変換して直接代入
            if (System.Enum.TryParse(values[0], out EEventType parsedType))
            {
                newData.eventType = parsedType;
            }
            else
            {
                newData.eventType = EEventType.EventType_None;
                Debug.LogError("無効なEventType文字列です(行 " + (i + 1) + "): " + values[0]);
            }

            //各項目の調整
            newData.colliderPositionNum = int.Parse(values[1]) - 1;
            newData.spawnPositionNum = int.Parse(values[2]) - 1;
            newData.dropDelayTime = float.Parse(values[3]);

            eventList.Add(newData);
        }
    }

    #region オブジェクトの読み込み
    private void LoadObjects()
    {
        string eventFoldar = "Script/Nakada/Event/EventObjecs/";

        //エラー防止の空オブジェクト
        dropObjects[EEventType.EventType_None] = Resources.Load<GameObject>(eventFoldar+"NoneObject");

        //障害物のスポーン
        dropObjects[EEventType.Hasira]= Resources.Load<GameObject>(eventFoldar+"Hasira");

        //敵のスポーン
        dropObjects[EEventType.LeftNui]= Resources.Load<GameObject>(eventFoldar+ "LeftMoveNui");
        dropObjects[EEventType.RightNui] = Resources.Load<GameObject>(eventFoldar + "RightMoveNui");

        dropObjects[EEventType.Eye] = Resources.Load<GameObject>(eventFoldar + "Eye");

        dropObjects[EEventType.LeftMiniYagi] = Resources.Load<GameObject>(eventFoldar + "LeftMiniYagi");
        dropObjects[EEventType.RightMiniYagi] = Resources.Load<GameObject>(eventFoldar + "RightMiniYagi");

        //発射通知のスポーン
        dropObjects[EEventType.Fire] = Resources.Load<GameObject>(eventFoldar + "FireNotice");
    }
    #endregion
}