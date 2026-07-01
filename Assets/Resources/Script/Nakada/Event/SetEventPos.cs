using UnityEngine;

public struct EventPointData
{
    public Vector3 position;
    public Quaternion rotation;
}

public class SetEventPos : MonoBehaviour
{
    [SerializeField] private Transform centerTransform;
    [SerializeField] private float radius = 5f;

    public EventPointData[] eventPoints;

    void Awake()
    {
        eventPoints = new EventPointData[12];

        //30度ずつ生成と配置を行う
        for (int i = 0; i < 12; i++)
        {
            //右に進むので反時計周りで設定、1時方向から始められるように+180
            float angleDeg = -30f - (i * 30f) + 180f;

            float angleRad = angleDeg * Mathf.Deg2Rad;

            float x = Mathf.Sin(angleRad) * radius;
            float z = Mathf.Cos(angleRad) * radius;

            //GameObject newObj = Instantiate(eventStartPos, centerTransform);
            //newObj.transform.localPosition = new Vector3(x, 0f, z);
            ////生成したオブジェクトが中心を向くように回転させる
            //newObj.transform.localRotation = Quaternion.Euler(0f, angleDeg, 0f);

            //EventTrigger trrigerComponent=newObj.GetComponent<EventTrigger>();
            //eventTriggers[i] = trrigerComponent;

            //↓↓↓実際に配置するのから位置、回転の情報のみを渡す形式に変更↓↓↓

            eventPoints[i] = new EventPointData
            {
                position = new Vector3(x, 0f, z),
                rotation = Quaternion.Euler(0f, angleDeg, 0f)
            };
        }
    }
}
