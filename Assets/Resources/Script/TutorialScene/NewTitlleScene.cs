using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class NewTitleScene: MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Image fadeImage;
    public float fadeTime = 1.0f;
    public string nextSceneName;

    void Start()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.StopBGMSound();
        }
            //SoundManager.instance.StopBGMSound();
        fadeImage.color = new Color(0, 0, 0, 0);
        videoPlayer.loopPointReached += OnMovieEnd;
    }

    void OnMovieEnd(VideoPlayer vp)
    {
        StartCoroutine(FadeOutAndLoad());
    }

    IEnumerator FadeOutAndLoad()
    {
        float t = 0f;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            float a = t / fadeTime;
            fadeImage.color = new Color(0, 0, 0, a);
            yield return null;
        }

        SceneManager.LoadScene(nextSceneName);
    }
}
