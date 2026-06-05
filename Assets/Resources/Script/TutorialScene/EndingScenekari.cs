using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneIntroManager : MonoBehaviour
{
    public TMP_Text messageText;
    public Image fadeImage;
    public float startDelay = 2f;
    public float textFadeDuration = 2f;
    public float textDisplayTime = 2f;
    public float fadeDuration = 1f;
    public string nextSceneName = "Stage1Color";

    void Start()
    {
        // 初期状態
        if (messageText != null) messageText.alpha = 0;

        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0;
            fadeImage.color = c;
        }

        StartCoroutine(IntroSequence());
    }

    System.Collections.IEnumerator IntroSequence()
    {
        yield return new WaitForSeconds(startDelay);

        float t = 0;
        while (t < textFadeDuration)
        {
            t += Time.deltaTime;
            if (messageText != null)
                messageText.alpha = Mathf.Lerp(0, 1, t / textFadeDuration);
            yield return null;
        }
        if (messageText != null) messageText.alpha = 1;

        yield return new WaitForSeconds(textDisplayTime);

        t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;

            if (fadeImage != null)
            {
                Color c = fadeImage.color;
                c.a = Mathf.Lerp(0, 1, t / fadeDuration);
                fadeImage.color = c;
            }

            if (messageText != null)
                messageText.alpha = Mathf.Lerp(1, 0, t / fadeDuration);

            yield return null;
        }

        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 1;
            fadeImage.color = c;
        }
        if (messageText != null) messageText.alpha = 0;

        SceneManager.LoadScene(nextSceneName);
    }
}
