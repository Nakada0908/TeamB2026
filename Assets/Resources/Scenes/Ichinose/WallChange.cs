using UnityEngine;
using UnityEngine.SceneManagement;

public class WallChange : MonoBehaviour
{
    public GameObject[] oldWall;
    public GameObject[] newWall;


    private float totalRotation = 0f;
    private int index = 0;
    private int lapCount = 0;
    public Transform player;
    public Transform boss;

    private float previousAngle;
    private bool passedZero = false;


    void Start()
    {

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

    public void ChangeWall(int index)
    {
        Debug.Log("ChangeWall : " + index);
        Debug.Log("Old : " + oldWall[index].name);
        Debug.Log("New : " + newWall[index].name);

        oldWall[index].SetActive(false);
        newWall[index].SetActive(true);
    }

    void Update()
    {
        Vector3 dir = player.position - boss.position;

        float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

        if (angle < 0)
            angle += 360f;

        // 前フレームからどれだけ回転したか
        float delta = Mathf.DeltaAngle(previousAngle, angle);

        // 回転量を加算
        totalRotation += delta;

        // 1周したら壁を切り替え
        if (Mathf.Abs(totalRotation) >= 360f)
        {
            lapCount++;
            Debug.Log("現在" + lapCount + "周");
            if (index < oldWall.Length)
            {
                ChangeWall(index);
                index++;
            }
            if(lapCount >= 5)
            {
                Debug.Log("5周回しました!Endingへ");

                SceneManager.LoadScene("LookBack");
                return;
            }

            // 次の1周を数えるためリセット
            totalRotation = 0f;
        }

        previousAngle = angle;
    }
}