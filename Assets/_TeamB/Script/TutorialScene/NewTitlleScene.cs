using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Audio;

public class NewTitleScene: MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private VideoClip horrorVideo;
    [SerializeField] private VideoClip notHorrorVideo;
    public Image fadeImage;
    public float fadeTime = 1.0f;
    public string nextSceneName;

    [SerializeField] private AudioMixer titleAudioMixer;

    void Start()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.StopBGMSound();
        }
            //SoundManager.instance.StopBGMSound();
        fadeImage.color = new Color(0, 0, 0, 0);
        videoPlayer.loopPointReached += OnMovieEnd;

        //通常時はホラーを流す
        if (HorrorModeChenger.isHorrorMode)
        {
            videoPlayer.clip = horrorVideo;
        }
        else
        {
            videoPlayer.clip = notHorrorVideo;
        }
        videoPlayer.Play();

        titleAudioMixer.SetFloat("TitleVolume", VolumeChenger.titleVolume);
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
