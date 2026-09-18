using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayPVAnyKey : MonoBehaviour
{
    void Update()
    {
        if(Input.anyKeyDown)
        {
            SceneManager.LoadScene("Stage1Color");
        }
    }
}
