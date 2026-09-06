using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeadFadeInOut : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeOutSpeed;
    [SerializeField] private float fadeInSpeed;

    public IEnumerator FadeOut()
    {
        Color fadeColor = fadeImage.color;
        fadeColor.a = 0;
        fadeImage.color = fadeColor;
        while (fadeImage.color.a < 1)
        {
            fadeImage.color += new Color(0, 0, 0, fadeOutSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public IEnumerator FadeIn()
    {
        Color fadeColor = fadeImage.color;
        fadeColor.a = 1;
        fadeImage.color = fadeColor;
        while (fadeImage.color.a > 0)
        {
            fadeImage.color += new Color(0, 0, 0, -fadeInSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public IEnumerator FadeOutIn()
    {
        yield return StartCoroutine(FadeOut());//ˆÃ“]
        yield return StartCoroutine(FadeIn());//–¾“]
    }
}