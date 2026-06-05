using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine("NextScene");
    }

    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Stage2");
    }
}
