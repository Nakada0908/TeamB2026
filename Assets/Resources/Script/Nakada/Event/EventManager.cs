using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EventType
{
    //イベントの種類を定義
    HasiraSpawn = 0,
    EnemySpawn = 100,
}

//JSON読み込み用のクラス
[System.Serializable]
public class EventData
{
    [HideInInspector]
    public int eventId;
    //JSONロード時に計算した配列インデックスを保持する変数
    [HideInInspector]
    public int pointIndex;
    public EventType dropType;
    public int clockPosition;
    public float dropDelayTime;
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
    [SerializeField] private GameObject[] hasira;
    [SerializeField] private GameObject[] enemy;
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
        }

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

    //プレイヤーとの接触判定とコルーチン
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

    public void ActivateEvent(EventData currentEvent, EventPointData targetPoint)
    {
        switch (currentEvent.dropType)
        {
            case EventType.HasiraSpawn:
                SpawnHasira(targetPoint);
                break;
            case EventType.EnemySpawn:
                SpawnEnemy(targetPoint);
                break;
            default:
                Debug.LogWarning("Unknown event type: " + currentEvent.dropType);
                break;
        }
    }

    #region 各種イベントの発生処理
    private void SpawnHasira(EventPointData position)
    {
        //柱を生成する処理
        Vector3 pos = position.position;
        pos.y += 10f; //柱が地面から少し上に出るように調整

        //合成した回転で生成
        Instantiate(hasira[0], pos, position.rotation);
        Debug.Log("Hasira spawned.");
    }

    private void SpawnEnemy(EventPointData position)
    {
        Vector3 pos = position.position;
        pos.y += 10f; //敵が地面から少し上に出るように調整

        Instantiate(enemy[0], pos, position.rotation);
        Debug.Log("Enemy spawned.");
    }
    #endregion
}