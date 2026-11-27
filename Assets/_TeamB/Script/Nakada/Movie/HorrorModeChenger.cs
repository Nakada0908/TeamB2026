using UnityEngine;
using UnityEngine.UI;

public class HorrorModeChenger : MonoBehaviour
{
    public static HorrorModeChenger Instance { get; private set; }
    public bool isHorrorMode { get; private set; }
    [SerializeField] private Text horrorModeText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        isHorrorMode = true;
        horrorModeText.text = "Horror";
    }

    public void ChangeHorrorMode()
    {
        isHorrorMode = !isHorrorMode;

        if (isHorrorMode)
        {
            horrorModeText.text = "Horror";
        }
        else
        {
            horrorModeText.text = "NotHorror";
        }
    }
}
