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
        //ゲーム本編
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
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SceneManager.LoadScene("LookBack");
        }

        //各自のシーン
        if (Input.GetKeyDown(KeyCode.F6))
        {
            SceneManager.LoadScene("BossAtk");
        }
        if (Input.GetKeyDown(KeyCode.F7))
        {
            SceneManager.LoadScene("oonari");
        }
        if (Input.GetKeyDown(KeyCode.F8))
        {
            SceneManager.LoadScene("ichinose");
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {
            SceneManager.LoadScene("BossAtk");
        }
    }
}
