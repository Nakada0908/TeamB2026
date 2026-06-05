using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SimpleTitleSequence : MonoBehaviour
{
    public GameObject nui;

    [Header("Camera")]
    public Transform cameraTransform;
    public Transform cameraTarget;
    public float cameraMoveTime = 2f;

    [Header("Animation")]
    public Animator nuiAnimator;
    public string animationTrigger = "Play";

    [Header("UI")]
    public GameObject titleLogo;
    public Image fadeImage;
    public float fadeTime = 1f;

    [Header("Scene")]
    public string nextSceneName;

    void Start()
    {
        titleLogo.SetActive(false);
        SetFadeAlpha(0f);
        StartCoroutine(Sequence());
    }

    IEnumerator Sequence()
    {
        yield return StartCoroutine(MoveCamera());

        nuiAnimator.SetTrigger(animationTrigger);

        nui = GameObject.Find("game_nuigurumi_V03");

        Destroy(nui);

        yield return new WaitForSeconds(1f);

        titleLogo.SetActive(true);

        yield return StartCoroutine(FadeOut());

        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator MoveCamera()
    {
        float t = 0f;
        Vector3 startPos = cameraTransform.position;
        Quaternion startRot = cameraTransform.rotation;

        while (t < cameraMoveTime)
        {
            t += Time.deltaTime;
            float rate = t / cameraMoveTime;

            cameraTransform.position = Vector3.Lerp(startPos, cameraTarget.position, rate);
            cameraTransform.rotation = Quaternion.Lerp(startRot, cameraTarget.rotation, rate);

            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        float t = 0f;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            SetFadeAlpha(t / fadeTime);
            yield return null;
        }
    }

    void SetFadeAlpha(float a)
    {
        Color c = fadeImage.color;
        c.a = a;
        fadeImage.color = c;
    }
}
