using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButton : MonoBehaviour
{
    public float exitDuration = 3.0f;
    private float exitTimer;
    public string sceneName;
    // public GameObject resetButtonTestObject;

    public static ResetButton instance;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ResetCurrentScene();
        BackToTitle();
        ExitGame();

        if (Input.GetKeyUp(KeyCode.F3))
        {
            SceneManager.LoadScene("Stage3Eyes");
        }

    }

    private void ExitGame()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            exitTimer += Time.deltaTime;
            if (exitTimer >= exitDuration)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }
        else
        {
            exitTimer = 0;
        }
    }

    private void ResetCurrentScene()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void BackToTitle()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
