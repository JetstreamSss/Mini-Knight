using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public AudioSource musicSource;
    public Slider volumeSlider;
    private float defaultVolume;

    private void Awake() => defaultVolume = volumeSlider.value;

    private void OnEnable()
    {
        float volume = PlayerPrefs.GetFloat(ProgressStore.MusicVolumeKey, defaultVolume);
        volumeSlider.SetValueWithoutNotify(volume);
        musicSource.volume = volume;
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    private void OnDisable() => volumeSlider.onValueChanged.RemoveListener(SetVolume);

    private void SetVolume(float value)
    {
        musicSource.volume = value;
        PlayerPrefs.SetFloat(ProgressStore.MusicVolumeKey, value);
        PlayerPrefs.Save();
    }
}
