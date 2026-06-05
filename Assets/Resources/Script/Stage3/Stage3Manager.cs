using UnityEngine;

public class Stage3Manager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(SoundManager.instance != null)
        {
            SoundManager.instance.PlayBGMSound("Stage3");
        }
    }
}
