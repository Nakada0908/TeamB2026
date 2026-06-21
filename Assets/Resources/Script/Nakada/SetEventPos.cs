using UnityEngine;

public class SetEventPos : MonoBehaviour
{
    public GameObject eventStartPos;
    public Transform centerTransform;
    [SerializeField] private float radius = 5f;

    public EventTrigger[] eventTriggers;

    void Awake()
    {
        eventTriggers = new EventTrigger[12];

        //30度ずつ生成と配置を行う
        for (int i = 0; i < 12; i++)
        {
            float angleDeg = i * 30f;
            //角度をラジアンに変換
            float angleRad = angleDeg * Mathf.Deg2Rad;

            float x = Mathf.Sin(angleRad) * radius;
            float z = Mathf.Cos(angleRad) * radius;

            GameObject newObj = Instantiate(eventStartPos, centerTransform);
            newObj.transform.localPosition = new Vector3(x, 0f, z);
            //生成したオブジェクトが中心を向くように回転させる
            newObj.transform.localRotation = Quaternion.Euler(0f, angleDeg, 0f);

            EventTrigger trrigerComponent=newObj.GetComponent<EventTrigger>();
            eventTriggers[i] = trrigerComponent;
        }
    }
}
