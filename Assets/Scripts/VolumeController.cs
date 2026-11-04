
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public AudioSource musicSource;
    public Slider volumeSlider;

    void Start()
    {
        volumeSlider.onValueChanged.AddListener(SetVolume);
        SetVolume(volumeSlider.value);
    }

    void SetVolume(float value)
    {
        musicSource.volume = value;
    }
}
