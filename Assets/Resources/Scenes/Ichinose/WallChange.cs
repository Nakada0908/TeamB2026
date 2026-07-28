using UnityEngine;

public class WallChange : MonoBehaviour
{
    [Header("壁")]
    public GameObject[] oldWall;
    public GameObject[] newWall;

    [Header("対象")]
    public Transform player;
    public Transform boss;

    // 現在切り替える壁番号
    private int wallIndex = 0;

    // 前フレームの角度
    private float previousAngle;

    // 一周判定用
    private bool canCountLap = false;

    void Start()
    {
        // 初期状態
        for (int i = 0; i < oldWall.Length; i++)
        {
            oldWall[i].SetActive(true);
            newWall[i].SetActive(false);
        }

        Vector3 dir = player.position - boss.position;

        previousAngle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

        if (previousAngle < 0)
            previousAngle += 360f;
    }

    void Update()
    {
        Debug.Log("Player = " + player.position);
        Debug.Log("Boss = " + boss.position);

        Vector3 dir = player.position - boss.position;
        Debug.Log("Dir = " + dir);

        float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

        if (angle < 0)
            angle += 360f;

        Debug.Log("Angle = " + angle);
        //Vector3 dir = player.position - boss.position;

        //float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

        if (angle < 0)
            angle += 360f;

        // デバッグ
        Debug.Log("Angle = " + angle);

        // 300°を超えたら、次に0°へ戻った時に一周とする
        if (angle > 300f)
        {
            canCountLap = true;
        }

        if (canCountLap && angle < 60f)
        {
            canCountLap = false;

            if (wallIndex < oldWall.Length)
            {
                Debug.Log("一周した！ 壁変更 " + wallIndex);

                oldWall[wallIndex].SetActive(false);
                newWall[wallIndex].SetActive(true);

                wallIndex++;
            }
        }

        previousAngle = angle;
    }
}