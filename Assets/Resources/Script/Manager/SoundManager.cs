using System;
using UnityEngine;

public class SoundDatas
{
    public float masterSoundValue;
    public float BGMSoundValue;
    public float SFXSoundValue;
}

public class SoundManager : MonoBehaviour
{
    private SoundDatas soundDatas;
    public SoundDatas GetSoundDatas() { return soundDatas; }

    [SerializeField]
    private GameObject SFXAudioresource;
    [SerializeField]
    private GameObject BGMAudioresource;

    [SerializeField]
    public SFXResource[] SFX;
    [SerializeField]
    public BGMResource[] BGM;

    private AudioSource SFXFromChild;
    private AudioSource BGMFromChild;


    public static SoundManager instance;
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

        GetSFXresource();
        GetBGMResource();
    }

    void GetSFXresource()
    {
        SFXFromChild = SFXAudioresource.GetComponent<AudioSource>();
    }

    void GetBGMResource()
    {
        BGMFromChild = BGMAudioresource.GetComponent<AudioSource>();
    }

    [Serializable]
    public struct SFXResource
    {
        [SerializeField]
        public string SoundName;
        [SerializeField]
        public AudioClip audioClip;
    }
    [Serializable]
    public struct BGMResource
    {
        [SerializeField]
        public string SoundName;
        [SerializeField]
        public AudioClip audioClip;
    }

    // BGM Play
    public void PlayBGMSound(string _soundName)
    {
        foreach (var bgm in BGM)
        {
            if (bgm.SoundName == _soundName)
            {
                BGMFromChild.clip = bgm.audioClip;
                BGMFromChild.Play();
                return;
            }
        }
        Debug.Log(_soundName + "サウンドが無いです。");
    }

    public void PauseBGMSound()
    {
        if (BGMFromChild.isPlaying)
        {
            BGMFromChild.Pause();
        }
    }

    public void StopBGMSound()
    {
        if (BGMFromChild.isPlaying)
        {
            BGMFromChild.Stop();
        }
    }

    public void PlaySFXSound(string _soundName)
    {
        foreach (var sfx in SFX)
        {
            if (sfx.SoundName == _soundName)
            {
                SFXFromChild.clip = sfx.audioClip;
                SFXFromChild.Play();
                return;
            }
        }
        Debug.Log(_soundName + "サウンドが無いです。");
    }

    public void PauseSFXSound()
    {
        if (SFXFromChild.isPlaying)
        {
            BGMFromChild.Pause();
        }
    }

    public void StopSFXSound()
    {
        if (SFXFromChild.isPlaying)
        {
            BGMFromChild.Stop();
        }
    }
    //=================================================


}
