using UnityEngine;
using UnityEngine.Video;

public class PlayPVManager : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;

    private void Start()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.StopBGMSound();
        }
    }
}
