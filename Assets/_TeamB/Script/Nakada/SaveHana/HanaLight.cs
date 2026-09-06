using System.Collections;
using UnityEngine;


public class HanaLight : MonoBehaviour
{
    private Light lightComponent;

    private void Start()
    {
        lightComponent = GetComponentInChildren<Light>();
        lightComponent.intensity = 0f; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(FadeInLight(0.15f, 1f));
        }
    }

    private IEnumerator FadeInLight(float targetIntensity, float endTime)
    {
        float time = 0;

        while(Time.time < endTime)
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
