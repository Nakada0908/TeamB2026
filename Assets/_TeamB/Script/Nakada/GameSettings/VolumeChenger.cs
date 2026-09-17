using UnityEngine;
using UnityEngine.UI;

public class VolumeChenger : MonoBehaviour
{
    public static VolumeChenger Instance { get; private set; }

    private const float MinVolumeDb = -20f;
    private const float MaxVolumeDb = 20f;
    private const float VolumeStepDb = 0.5f;

    [Header("Runtime")]
    [SerializeField] private bool persistentManager;

    [Header("Mixers (persistent manager only)")]
    [SerializeField] private AudioMixer pvAudioMixer;
    [SerializeField] private string pvVolumeParameter = "PVVolume";
    [SerializeField] private AudioMixer titleAudioMixer;
    [SerializeField] private string titleVolumeParameter = "MovieVolume";

    [Header("PV")]
    [SerializeField] private Slider pvVolumeSlider;
    [SerializeField] private Button pvUpButton;
    [SerializeField] private Button pvDownButton;
    [SerializeField] private Text pvVolumeText;

    [Header("Title (same volume as PV)")]
    [SerializeField] private Slider titleVolumeSlider;
    [SerializeField] private Button titleUpButton;
    [SerializeField] private Button titleDownButton;
    [SerializeField] private Text titleVolumeText;

    private float currentVolumeDb;

    private void Awake()
    {
        if (!persistentManager)
        {
            return;
        }

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        currentVolumeDb = GetCurrentVolume();
    }

    private void Start()
    {
        if (persistentManager)
        {
            ApplyMixerVolume(currentVolumeDb);
            return;
        }

        if (Instance == null)
        {
            Debug.LogError("常駐用のVolumeChengerがシーン内にありません。", this);
            enabled = false;
            return;
        }

        InitializeSlider(pvVolumeSlider);
        InitializeSlider(titleVolumeSlider);

        currentVolumeDb = Instance.currentVolumeDb;
        UpdateUi(currentVolumeDb);

        pvVolumeSlider.onValueChanged.AddListener(SetVolume);
        pvUpButton.onClick.AddListener(IncreaseVolume);
        pvDownButton.onClick.AddListener(DecreaseVolume);
        titleVolumeSlider.onValueChanged.AddListener(SetVolume);
        titleUpButton.onClick.AddListener(IncreaseVolume);
        titleDownButton.onClick.AddListener(DecreaseVolume);
    }

    private void OnDestroy()
    {
        if (!persistentManager)
        {
            pvVolumeSlider.onValueChanged.RemoveListener(SetVolume);
            pvUpButton.onClick.RemoveListener(IncreaseVolume);
            pvDownButton.onClick.RemoveListener(DecreaseVolume);
            titleVolumeSlider.onValueChanged.RemoveListener(SetVolume);
            titleUpButton.onClick.RemoveListener(IncreaseVolume);
            titleDownButton.onClick.RemoveListener(DecreaseVolume);
        }

        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void IncreaseVolume()
    {
        SetVolume(currentVolumeDb + VolumeStepDb);
    }

    public void DecreaseVolume()
    {
        SetVolume(currentVolumeDb - VolumeStepDb);
    }

    public void SetVolume(float volumeDb)
    {
        VolumeChenger manager = persistentManager ? this : Instance;
        if (manager == null)
        {
            return;
        }

        manager.currentVolumeDb = Mathf.Clamp(volumeDb, MinVolumeDb, MaxVolumeDb);
        manager.ApplyMixerVolume(manager.currentVolumeDb);

        if (!persistentManager)
        {
            currentVolumeDb = manager.currentVolumeDb;
            UpdateUi(currentVolumeDb);
        }
    }

    private static void InitializeSlider(Slider slider)
    {
        slider.minValue = MinVolumeDb;
        slider.maxValue = MaxVolumeDb;
        slider.wholeNumbers = false;
    }

    private float GetCurrentVolume()
    {
        if (pvAudioMixer.GetFloat(pvVolumeParameter, out float volumeDb))
        {
            return Mathf.Clamp(volumeDb, MinVolumeDb, MaxVolumeDb);
        }

        if (titleAudioMixer.GetFloat(titleVolumeParameter, out volumeDb))
        {
            return Mathf.Clamp(volumeDb, MinVolumeDb, MaxVolumeDb);
        }

        return 0f;
    }

    private void ApplyMixerVolume(float volumeDb)
    {
        pvAudioMixer.SetFloat(pvVolumeParameter, volumeDb);
        titleAudioMixer.SetFloat(titleVolumeParameter, volumeDb);
    }

    private void UpdateUi(float volumeDb)
    {
        pvVolumeSlider.SetValueWithoutNotify(volumeDb);
        titleVolumeSlider.SetValueWithoutNotify(volumeDb);
        pvVolumeText.text = $"{volumeDb:0.0} dB";
        titleVolumeText.text = $"{volumeDb:0.0} dB";

        bool canIncrease = volumeDb < MaxVolumeDb;
        bool canDecrease = volumeDb > MinVolumeDb;
        pvUpButton.interactable = canIncrease;
        titleUpButton.interactable = canIncrease;
        pvDownButton.interactable = canDecrease;
        titleDownButton.interactable = canDecrease;
    }
}
