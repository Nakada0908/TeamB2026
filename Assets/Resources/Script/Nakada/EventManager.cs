using System.Linq;
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
    [SerializeField] private Transform bossTransform;
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
        //引数でイベント発生地点渡し、イベント発生地点の上に出るようにする
        Vector3 pos = position.position;
        pos.y += 10f; //柱が地面から少し上に出るように調整

        //出現位置からボスへ向かう方向ベクトルを求める
        Vector3 direction = bossTransform.position - position.position;
        //高低差による傾きを防ぐためY軸の要素を0にする
        //direction.y = 0;
        //ボスの方向を向く回転に対して、プレハブの回転を合成する
        Quaternion lookRotation = Quaternion.LookRotation(direction) * hasira[0].transform.rotation;

        //合成した回転で生成
        Instantiate(hasira[0], pos, position.rotation);
        Debug.Log("Hasira spawned.");
    }

    private void SpawnEnemy(Transform position)
    {
        //敵はプレイヤーと同じく円移動するから、ボスと同じ位置に出現……
        //はだめか、……いや、ボスの回転を渡せばプレイヤーの位置にあわせられるからむしろいいか？

        //敵を生成する処理
        Vector3 pos = position.position;
        pos.y += 10f; //敵が地面から少し上に出るように調整
        Instantiate(enemy[0], pos, enemy[0].transform.rotation);
        Debug.Log("Enemy spawned.");
    }
}
