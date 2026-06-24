using UnityEngine;
using UnityEngine.EventSystems;

public class EventTrigger : MonoBehaviour
{
    //[Header("Event Typeを選択")]
    //[SerializeField] private EventType eventType;
    private EventData currentData;

    //イベントマネージャーから周回ごとにデータを受け取る
    public void SetEvent(EventData data)
    {
        currentData = data;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //自身のインデックス番号とイベント発生位置を渡す
            //EventManager.Instance.ActivateEvent(eventType,this.transform);
            //Destroy(this.gameObject);

            //イベント発生の時間調整
            Invoke(nameof(ExecuteEvents), currentData.dropDelayTime);
            //イベントが起きたらいったんリセット
            currentData = null;
            //次のイベントを取得する
        }
    }

    private void ExecuteEvents()
    {
        //EventManager.Instance.ActivateEvent(currentData.dropType, this.transform);
    }
}