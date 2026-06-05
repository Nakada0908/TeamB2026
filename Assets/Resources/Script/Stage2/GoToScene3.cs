using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToScene3 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SceneManager.LoadScene("Stage3Eyes");
        }
    }
}
