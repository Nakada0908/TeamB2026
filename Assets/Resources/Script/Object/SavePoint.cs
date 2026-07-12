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
    [SerializeField] private float lightPower = 10f;
    [SerializeField] private float lightFadeTime = 1f;
    private Light lightComponent;

    private void Awake()
    {
        newPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);

        lightComponent = GetComponentInChildren<Light>();
        lightComponent.intensity = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerSaveManager.instance.SavePlayerPosition(newPos, savePointType);
            Debug.Log("New SavePoint: " + newPos);
            StartCoroutine(FadeInLight(lightPower, lightFadeTime));
            GetComponent<Collider>().enabled = false;
        }
    }

    private IEnumerator FadeInLight(float targetIntensity, float endTime)
    {
        float time = 0;

        while (time < endTime)
        {
            time += Time.deltaTime;
            float t = time / endTime;
            lightComponent.intensity = Mathf.Lerp(0f, targetIntensity, t);
            yield return null;
        }

        //最後正しく合わせる
        lightComponent.intensity = targetIntensity;
    }
}
