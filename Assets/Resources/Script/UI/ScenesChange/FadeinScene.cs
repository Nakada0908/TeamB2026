using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeinScene : MonoBehaviour
{
    public Image fadein_Image;
    public float speed = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SceneLoadIn());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SceneLoadIn()
    {
        yield return null;
        Color fadeColor = fadein_Image.color;
        fadeColor.a = 1;
        fadein_Image.color = fadeColor;
        while (fadein_Image.color.a > 0)
        {
            fadein_Image.color += new Color(0, 0, 0, -speed * Time.deltaTime);
            yield return null;
        }
        //fadein_Iamge.enabled = false;
    }
}
