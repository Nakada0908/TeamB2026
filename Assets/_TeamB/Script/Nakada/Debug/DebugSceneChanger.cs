using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugSceneChanger : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void AutoInitialize()
    {
        GameObject managerObj = new GameObject("DebugSceneChanger");
        managerObj.AddComponent<DebugSceneChanger>();
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        //ゲーム本編
        if(Input.GetKeyDown (KeyCode.F1))
        {
            SceneManager.LoadScene("Stage1Color");
        }
        if(Input.GetKeyDown (KeyCode.F2))
        {
            SceneManager.LoadScene("Stage2");
        }
        if (Input.GetKeyDown (KeyCode.F3))
        {
            SceneManager.LoadScene("Stage3Eyes");
        }
        if(Input.GetKeyDown(KeyCode.F4))
        {
            SceneManager.LoadScene("StageBoss");
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SceneManager.LoadScene("LookBack");
        }

        //F6～F12は未設定
    }
}
