using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    [Header("Event Typeを選択")]
    [SerializeField] private EventType eventType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //自身のインデックス番号とイベント発生位置を渡す
            EventManager.Instance.ActivateEvent(eventType,this.transform);
            Destroy(this.gameObject);
        }
    }
}