using UnityEngine;

public class StageBossManager : MonoBehaviour
{
    private string bgmName = "StageBoss";

    void Start()
    {
        SoundManager.instance.PlayBGMSound(bgmName);
    }
}
