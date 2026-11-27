using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HorrorModeChenger : MonoBehaviour
{
    public static bool isHorrorMode { get; private set; } = true;
    [SerializeField] private Text horrorModeText;

    private void Start()
    {
        UpdateText();
    }

    //ƒ{ƒ^ƒ“‰Ÿ‚³‚ê‚½‚Æ‚«—p
    public void ChangeHorrorMode()
    {
        isHorrorMode = !isHorrorMode;
        UpdateText();
    }

    private void UpdateText()
    {
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
