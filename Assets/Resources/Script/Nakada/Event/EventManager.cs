using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    EventType_None = 0,
    //イベントの種類を定義
    HasiraSpawn = 1,

    EnemySpawn = 100,
}

//JSON読み込み用のクラス
[System.Serializable]
public class EventData
{
    //JSONから取得するもの
    public string eventTypeStr;
    public int clockPosition;
    public float dropDelayTime;

    //自動設定されるもの
    [HideInInspector] public int eventId;
    //dropTypeStrをEventTypeに変換したのを保持
    [HideInInspector] public EventType eventType;
    //移動地点を配列用に変換したのを保持
    [HideInInspector] public int pointIndex;
}

//Jsonの配列を読み込むために必要なラッパークラス
[System.Serializable]
public class EventDataContainer
{
    public List<EventData> eventData;
}

public class EventManager : MonoBehaviour
{
    //出現位置
    [SerializeField] private SetEventPos eventPos;
    //出現オブジェクトを設定
    private Dictionary<EventType, GameObject> dropObjects = new Dictionary<EventType, GameObject>();
    //イベントのデータ
    [SerializeField] private List<EventData> eventList = new List<EventData>();
    [SerializeField] private TextAsset eventJsonFile;
    private int currentEventIndex = 0;

    private EventData currentEventData;
    private EventPointData currentTargetPoint;

    void Awake()
    {
        //Jsonファイルからイベントデータを読み込み
        EventDataContainer loadedData = JsonUtility.FromJson<EventDataContainer>(eventJsonFile.text);
        eventList = loadedData.eventData;
        //イベントIDを設定及び時計のように位置をJSONに書いたので、それを調整する
        for (int i = 0; i < eventList.Count; i++)
        {
            eventList[i].eventId = i;
            eventList[i].pointIndex = eventList[i].clockPosition - 1;

            //JSONの文字列からEnum型へ変換する
            if (System.Enum.TryParse(eventList[i].eventTypeStr, out EventType parsedType))
            {
                eventList[i].eventType = parsedType;
            }
            else
            {
                eventList[i].eventType = EventType.EventType_None;
                Debug.LogError("無効なEventType文字列です: " + eventList[i].eventTypeStr);
            }
        }
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

        //次のイベントデータからイベント地点の位置と回転を取得して、コライダーを移動させる
        currentEventData = eventList[currentEventIndex];
        currentTargetPoint = eventPos.eventPoints[currentEventData.pointIndex];

        //自身を移動
        transform.position = currentTargetPoint.position;
        transform.rotation = currentTargetPoint.rotation;

        //次のイベントへ進めておく
        currentEventIndex++;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(StartEvent(currentEventData, currentTargetPoint));
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
    public void ActivateEvent(EventData currentEvent, EventPointData targetPoint)
    {
        //イベントタイプがあったらオブジェクトを取得する
        if (dropObjects.TryGetValue(currentEvent.eventType, out GameObject prefab))
        {
            Vector3 pos = targetPoint.position;
            pos.y += 10f;
            Instantiate(prefab, pos, targetPoint.rotation);
            Debug.Log(currentEvent.eventType.ToString() + "がスポーンしたよ");
        }
        else
        {
            Debug.LogError("存在しないイベントを指定してるよ！ID:"+currentEvent.eventId);
        }
    }

    #region オブジェクトの読み込み
    private void LoadObjects()
    {
        //障害物のスポーン
        dropObjects[EventType.HasiraSpawn]= Resources.Load<GameObject>("Script/Nakada/Hasira");

        //敵のスポーン
        dropObjects[EventType.EnemySpawn]= Resources.Load<GameObject>("Script/Nakada/Enemy");
    }
    #endregion
}