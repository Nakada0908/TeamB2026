using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Video;

public class PlayPVManager : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private AudioMixer pvAudioMixer;

    private void Start()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.StopBGMSound();
        }

        pvAudioMixer.SetFloat("PVVolume", VolumeChenger.pvVolume);
    }
}
