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
            //右に進むので反時計周りで設定
            //1時方向から始められるように+120と少し前方にするため-10
            float angleDeg = -30f - (i * 30f) + 120f　- 10f ;

            float angleRad = angleDeg * Mathf.Deg2Rad;

            float x = Mathf.Sin(angleRad) * radius;
            float z = Mathf.Cos(angleRad) * radius;

            eventPoints[i] = new EventPointData
            {
                position = new Vector3(x, 0f, z),
                rotation = Quaternion.Euler(0f, angleDeg, 0f)
            };
        }
    }
}
