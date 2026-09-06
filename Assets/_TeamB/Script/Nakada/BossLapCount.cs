using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BossLapCount : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform boss;
    private float totalRotation = 0f;
    public int lapCount { get; private set; }
    private int maxLap = 3;
    private float previousAngle;

    [Header("光加減の調整")]
    [SerializeField] private float emissionPower = 1f;
    [SerializeField] private float emissionFadeTime = 1f;

    private Material flowerMaterial;
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

        //全部の子オブジェクトのRendererを取得する
        Renderer[] renderers = flower.GetComponentsInChildren<Renderer>();

        //マテリアルを共通の１つにして、全員に配る
        flowerMaterial = renderers[0].material;
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].sharedMaterial = flowerMaterial;
        }

        //デフォでエミッションつけてるから消しておく
        flowerMaterial.SetColor("_EmissionColor", Color.black);
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

            flowerAnime.SetBool("isBloom", true);
            StartCoroutine(FadeInEmission());

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
            flowerAnime.SetBool("isBloom", false);
            ResetEmission();
        }

        previousAngle = angle;
    }

    //セーブ時のリセット用
    public void ResetRotation()
    {
        ResetEmission();

        totalRotation = 0f;

        Vector3 dir = player.position - boss.position;

        previousAngle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

        if (previousAngle < 0)
        {
            previousAngle += 360f;
        }

        flowerAnime.SetBool("isBloom", false);
    }

    private IEnumerator FadeInEmission()
    {
        float time = 0;

        while (time < emissionFadeTime)
        {
            time += Time.deltaTime;
            float t = time / emissionFadeTime;
            flowerMaterial.SetColor("_EmissionColor", Color.white * Mathf.Lerp(0f, emissionPower, t));
            yield return null;
        }

        //最後正しく合わせる
        flowerMaterial.SetColor("_EmissionColor", Color.white * emissionPower);
    }

    private void ResetEmission()
    {
        flowerMaterial.SetColor("_EmissionColor", Color.black);
    }
}
