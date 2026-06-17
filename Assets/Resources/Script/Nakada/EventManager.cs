using UnityEngine;

public enum EventType
{
    //イベントの種類を定義
    HasiraSpawn,
    EnemySpawn,
}

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    //[SerializeField] private GameObject[] eventObjects;
    //dropsObujectsって名前にして、落とすオブジェクトを配列化
    //んで、要素数もenumでマジックナンバーを避ける
    //……ってかEventTypeのenumをintにキャストすれば、配列のインデックスとして使えるから
    //enumの順番と配列の順番を合わせればいいんじゃないかな
    [SerializeField] private GameObject[] hasira;
    [SerializeField] private GameObject[] enemy;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ActivateEvent(EventType eventType, Transform spawnPosition)
    {
        switch (eventType)
        {
            case EventType.HasiraSpawn:
                SpawnHasira(spawnPosition);
                break;
            case EventType.EnemySpawn:
                SpawnEnemy(spawnPosition);
                break;
            default:
                Debug.LogWarning("Unknown event type: " + eventType);
                break;
        } 
    }
    

    private void SpawnHasira(Transform position)
    {
        //柱を生成する処理
        //引数でイベント発生地点を生成位置にする
        Vector3 pos = position.position;
        pos.y += 10f; //柱が地面から少し上に出るように調整
        //プレハブの回転をそのまま使う
        Instantiate(hasira[0], pos, hasira[0].transform.rotation);
        //今は位置を直指定しているが、制御できるようにする
        //hasira[0].transform.localPosition = new Vector3(10, 10, 0);
        Debug.Log("Hasira spawned.");
    }

    private void SpawnEnemy(Transform position)
    {
        //敵を生成する処理
        Vector3 pos = position.position;
        pos.y += 10f; //敵が地面から少し上に出るように調整
        Instantiate(enemy[0], pos, enemy[0].transform.rotation);
        Debug.Log("Enemy spawned.");
    }
}
