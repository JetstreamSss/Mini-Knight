using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider volumeSlider;

    private void Start()
    {
        volumeSlider.SetValueWithoutNotify(MusicManager.ReadVolume("MusicVolume"));
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    private void SetVolume(float value) => MusicManager.SaveVolume("MusicVolume", value);
}
