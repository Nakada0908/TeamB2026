using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour
{
    public Image fadeImage;     // 黒画像
    public float fadeSpeed = 1; // 1秒で切り替わる

    private bool isFadeIn = false;
    private bool isFadeOut = false;
    //private float alpha = 1;    // 画像の透明度 0〜1
    public float fadeDuration = 1.0f;

    void Start()
    {
       
    }

    void Update()
    {
        if (isFadeIn)
        {
            FadeIn();
        }
        else if (isFadeOut)
        {
            FadeOut();
        }
    }

    ////フェードイン関数（明るくなる）
    //public void FadeIn()
    //{
    //    isFadeOut = false;
    //    isFadeIn = true;
    //}

    ////フェードアウト関数（暗くなる）
    //public void FadeOut()
    //{
    //    isFadeIn = false;
    //    isFadeOut = true;
    //}

    // αを変更する処理（毎回呼ばれる）
    private void SetAlpha(float a)
    {
        Color c = fadeImage.color;
        c.a = a;
        fadeImage.color = c;
    }

    //Black -> Clear
    public void FadeIn()
    {
        StartCoroutine(Fade(1f, 0f));
    }

    //Clear -> Black
    public void FadeOut()
    {
        StartCoroutine(Fade(0f, 1f));
    }

    private IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;
        Color c = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            c.a = Mathf.Lerp(from, to, t);
            fadeImage.color = c;
            yield return null;
        }

        c.a = to;
        fadeImage.color = c;
    }
}
