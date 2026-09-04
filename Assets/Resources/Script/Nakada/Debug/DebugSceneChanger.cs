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

        //F6～F8は未設定

        //各自のシーン
        if (Input.GetKeyDown(KeyCode.F9))
        {
            SceneManager.LoadScene("LookBack");
        }
        if (Input.GetKeyDown(KeyCode.F10))
        {
            SceneManager.LoadScene("BossAtk");
        }
        if (Input.GetKeyDown(KeyCode.F11))
        {
            SceneManager.LoadScene("oonari");
        }
        if (Input.GetKeyDown(KeyCode.F12))
        {
            SceneManager.LoadScene("ichinose");
        }
    }
}
