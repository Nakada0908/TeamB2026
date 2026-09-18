using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeChenger : MonoBehaviour
{
    private const float minVolume = -20f;
    private const float maxVolume = 20f;
    private const float buttonStep = 0.1f;

    public static float titleVolume { get; private set; } = 0f;
    public static float pvVolume { get; private set; } = 0f;

    [Header("ミキサー")]
    [SerializeField] private AudioMixer titleAudioMixer;
    [SerializeField] private AudioMixer pvAudioMixer;
    [Header("Title")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Text nowVolumeText;
    [Header("PV")]
    [SerializeField] private Slider pvVolumeSlider;
    [SerializeField] private Text pvNowVolumeText;

    private void Start()
    {
        SetTitleVolume(titleVolume);
        SetPVVolume(pvVolume);
    }

    #region ボタンとスライダーの設定
    public void SetTitleVolume(float volume)
    {
        titleVolume = Mathf.Clamp(volume, minVolume, maxVolume);

        titleAudioMixer.SetFloat("TitleVolume", titleVolume);
        volumeSlider.SetValueWithoutNotify(titleVolume);
        nowVolumeText.text = $"{titleVolume:0.0} dB";
    }

    public void SetPVVolume(float volume)
    {
        pvVolume = Mathf.Clamp(volume, minVolume, maxVolume);

        pvAudioMixer.SetFloat("PVVolume", pvVolume);
        pvVolumeSlider.SetValueWithoutNotify(pvVolume);
        pvNowVolumeText.text = $"{pvVolume:0.0} dB";
    }

    public void TitleVolumeUpButton()
    {
        SetTitleVolume(titleVolume + buttonStep);
    }

    public void TitleVolumeDownButton()
    {
        SetTitleVolume(titleVolume - buttonStep);
    }

    public void PVVolumeUpButton()
    {
        SetPVVolume(pvVolume + buttonStep);
    }

    public void PVVolumeDownButton()
    {
        SetPVVolume(pvVolume - buttonStep);
    }
    #endregion
}
