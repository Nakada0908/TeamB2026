using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossLapCount : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform boss;
    private float totalRotation = 0f;
    public int lapCount { get; private set; }
    private int maxLap = 3;
    private float previousAngle;

    [SerializeField] private GameObject flower;
    private Animator flowerAnime;

    void Start()
    {
        Vector3 dir = player.position - boss.position;

        previousAngle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

        if (previousAngle < 0)
        {
            previousAngle += 360f;
        }

        flowerAnime = flower.GetComponentInChildren<Animator>();
    }

    void Update()
    {
        Vector3 dir = player.position - boss.position;

        float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

        if (angle < 0)
        {
            angle += 360f;
        }

        //前フレームからどれだけ回転したか
        float delta = Mathf.DeltaAngle(previousAngle, angle);

        //回転量を加算
        totalRotation += delta;

        if (Mathf.Abs(totalRotation) >= 360f)
        {
            lapCount++;
            Debug.Log("現在" + lapCount + "周");

            //花を開いた状態にする
            flowerAnime.SetBool("isBloom", true);

            if (lapCount >= maxLap)
            {
                Debug.Log(maxLap+"周回しました!Endingへ");

                SceneManager.LoadScene("LookBack");
                return;
            }

            //次の1周を数えるためリセット
            totalRotation = 0f;
        }
        else if(Mathf.Abs(totalRotation) >= 180f)
        {
            //花を閉じた状態に戻す
            //同じ値を入れ直すだけなので、毎フレーム呼んでも問題ない
            flowerAnime.SetBool("isBloom", false);
        }

        previousAngle = angle;
    }

    //セーブ時のリセット用
    public void ResetRotation()
    {
        totalRotation = 0f;

        Vector3 dir = player.position - boss.position;

        previousAngle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

        if (previousAngle < 0)
        {
            previousAngle += 360f;
        }

        //セーブ地点は1周した直後なので、花は開いた状態に戻す
        flowerAnime.SetBool("isBloom", true);
    }
}
