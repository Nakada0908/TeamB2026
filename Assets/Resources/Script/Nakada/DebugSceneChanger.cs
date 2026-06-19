using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugSceneChanger : MonoBehaviour
{
    public static DebugSceneChanger instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown (KeyCode.F1))
        {
            SceneManager.LoadScene("Stage1color");
        }
        if(Input.GetKeyDown (KeyCode.F2))
        {
            SceneManager.LoadScene("Stage2");
        }
        if (Input.GetKeyDown (KeyCode.F3))
        {
            SceneManager.LoadScene("Stage3esey");
        }
        if(Input.GetKeyDown(KeyCode.F4))
        {
            SceneManager.LoadScene("StageBoss");
        }
    }
}
