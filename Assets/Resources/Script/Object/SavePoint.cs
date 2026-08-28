using UnityEngine;
using System.Collections;

public enum ESavePointType
{
    None,
    YagiPoint1,
    YagiPoint2,
    YagiPoint3
}

public class SavePoint : MonoBehaviour
{
    [SerializeField] private ESavePointType savePointType;

    private Vector3 newPos;

    [Header("光加減の調整")]
    [SerializeField] private float emissionPower = 2f;
    [SerializeField] private float emissionFadeTime = 1f;
    
    private Material hanaMaterial;
    private Animator hanaAnimator;

    private void Awake()
    {
        newPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);

        //全部の子オブジェクトのRendererを取得する
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        //マテリアルは全員共通なので、コピーを1個だけ作って全員に配る
        hanaMaterial = renderers[0].material;
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].sharedMaterial = hanaMaterial;
        }

        //デフォでエミッションつけてるから消しておく
        hanaMaterial.SetColor("_EmissionColor", Color.black);

        hanaAnimator = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerSaveManager.instance.SavePlayerPosition(newPos, savePointType);
            Debug.Log("New SavePoint: " + newPos);
            GetComponent<Collider>().enabled = false;
            hanaAnimator.SetTrigger("Bloom");
            StartCoroutine(FadeInEmission());
        }
    }

    private IEnumerator FadeInEmission()
    {
        float time = 0;

        while (time < emissionFadeTime)
        {
            time += Time.deltaTime;
            float t = time / emissionFadeTime;
            hanaMaterial.SetColor("_EmissionColor", Color.white * Mathf.Lerp(0f, emissionPower, t));
            yield return null;
        }

        //最後正しく合わせる
        hanaMaterial.SetColor("_EmissionColor", Color.white * emissionPower);
    }
}
