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
    //追加:JSONロード時に計算した配列インデックスを保持する変数
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
    [SerializeField] private SetEventPos setEventPos;
    //出現オブジェクトを設定
    [SerializeField] private GameObject[] hasira;
    [SerializeField] private GameObject[] enemy;
    //イベントのデータ
    [SerializeField] private List<EventData> eventList = new List<EventData>();
    [SerializeField] private TextAsset eventJsonFile;
    private int currentEventIndex = 0;

    void Awake()
    {
        //Jsonファイルからイベントデータを読み込み
        EventDataContainer loadedData = JsonUtility.FromJson<EventDataContainer>(eventJsonFile.text);
        eventList = loadedData.eventData;
        //イベントIDを設定及び時計のように位置JSONに書いたので、それを調整する
        for (int i = 0; i < eventList.Count; i++)
        {
            eventList[i].eventId = i;
            //削除:int dropPointNum = eventList[i].clockPosition - 1;
            //変更:ローカル変数ではなくデータクラスに保持させる
            eventList[i].pointIndex = eventList[i].clockPosition - 1;
        }

        SetNextEventCollider();
    }

    //追加:次のイベント地点へ自身(コライダー)を移動させるメソッド
    private void SetNextEventCollider()
    {
        if (currentEventIndex >= eventList.Count)
        {
            gameObject.SetActive(false);
            return;
        }

        EventData nextEvent = eventList[currentEventIndex];
        EventPointData targetPoint = setEventPos.eventPoints[nextEvent.pointIndex];

        transform.position = targetPoint.position;
        gameObject.SetActive(true);
    }

    //追加:プレイヤーとの接触判定とコルーチンの開始
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EventData dataToExecute = eventList[currentEventIndex];
            EventPointData spawnPoint = setEventPos.eventPoints[dataToExecute.pointIndex];

            StartCoroutine(ExecuteEventCoroutine(dataToExecute, spawnPoint));

            currentEventIndex++;
            SetNextEventCollider();
        }
    }

    //追加:遅延発火を管理するコルーチン
    private IEnumerator ExecuteEventCoroutine(EventData eventData, EventPointData spawnPoint)
    {
        yield return new WaitForSeconds(eventData.dropDelayTime);
        ActivateEvent(eventData, spawnPoint);
    }

    //削除:public void ActivateEvent(EventType eventType, Transform spawnPosition)
    //変更:引数をイベントデータと座標データに変更
    public void ActivateEvent(EventData currentEvent, EventPointData targetPoint)
    {
        //eventPointsの配列から対象の座標データを取得する
        //EventPointData targetPoint = eventPoints[eventList.dropPointNum];

        //変更:currentEventのプロパティを参照するように変更
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
    //削除:private void SpawnHasira(Transform position)
    //変更:引数をEventPointDataに変更
    private void SpawnHasira(EventPointData position)
    {
        //柱を生成する処理
        //引数でイベント発生地点渡し、イベント発生地点の上に出るようにする
        //削除:Vector3 pos = position.position;
        //変更:EventPointDataのpositionを参照
        Vector3 pos = position.position;
        pos.y += 10f; //柱が地面から少し上に出るように調整

        //出現位置からボスへ向かう方向ベクトルを求める
        //Vector3 direction = bossTransform.position - position.position;
        //ボスの方向を向く回転に対して、プレハブの回転を合成する
        //Quaternion lookRotation = Quaternion.LookRotation(direction) * hasira[0].transform.rotation;

        //合成した回転で生成
        //削除:Instantiate(hasira[0], pos, position.rotation);
        //変更:EventPointDataのrotationを参照
        Instantiate(hasira[0], pos, position.rotation);
        Debug.Log("Hasira spawned.");
    }

    //削除:private void SpawnEnemy(Transform position)
    //変更:引数をEventPointDataに変更
    private void SpawnEnemy(EventPointData position)
    {
        //敵はプレイヤーと同じく円移動するから、ボスと同じ位置に出現……
        //はだめか、……いや、ボスの回転を渡せばプレイヤーの位置にあわせられるからむしろいいか？

        //敵を生成する処理
        //削除:Vector3 pos = position.position;
        //変更:EventPointDataのpositionを参照
        Vector3 pos = position.position;
        pos.y += 10f; //敵が地面から少し上に出るように調整
                      //削除:Instantiate(enemy[0], pos, enemy[0].pointData.rotation);
                      //変更:EventPointDataのrotationを参照
        Instantiate(enemy[0], pos, position.rotation);
        Debug.Log("Enemy spawned.");
    }
    #endregion
}