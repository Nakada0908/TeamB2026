using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.VisualScripting;

public class FadeoutScene : MonoBehaviour
{
    public Image fadeout_Image;
    public float speed = 1.0f;
    public string SceneName = "";

    private bool isHit;

    private void Update()
    {
        if (isHit == true)
        {

            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Joystick1Button0))
            {
                StartCoroutine(SceneLoadout());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isHit = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isHit = false;
        }
    }

    IEnumerator SceneLoadout()
    {
        Color fadeColor = fadeout_Image.color;
        fadeColor.a = 0;
        fadeout_Image.color = fadeColor;
        while (fadeout_Image.color.a < 1)
        {
            fadeout_Image.color += new Color(0, 0, 0, speed * Time.deltaTime);
            yield return null;
        }
        SceneManager.LoadScene(SceneName);
    }

}
